<script lang="ts" setup>
import { onMounted } from 'vue';
import { useAuthStore } from '@/Shared/Stores/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

onMounted(async () => {
  await authStore.logout();
  await router.push({ name: 'home' });
});
</script>

<template>
  <br />
</template>

<style scoped></style>
