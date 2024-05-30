<script lang="ts" setup>
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import { ref, type Ref } from 'vue';
import { config } from '@/config';
import { useRouter } from 'vue-router';
import { API_ENDPOINTS } from '@/ApiEndpoints';
import { useToast } from 'primevue/usetoast';

const invitationId: Ref<string | null> = ref(null);
const isInvalid = ref(false);
const loading = ref(false);

const router = useRouter();
const toast = useToast();

const joinSurvey = () => {
  if (!invitationId.value) {
    isInvalid.value = true;
    return;
  }

  loading.value = true;

  fetch(`${API_ENDPOINTS.Questions}/${invitationId.value}`, { method: 'HEAD' })
    .then((resp) => {
      if (resp.ok) {
        isInvalid.value = false;
        router.push(`/question?id=${invitationId.value}`);
      } else {
        toast.add({
          severity: 'error',
          summary: 'Error',
          detail: `Invalid invitation id (${invitationId.value}).`,
          life: 3000
        });
        isInvalid.value = true;
      }

      loading.value = false;
    })
    .catch(() => {
      isInvalid.value = true;
      loading.value = false;
    });
};
</script>

<template>
  <div class="welcome">
    <div class="content">
      <h1>{{ config.AppTitle }}</h1>
      <h4>Simple ranking surveys for everyone</h4>
    </div>
    <div class="join-container">
      <div>
        <InputText
          id="invitation-id"
          v-model="invitationId"
          :invalid="isInvalid"
          autofocus
          placeholder="Invitation ID"
          @keyup.enter="joinSurvey"
        />
        <Button
          :disabled="!invitationId"
          :loading="loading"
          label="Join survey"
          @click="joinSurvey"
        />
      </div>
    </div>
  </div>
</template>

<style scoped>
div.welcome {
  margin-top: 0.5em;
  display: flex;

  justify-content: center;
  flex-direction: column;

  div.content {
    margin-bottom: 2.5em;
  }

  div.join-container > div {
    display: flex;
    flex-direction: row;

    input {
      margin-right: 0.25em;
    }
  }

  div {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
}
</style>
