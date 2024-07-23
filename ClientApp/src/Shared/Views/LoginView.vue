<script lang="ts" setup>
import InputText from 'primevue/inputtext';
import Password from 'primevue/password';
import Button from 'primevue/button';
import { type Ref, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/Shared/Stores/authStore';
import { useToast } from 'primevue/usetoast';

const username: Ref<string | null> = ref(null);
const password: Ref<string | null> = ref(null);
const isInvalid = ref(false);
const loading = ref(false);

const router = useRouter();
const authStore = useAuthStore();
const toast = useToast();

const submit = () => {
  loading.value = true;
  setTimeout(() => {
    loading.value = false;

    authStore.login(username.value!, password.value!).then(() => {
      if (authStore.isAuthenticated) {
        router.push('/admin');
      } else {
        isInvalid.value = true;
        toast.add({ severity: 'error', summary: 'Error', detail: 'Login failed.', life: 1000 });
      }
    });
  }, 2000);
};
</script>

<template>
  <div class="card login-container">
    <h2>Login</h2>
    <form @submit.prevent="submit">
      <div>
        <InputText
          id="username"
          v-model="username"
          :invalid="isInvalid"
          placeholder="Username"
          @keyup.enter="submit"
        />
      </div>
      <div>
        <Password
          id="password"
          v-model="password"
          :feedback="false"
          :invalid="isInvalid"
          placeholder="Password"
          @keyup.enter="submit"
        />
      </div>
      <div>
        <Button :loading="loading" label="Submit" type="submit" />
      </div>
    </form>
  </div>
</template>

<style scoped>
div.login-container {
  margin: 0.5em 1em 0 1em;

  div {
    margin-bottom: 0.25em;
  }
}
</style>
