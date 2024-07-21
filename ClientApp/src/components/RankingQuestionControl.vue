<script lang="ts" setup>
import { computed, onBeforeMount, type Ref, ref } from 'vue';
import type { IQuestion } from '@/entities/Question';
import { QuestionService } from '@/services/QuestionService';
import { API_ENDPOINTS } from '@/ApiEndpoints';
import Chart from 'primevue/chart';
import InlineMessage from 'primevue/inlinemessage';
import RankingQuestionOptionResponseControl from '@/components/RankingQuestionOptionResponseControl.vue';

const props = defineProps<{
  question: IQuestion;
  username: string;
}>();

const question: Ref<IQuestion> = ref(props.question);
const result: Ref<any | null> = ref(null);
const isInitialized = ref(false);
const username: Ref<string | null> = ref(props.username);
const hasVoted = ref(QuestionService.hasVoted(question.value.identifier!, username.value!));

onBeforeMount(async () => {
  await tryFetchResult();

  const intervalId = setInterval(async () => {
    if (!!result.value || (await tryFetchResult())) {
      clearInterval(intervalId);
    }
  }, 5000);

  isInitialized.value = true;
});

const onVoted = () => {
  QuestionService.setVoted(question.value!.identifier!, username!.value!);
  hasVoted.value = true;
};

const tryFetchResult = async (): Promise<boolean> => {
  if (await checkIfResultsAvailable()) {
    result.value = await getResult();
    return true;
  }

  return false;
};

const checkIfResultsAvailable = async (): Promise<boolean> => {
  const resp = await fetch(`${API_ENDPOINTS.Questions}/${question.value?.identifier!}/result`, {
    method: 'HEAD'
  });
  return resp.ok;
};
const getResult = async (): Promise<any> => {
  const resp = await fetch(`${API_ENDPOINTS.Questions}/${question.value?.identifier!}/result`, {
    method: 'GET'
  });
  return await resp.json();
};

const resultChartData = computed(() => {
  return {
    labels: result.value!.items.map((x: any) => x.title),
    datasets: [
      {
        label: 'Result',
        data: result.value!.items.map((x: any) => x.score)
      }
    ]
  };
});

const resultChartOptions = {
  indexAxis: 'y',
  plugins: {
    legend: {
      display: false
    }
  }
};
</script>

<template>
  <div>
    <p v-if="!isInitialized">Attempting to join the vote...</p>
    <template v-else>
      <RankingQuestionOptionResponseControl
        v-if="result === null && !hasVoted"
        :question="question!"
        :username="username!"
        @voted="onVoted"
      />

      <div v-if="result === null && hasVoted">
        <p>
          Thank you for voting! As soon as the result is available, this page will be refreshed.
        </p>
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
        <Chart :data="resultChartData" :options="resultChartOptions" type="bar" />
      </div>
    </template>
  </div>
</template>

<style scoped></style>
