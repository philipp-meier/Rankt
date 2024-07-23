import { defineStore } from 'pinia';
import { API_ENDPOINTS } from '@/ApiEndpoints';

export const useAuthStore = () => {
  const innerStore = defineStore({
    id: 'auth',
    state: () => ({
      isAuthenticated: false
    }),
    actions: {
      async login(username: string, password: string) {
        await fetch(API_ENDPOINTS.Login, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ username, password })
        });

        await this.refreshState();
      },
      async logout() {
        await fetch(API_ENDPOINTS.Logout, { method: 'POST' });
        await this.refreshState();
      },
      async refreshState() {
        const resp = await fetch(API_ENDPOINTS.UserInfo, {
          method: 'POST'
        });

        if (!resp.ok) {
          this.isAuthenticated = false;
          return;
        }

        this.isAuthenticated = (await resp.json()).isAuthenticated;
      }
    }
  });
  const store = innerStore();
  if (!store.isAuthenticated) {
    store.refreshState();
  }
  return store;
};
