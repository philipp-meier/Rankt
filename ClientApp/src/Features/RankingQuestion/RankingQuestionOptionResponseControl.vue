<script lang="ts" setup>
import Button from 'primevue/button';
import OrderList from 'primevue/orderlist';
import { useToast } from 'primevue/usetoast';
import type { IQuestionOption } from '@/Entities/QuestionOption';
import { type Ref, ref } from 'vue';
import type { IQuestion } from '@/Entities/Question';
import { API_ENDPOINTS } from '@/ApiEndpoints';

const props = defineProps<{
  question: IQuestion;
  username: string;
}>();

const emit = defineEmits(['voted']);

const toast = useToast();

const maxItemsToVote = props.question.maxSelectableItems ?? props.question.options!.length;
const options: Ref<IQuestionOption[]> = ref([...props.question.options!]);
const loading = ref(false);

const submit = () => {
  loading.value = true;

  fetch(`${API_ENDPOINTS.Questions}/${props.question.identifier!}/response`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      username: props.username,
      options: options.value.map((x) => {
        return {
          rank: options.value.indexOf(x),
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
      emit('voted');
    }
  });
};

const getPoints = (item: IQuestionOption) => {
  const position = options.value.indexOf(item);
  if (position + 1 > maxItemsToVote) return 0;

  return maxItemsToVote - position;
};

const getBadgeText = (item: IQuestionOption) => {
  const points = getPoints(item);
  return points !== 1 ? `${points} points` : `${points} point`;
};
</script>

<template>
  <div class="card">
    <h3>Please place the items in order of your preference:</h3>
    <br />
    <OrderList
      v-model="options"
      :meta-key-selection="true"
      dataKey="identifier"
      listStyle="height:auto"
    >
      <template #item="slotProps">
        <div>
          <div class="headline">
            <span :class="getPoints(slotProps.item) == 0 ? 'badge no-points' : 'badge'">{{
              getBadgeText(slotProps.item)
            }}</span>
            <span class="text">{{ slotProps.item.title }}</span>
          </div>
          <div v-if="slotProps.item.description">
            <span>{{ slotProps.item.description }}</span>
          </div>
        </div>
      </template>
    </OrderList>
    <br />
    <Button :loading="loading" label="Submit" @click="submit" />
  </div>
</template>

<style scoped>
div.headline {
  display: flex;
  align-items: center;

  span.text {
    font-weight: bold;
  }

  span.badge {
    margin-right: 0.25em;
    border: 1px solid black;
    border-radius: 10px;
    padding-left: 5px;
    padding-right: 5px;
    font-size: 0.75em;
    background: #10b981a6;

    &.no-points {
      background: #ef44446b;
    }
  }
}
</style>
