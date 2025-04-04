import { mount } from '@vue/test-utils';
import { describe, it, expect } from 'vitest';
import PrimeVue from 'primevue/config';
import CreateDataDialog from '@/components/CreateDataDialog.vue';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';

describe('CreateDataDialog.vue', () => {
  it('should render the CreateDataDialog with the PrimeVue Button', async () => {
    const wrapper = mount(CreateDataDialog, {
      global: {
        plugins: [PrimeVue], // Register PrimeVue plugin globally
      },
    });

    expect(wrapper.exists()).toBe(true);
    expect(wrapper.findComponent(Button).exists()).toBe(true);
    expect(wrapper.text()).toContain('Create');

  });

  it('should render the CreateDataDialog with the PrimeVue Button', async () => {
    const wrapper = mount(CreateDataDialog, {
      global: {
        plugins: [PrimeVue], // Register PrimeVue plugin globally
        components: {
          Button,  // Explicitly register Button component
          Dialog,  // Explicitly register Dialog component
        }
      },
    });

    const button = wrapper.findComponent(Button);
    expect(button.exists()).toBe(true);
    await button.trigger('click');

    const dialog = wrapper.findComponent(Dialog);

    expect(dialog.exists()).toBe(true); // Check if the Dialog exists in the DOM
    expect(dialog.props('visible')).toBe(true); 
  });


});
