<script lang="ts" setup>
import { useRoute, useRouter } from 'vue-router';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import { computed, onBeforeMount, type Ref, ref } from 'vue';
import type { IQuestion } from '@/Entities/Question';
import { QuestionService } from '@/Shared/Services/QuestionService';
import { API_ENDPOINTS } from '@/ApiEndpoints';
import TextService from '@/Shared/Services/TextService';
import RankingQuestionControl from '@/Features/RankingQuestion/RankingQuestionControl.vue';
import VotingQuestionControl from '@/Features/VotingQuestion/VotingQuestionControl.vue';

const route = useRoute();
const router = useRouter();

const surveyIdentifier = route.query.id?.toString() || '-1';

const question: Ref<IQuestion | null> = ref(null);
const resultsAvailable = ref(false);

onBeforeMount(async () => {
  if (await checkIfQuestionAvailable()) {
    const resp = await fetch(`${API_ENDPOINTS.Questions}/${surveyIdentifier}`);
    if (resp.ok) {
      question.value = await resp.json();
    }

    if (await checkIfResultsAvailable()) {
      resultsAvailable.value = true;
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

const showQuestion = computed(
  () =>
    question.value &&
    (usernameAvailable.value ||
      (usernameAvailable.value &&
        QuestionService.hasVoted(question.value.identifier!, username.value!)) ||
      resultsAvailable.value)
);

const continueToQuestion = () => {
  if (!username.value) {
    isInvalidUsername.value = true;
    return;
  }

  usernameAvailable.value = true;
  isInvalidUsername.value = false;
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
</script>

<template>
  <div class="question-container">
    <p v-if="question == null">Loading question...</p>
    <template v-else>
      <h2>{{ question?.title }}</h2>
      <h4>{{ TextService.formatDateFromString(question?.created) }}</h4>
      <br />

      <div v-if="!showQuestion" class="username-container">
        <div>
          <InputText
            id="username"
            v-model="username"
            :invalid="isInvalidUsername"
            autofocus
            placeholder="Username"
            @keyup.enter="continueToQuestion"
          />
          <Button label="Continue" @click="continueToQuestion" />
        </div>
      </div>

      <RankingQuestionControl
        v-if="showQuestion && question.type == 'RQ'"
        :question="question!"
        :username="username!"
      />
      <VotingQuestionControl
        v-if="showQuestion && question.type == 'V'"
        :question="question!"
        :username="username!"
      />
    </template>
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
