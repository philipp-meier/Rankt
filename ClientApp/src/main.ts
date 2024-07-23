import './main.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';

import App from './App.vue';
import router from './Shared/Router';
import PrimeVue from 'primevue/config';
import Aura from 'primevue/themes/aura';
import ToastService from 'primevue/toastservice';

import 'primeicons/primeicons.css';

const app = createApp(App);

app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      prefix: 'p',
      darkModeSelector: 'system',
      cssLayer: false
    }
  }
});
app.use(ToastService);
app.use(createPinia());
app.use(router);

app.mount('#app');
