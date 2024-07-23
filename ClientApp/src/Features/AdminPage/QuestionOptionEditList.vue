<script lang="ts" setup>
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import type { IQuestionOption } from '@/Entities/QuestionOption';
import { type Ref, ref } from 'vue';

const props = defineProps<{
  items?: IQuestionOption[];
}>();

const options: Ref<IQuestionOption[]> = ref(props.items || []);

const addOption = () => {
  options.value.push({
    title: 'New Option'
  });
};

const onCellEditComplete = (event: any) => {
  let { data, newValue, field } = event;

  if (field === 'description' || newValue.trim().length > 0) data[field] = newValue;
  else event.preventDefault();
};

const deleteOption = (item: IQuestionOption) => {
  const index = options.value.indexOf(item);
  if (index >= 0) options.value.splice(index, 1);
};
</script>

<template>
  <div>
    <Button icon="pi pi-plus" label="Add option" severity="success" @click="addOption" />
    <DataTable
      :value="options"
      dataKey="identifier"
      edit-mode="cell"
      sort-field="title"
      striped-rows
      @cell-edit-complete="onCellEditComplete"
    >
      <Column field="title" header="Title">
        <template #editor="{ data, field }">
          <InputText v-model="data[field]" autofocus />
        </template>
      </Column>
      <Column field="description" header="Description">
        <template #editor="{ data, field }">
          <InputText v-model="data[field]" autofocus />
        </template>
      </Column>
      <Column :exportable="false" class="actions" style="min-width: 8rem">
        <template #body="slotProps">
          <Button
            icon="pi pi-trash"
            outlined
            rounded
            severity="danger"
            @click="deleteOption(slotProps.data)"
          />
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<style scoped></style>
