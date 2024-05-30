<script lang="ts" setup>
import { useRoute, useRouter } from 'vue-router';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import { computed, onBeforeMount, type Ref, ref } from 'vue';
import type { IRankingQuestion } from '@/entities/RankingQuestion';
import RankingQuestionOptionResponseControl from '@/components/RankingQuestionOptionResponseControl.vue';
import { QuestionService } from '@/services/QuestionService';
import { API_ENDPOINTS } from '@/ApiEndpoints';
import TextService from '../services/TextService';
import Chart from 'primevue/chart';
import InlineMessage from 'primevue/inlinemessage';

const route = useRoute();
const router = useRouter();

const surveyIdentifier = route.query.id?.toString() || '-1';

const question: Ref<IRankingQuestion | null> = ref(null);
const result: Ref<any | null> = ref(null);
const waitingForResults = ref(false);

onBeforeMount(async () => {
  if (await checkIfResultsAvailable()) {
    waitingForResults.value = false;
    result.value = await getResult();
  }

  if (await checkIfQuestionAvailable()) {
    const resp = await fetch(`${API_ENDPOINTS.Questions}/${surveyIdentifier}`);
    if (resp.ok) {
      question.value = await resp.json();
    }
  } else {
    // Survey could not be found. Redirect to the start page.
    // Since the ID is validated on the homepage, the user must have used an invitation link to get here.
    await router.push({ name: 'home' });
  }
});

const username: Ref<string | null> = ref(route.query.username?.toString() ?? null);
const usernameAvailable = ref(!!username.value);
const isInvalidUsername = ref(false);

const loading = ref(false);
const showQuestion = computed(
  () =>
    question.value &&
    usernameAvailable.value &&
    (!!username.value || QuestionService.hasVoted(question.value.identifier!, username.value!))
);

const continueToQuestion = () => {
  if (!username.value) {
    isInvalidUsername.value = true;
    return;
  }

  // If the user has already voted, we can directly execute the onVoted event and wait for the result.
  if (QuestionService.hasVoted(question.value!.identifier!, username.value)) {
    onVoted();
  }

  usernameAvailable.value = true;
  isInvalidUsername.value = false;
  loading.value = false;
};

const onVoted = () => {
  QuestionService.setVoted(question.value!.identifier!, username!.value!);
  waitingForResults.value = true;

  const intervalId = setInterval(async () => {
    if (await checkIfResultsAvailable()) {
      clearInterval(intervalId);
      result.value = await getResult();
      waitingForResults.value = false;
    }
  }, 5000);
};

const checkIfQuestionAvailable = async (): Promise<boolean> => {
  const resp = await fetch(`${API_ENDPOINTS.Questions}/${surveyIdentifier!}`, { method: 'HEAD' });
  return resp.ok;
};
const checkIfResultsAvailable = async (): Promise<boolean> => {
  const resp = await fetch(`${API_ENDPOINTS.Questions}/${surveyIdentifier!}/result`, {
    method: 'HEAD'
  });
  return resp.ok;
};
const getResult = async (): Promise<any> => {
  const resp = await fetch(`${API_ENDPOINTS.Questions}/${surveyIdentifier!}/result`, {
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
  <div class="question-container">
    <h2>{{ question?.title }}</h2>
    <h4>{{ TextService.formatDateFromString(question?.created) }}</h4>
    <br />

    <div
      v-if="result === null && !waitingForResults && !showQuestion && question"
      class="username-container"
    >
      <div>
        <InputText
          id="username"
          v-model="username"
          :invalid="isInvalidUsername"
          autofocus
          placeholder="Username"
          @keyup.enter="continueToQuestion"
        />
        <Button :loading="loading" label="Continue" @click="continueToQuestion" />
      </div>
    </div>

    <RankingQuestionOptionResponseControl
      v-if="result === null && !waitingForResults && showQuestion && question"
      :question="question"
      :username="username!"
      @voted="onVoted"
    />

    <div v-if="result === null && waitingForResults">
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
      <Chart :data="resultChartData" :options="resultChartOptions" type="bar" />
    </div>
  </div>
</template>

<style scoped>
div.question-container {
  margin: 0.5em 1em 0 1em;

  input {
    margin-right: 0.25em;
  }
}
</style>
