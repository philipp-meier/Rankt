<script lang="ts" setup>
import Button from 'primevue/button';
import Toolbar from 'primevue/toolbar';
import DataTable from 'primevue/datatable';
import IconField from 'primevue/iconfield';
import InputIcon from 'primevue/inputicon';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Column from 'primevue/column';
import Dialog from 'primevue/dialog';
import Tag from 'primevue/tag';
import Select from 'primevue/select';
import Dropdown from 'primevue/dropdown';
import { onBeforeMount, type Ref, ref } from 'vue';
import { FilterMatchMode } from 'primevue/api';
import { useToast } from 'primevue/usetoast';
import QuestionOptionEditList from '@/Features/AdminPage/QuestionOptionEditList.vue';
import type { IQuestion } from '@/Entities/Question';
import { API_ENDPOINTS } from '@/ApiEndpoints';
import TextService from '@/Shared/Services/TextService';

const questions: Ref<IQuestion[]> = ref([]);
const availableStatuses: Ref<{ label: string; value: string }[]> = ref([]);

onBeforeMount(async () => {
  fetch(API_ENDPOINTS.ManagementQuestions, { method: 'GET' }).then(async (resp) => {
    if (resp.ok) {
      const jsonResponse = await resp.json();
      questions.value = jsonResponse.questions;
      availableStatuses.value = jsonResponse.availableStatusOptions.map((x: any) => {
        return {
          label: x.name,
          value: x.identifier
        };
      });
    }
  });
});

const selectedQuestions: Ref<IQuestion[] | null> = ref([]);
const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS }
});

const getStatusIdentifierFromName = (statusName: string) => {
  const status = availableStatuses.value.find((x) => x.label == statusName);
  if (!status) return undefined;

  return status.value;
};
const getStatusLabelFromName = (statusName: string): string | undefined => {
  return getStatusLabel(getStatusIdentifierFromName(statusName)!);
};
const getStatusLabel = (statusIdentifier: string): string | undefined => {
  switch (statusIdentifier) {
    case 'NEW':
      return 'info';

    case 'PUBLISHED':
      return 'success';

    case 'INPROGRESS':
      return 'success';

    case 'COMPLETED':
      return 'secondary';

    case 'ARCHIVED':
      return 'contrast';

    default:
      return undefined;
  }
};

const toast = useToast();

const questionDialog = ref(false);
const deleteQuestionDialog = ref(false);
const deleteQuestionsDialog = ref(false);

const questionEditModel: Ref<IQuestion> = ref({});

const submitted = ref(false);
const isAddMode = ref(false);

const openNew = () => {
  isAddMode.value = true;
  questionType.value = questionTypes[0];
  questionEditModel.value = { options: [] };
  submitted.value = false;
  questionDialog.value = true;
};
const hideDialog = () => {
  questionDialog.value = false;
  submitted.value = false;
};
const saveQuestion = () => {
  submitted.value = true;

  if (!isValidMaxSelectableItems(questionEditModel.value)) return;

  questionEditModel.value.type = questionType.value.id;

  if (questionEditModel?.value.title?.trim()) {
    if (questionEditModel.value.identifier) {
      fetch(`${API_ENDPOINTS.ManagementQuestions}/${questionEditModel.value.identifier}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(questionEditModel.value)
      }).then(async (resp) => {
        if (resp.ok) {
          questions.value[findIndexByIdentifier(questionEditModel.value.identifier!)] =
            questionEditModel.value;
          toast.add({
            severity: 'success',
            summary: 'Successful',
            detail: 'Question Updated',
            life: 3000
          });

          questionDialog.value = false;
          questionEditModel.value = {};
        }
        // TODO: toast message?
      });
    } else {
      fetch(API_ENDPOINTS.ManagementQuestions, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(questionEditModel.value)
      }).then(async (resp) => {
        if (resp.ok) {
          const jsonResponse = await resp.json();
          questionEditModel.value.identifier = jsonResponse.identifier;
          questionEditModel.value.status = jsonResponse.status;
          questionEditModel.value.created = jsonResponse.created;
          questionEditModel.value.responseCount = 0;

          questions.value.push(questionEditModel.value);
          toast.add({
            severity: 'success',
            summary: 'Successful',
            detail: 'Question Created',
            life: 3000
          });

          questionDialog.value = false;
          questionEditModel.value = {};
        }
        // TODO: toast message?
      });
    }
  }
};

const editQuestion = (selectedQuestion: IQuestion) => {
  isAddMode.value = false;
  questionType.value = questionTypes.find((x) => x.id == selectedQuestion.type) ?? questionTypes[0];

  // Lazy-load options
  fetch(`${API_ENDPOINTS.Questions}/${selectedQuestion.identifier}`, { method: 'GET' }).then(
    async (resp) => {
      if (resp.ok) {
        questionEditModel.value = await resp.json();
        questionDialog.value = true;
      }
      // TODO: toast message?
    }
  );
};

const confirmDeleteQuestion = (selectedQuestion: IQuestion) => {
  questionEditModel.value = selectedQuestion;
  deleteQuestionDialog.value = true;
};

const confirmDeleteSelected = () => {
  deleteQuestionsDialog.value = true;
};

const deleteQuestion = () => {
  fetch(`${API_ENDPOINTS.ManagementQuestions}/${questionEditModel.value.identifier}`, {
    method: 'DELETE'
  }).then(async (resp) => {
    if (resp.ok) {
      questions.value = questions.value.filter(
        (val) => val.identifier !== questionEditModel.value.identifier
      );
      deleteQuestionDialog.value = false;
      toast.add({
        severity: 'success',
        summary: 'Successful',
        detail: 'Question Deleted',
        life: 3000
      });
    }
    // TODO: toast message?
  });
};

const deleteSelectedQuestions = () => {
  if (selectedQuestions.value) {
    selectedQuestions.value?.forEach(async (question) => {
      const resp = await fetch(`${API_ENDPOINTS.ManagementQuestions}/${question.identifier}`, {
        method: 'DELETE'
      });
      if (resp.ok) {
        questions.value = questions.value.filter((val) => val.identifier !== question.identifier);
        toast.add({
          severity: 'success',
          summary: 'Successful',
          detail: `Question "${question.title}" deleted.`,
          life: 3000
        });
      } else {
        toast.add({
          severity: 'error',
          summary: 'Error',
          detail: `Deleting question "${question.title}" failed.`,
          life: 3000
        });
      }
    });
  }

  deleteQuestionsDialog.value = false;
  selectedQuestions.value = null;
};

const findIndexByIdentifier = (identifier: string) => {
  let index = -1;
  for (let i = 0; i < questions.value.length; i++) {
    if (questions.value[i].identifier === identifier) {
      index = i;
      break;
    }
  }

  return index;
};
const onCellEditComplete = (event: any) => {
  let { data, newValue, field } = event;
  if (field !== 'status' || data.status === newValue) {
    return;
  }

  // No change detected
  const currentStatusIdentifier = availableStatuses.value.find(
    (x) => x.label === data.status
  )?.value;
  if (currentStatusIdentifier === newValue) {
    return;
  }

  fetch(`${API_ENDPOINTS.ManagementQuestions}/${data.identifier}/status`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ identifier: newValue })
  }).then(async (resp) => {
    if (resp.ok) {
      data.status = availableStatuses.value.find((x) => x.value === newValue)?.label;
      toast.add({
        severity: 'success',
        summary: 'Successful',
        detail: `Status was changed.`,
        life: 3000
      });
    } else {
      let errorMessage = 'Status change failed';
      try {
        errorMessage = (await resp.json()).error;
      } catch (err) {
        console.log(err);
      }

      toast.add({ severity: 'error', summary: 'Error', detail: errorMessage, life: 3000 });
    }
  });
};

const copyToClipboard = (identifier: string) => {
  navigator.clipboard.writeText(identifier);
  toast.add({
    severity: 'info',
    summary: 'Copied to clipboard',
    detail: `The invitation ID "${identifier}" was copied to the clipboard.`,
    life: 1000
  });
};

const isValidMaxSelectableItems = (question: IQuestion): boolean => {
  if (!question.options || !question.maxSelectableItems) return true;

  return question.maxSelectableItems <= question.options.length;
};

const questionTypes = [
  {
    id: 'RQ',
    name: 'Ranking Question'
  },
  {
    id: 'V',
    name: 'Voting'
  }
];

const questionType = ref(questionTypes[0]);
</script>

<template>
  <div class="card question-management">
    <Toolbar class="toolbar">
      <template #start>
        <Button icon="pi pi-plus" label="New" severity="success" @click="openNew" />
        <Button
          :disabled="!selectedQuestions || !selectedQuestions.length"
          icon="pi pi-trash"
          label="Delete"
          severity="danger"
          @click="confirmDeleteSelected"
        />
      </template>
    </Toolbar>

    <DataTable
      ref="dt"
      v-model:selection="selectedQuestions"
      :filters="filters"
      :value="questions"
      dataKey="identifier"
      edit-mode="cell"
      sort-field="title"
      striped-rows
      @cell-edit-complete="onCellEditComplete"
    >
      <template #header>
        <div class="table-header">
          <h4>Manage Questions</h4>
          <IconField iconPosition="left">
            <InputIcon>
              <i class="pi pi-search" />
            </InputIcon>
            <InputText v-model="filters['global'].value" placeholder="Search..." />
          </IconField>
        </div>
      </template>

      <Column headerStyle="width: 3rem" selectionMode="multiple"></Column>
      <Column field="title" header="Title" style="min-width: 16rem"></Column>
      <Column field="status" header="Status" style="min-width: 16rem">
        <template #editor="{ data, field }">
          <Dropdown
            v-model="data[field]"
            :options="availableStatuses"
            optionLabel="label"
            optionValue="value"
            placeholder="Select a Status"
          >
            <template #option="slotProps">
              <Tag
                :severity="getStatusLabel(slotProps.option.value)"
                :value="slotProps.option.label"
              />
            </template>
          </Dropdown>
        </template>
        <template #body="slotProps">
          <Tag
            :severity="getStatusLabelFromName(slotProps.data.status!)"
            :value="slotProps.data.status"
          />
          <i class="pi pi-pencil" style="font-size: 0.6rem; margin-left: 0.25rem" />
        </template>
      </Column>
      <Column field="identifier" header="Invitation ID" style="min-width: 16rem">
        <template #body="slotProps">
          <div v-if="getStatusIdentifierFromName(slotProps.data.status) !== 'NEW'">
            <router-link :to="'/question?id=' + slotProps.data.identifier" target="_blank">
              <Button :label="slotProps.data.identifier" cli link />
            </router-link>
            <Button
              icon="pi pi-clipboard"
              outlined
              rounded
              severity="secondary"
              @click="copyToClipboard(slotProps.data.identifier)"
            />
          </div>
        </template>
      </Column>
      <Column field="responseCount" header="Responses" style="min-width: 10rem"></Column>
      <Column field="created" header="Create Date" style="min-width: 10rem">
        <template #body="slotProps">
          {{ TextService.formatDateFromString(slotProps.data.created) }}
        </template>
      </Column>
      <Column :exportable="false" class="actions" style="min-width: 8rem">
        <template #body="slotProps">
          <Button
            :disabled="getStatusIdentifierFromName(slotProps.data.status) !== 'NEW'"
            icon="pi pi-pencil"
            outlined
            rounded
            @click="editQuestion(slotProps.data)"
          />
          <Button
            icon="pi pi-trash"
            outlined
            rounded
            severity="danger"
            @click="confirmDeleteQuestion(slotProps.data)"
          />
        </template>
      </Column>
    </DataTable>

    <!-- see https://primevue.org/datatable/#single_row_selection for dropdowns etc. -->
    <Dialog
      v-model:visible="questionDialog"
      :modal="true"
      :style="{ width: '450px' }"
      class="p-fluid"
      header="Question Details"
    >
      <div class="field">
        <label for="name">Name</label>
        <InputText
          id="name"
          v-model.trim="questionEditModel.title"
          :invalid="submitted && !questionEditModel.title"
          autofocus
          required="true"
        />
        <small v-if="submitted && !questionEditModel.title" class="p-error"
          >Title is required.</small
        >
      </div>

      <div class="field">
        <label for="maxSelectableItems">Max. Selectable Items</label>
        <InputNumber
          id="maxSelectableItems"
          :min="1"
          :invalid="submitted && !isValidMaxSelectableItems(questionEditModel)"
          v-model.trim="questionEditModel.maxSelectableItems"
        />
        <small v-if="submitted && !isValidMaxSelectableItems(questionEditModel)" class="p-error"
          >Number must not be greater than the number of available options.</small
        >
      </div>
      <div class="field">
        <label for="questionTypeSelection">Type</label>
        <Select
          id="questionTypeSelection"
          v-model="questionType"
          :options="questionTypes"
          optionLabel="name"
          :disabled="!isAddMode"
        ></Select>
      </div>

      <br />
      <QuestionOptionEditList :items="questionEditModel.options" />

      <template #footer>
        <Button icon="pi pi-times" label="Cancel" text @click="hideDialog" />
        <Button icon="pi pi-check" label="Save" text @click="saveQuestion" />
      </template>
    </Dialog>

    <Dialog
      v-model:visible="deleteQuestionDialog"
      :modal="true"
      :style="{ width: '450px' }"
      header="Confirm"
    >
      <div class="confirmation-content">
        <span v-if="questionEditModel"
          >Are you sure you want to delete <b>{{ questionEditModel.title }}</b
          >?</span
        >
      </div>
      <template #footer>
        <Button icon="pi pi-times" label="No" text @click="deleteQuestionDialog = false" />
        <Button icon="pi pi-check" label="Yes" text @click="deleteQuestion" />
      </template>
    </Dialog>

    <Dialog
      v-model:visible="deleteQuestionsDialog"
      :modal="true"
      :style="{ width: '450px' }"
      header="Confirm"
    >
      <div class="confirmation-content">
        <span v-if="questionEditModel"
          >Are you sure you want to delete the selected questions?</span
        >
      </div>
      <template #footer>
        <Button icon="pi pi-times" label="No" text @click="deleteQuestionsDialog = false" />
        <Button icon="pi pi-check" label="Yes" text @click="deleteSelectedQuestions" />
      </template>
    </Dialog>
  </div>
</template>

<style scoped>
div.question-management {
  margin: 0.5em 1em 0 1em;

  .toolbar button {
    margin-right: 0.25em;
  }

  div.table-header {
    display: flex;
    justify-content: space-between;
    align-items: center;

    h4 {
      margin: 0;
    }
  }

  .actions > button {
    margin-right: 0.25em;
  }
}
</style>
