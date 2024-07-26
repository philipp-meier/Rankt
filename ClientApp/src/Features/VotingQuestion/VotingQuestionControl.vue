<script lang="ts" setup>
import Button from 'primevue/button';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Checkbox from 'primevue/checkbox';
import InlineMessage from 'primevue/inlinemessage';
import type { IQuestion } from '@/Entities/Question';
import { computed, onBeforeMount, ref, type Ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import type { IQuestionOption } from '@/Entities/QuestionOption';
import { API_ENDPOINTS } from '@/ApiEndpoints';
import { QuestionService } from '@/Shared/Services/QuestionService';

const props = defineProps<{
  question: IQuestion;
  username: string;
}>();

const toast = useToast();
const question: Ref<IQuestion> = ref(props.question);

interface IRowData {
  username: string;
  choice: string[];
  isInput: boolean;
}

const result: Ref<any | null> = ref(null);

const hasVoted = ref(QuestionService.hasVoted(question.value.identifier!, props.username));
const options: Ref<IQuestionOption[]> = ref([...props.question.options!]);
const loading = ref(false);

onBeforeMount(async () => {
  await tryFetchResult();

  const intervalId = setInterval(async () => {
    if (result.value !== null || (await tryFetchResult())) {
      clearInterval(intervalId);
    }
  }, 5000);
});

const rowData = computed<IRowData[]>(() => {
  const data = (result.value?.responses || question.value.responses || []).map((r: any) => ({
    username: r.username!,
    choice: r.choice!,
    isInput: false
  }));

  if (!hasVoted.value && result.value === null) {
    data.push({
      username: props.username,
      choice: [],
      isInput: true
    });
  }

  return data;
});

const columnData = ref(
  options.value.map((o, index) => ({
    identifier: o.identifier,
    title: o.title,
    description: o.description,
    index
  }))
);

const submit = () => {
  loading.value = true;

  fetch(`${API_ENDPOINTS.Questions}/${props.question.identifier!}/response`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      username: props.username,
      options: userInput.value
        .filter((x) => x.value)
        .map((x) => {
          return {
            identifier: x.identifier
          };
        })
    })
  }).then(async (resp) => {
    loading.value = false;
    if (resp.ok) {
      toast.add({
        severity: 'success',
        summary: 'Successful',
        detail: 'Vote was submitted',
        life: 3000
      });

      QuestionService.setVoted(question.value!.identifier!, props.username!);
      hasVoted.value = true;
    }
  });
};

const userInput = ref([
  ...columnData.value.map((o) => {
    return {
      identifier: o.identifier,
      value: false
    };
  })
]);

const responseStats = options.value.map((o) => {
  const responseCount =
    rowData.value?.filter((x) => x.choice.indexOf(o.identifier!) >= 0)?.length ?? 0;
  return {
    option: o,
    responseCount
  };
});

const maxResponses = Math.max(...responseStats.map((x) => x.responseCount));

const tryFetchResult = async (): Promise<boolean> => {
  const resultValue = await QuestionService.getResult(question.value.identifier!);
  if (resultValue) {
    result.value = resultValue;
    return true;
  }
  return false;
};
</script>

<template>
  <div class="card">
    <div v-if="result === null && hasVoted">
      <p>Thank you for voting! As soon as the result is available, this page will be refreshed.</p>
      <br />
      <InlineMessage severity="info"
        >Waiting for the initiator to complete the survey...
      </InlineMessage>
    </div>

    <div v-if="result !== null">
      <p>This survey was closed by the initiator.</p>
      <p>The following result is based on {{ result.responseCount }} responses.</p>

      <br />
      <h3>Result</h3>
    </div>

    <DataTable
      v-if="!hasVoted || result !== null"
      :value="rowData"
      showGridlines
      tableStyle="min-width: 50rem"
    >
      <Column field="username" header="User"></Column>
      <Column
        v-for="col of columnData"
        :key="col.identifier"
        field="choice"
        style="text-align: center"
      >
        <template #header>
          <div class="header">
            <div class="date-header">
              {{ col.title }}
            </div>
            <div class="description" v-if="col.description">
              {{ col.description }}
            </div>
            <div
              :style="
                maxResponses > 0 &&
                responseStats.find((x) => x.option.identifier == col.identifier)?.responseCount ==
                  maxResponses
                  ? 'color: green; font-weight: bold'
                  : undefined
              "
            >
              <i class="pi pi-user"></i>
              {{ responseStats.find((x) => x.option.identifier == col.identifier)?.responseCount }}
            </div>
          </div>
        </template>
        <template #body="{ data }">
          <i
            v-if="!data.isInput"
            :class="data.choice.indexOf(col.identifier) >= 0 ? 'pi pi-check' : 'pi pi-times'"
          ></i>
          <Checkbox v-else v-model="userInput[col.index].value" binary variant="filled" />
        </template>
      </Column>
    </DataTable>
    <br />
    <Button v-if="!hasVoted && result === null" :loading="loading" label="Submit" @click="submit" />
  </div>
</template>

<style scoped>
i.pi-check {
  color: green;
}

i.pi-times {
  color: red;
}

div.header {
  text-align: center;
  width: 100%;
}

div.header > div.date-header {
  font-size: 1.1em;
}

div.header > div.description {
  font-size: 0.9em;
}
</style>
