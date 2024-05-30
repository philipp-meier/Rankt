<script lang="ts" setup>
import InputText from 'primevue/inputtext';
import Password from 'primevue/password';
import Button from 'primevue/button';
import { type Ref, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';

const username: Ref<string | null> = ref(null);
const password: Ref<string | null> = ref(null);
const isInvalid = ref(false);
const loading = ref(false);

const router = useRouter();
const authStore = useAuthStore();

const submit = () => {
  loading.value = true;
  setTimeout(() => {
    loading.value = false;

    authStore.login(username.value!, password.value!).then(() => {
      if (authStore.isAuthenticated) router.push('/admin');
      else isInvalid.value = true;
    });
  }, 2000);
};
</script>

<template>
  <div class="card login-container">
    <h2>Login</h2>
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
      <Button :loading="loading" label="Submit" @click="submit" />
    </div>
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
