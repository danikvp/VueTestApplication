<template>
  <Button label="Create" severity="info" @click="createButtonClick" />

  <Dialog v-model:visible="openDialog" modal header="Create New Item" :style="{ width: '40rem' }">

    <Form v-slot="$form" :initialValues :resolver @submit="onFormSubmit" class="flex flex-col gap-4 w-full pt-1">

      <div class="flex flex-col gap-1">
        <FloatLabel variant="on">
          <InputText id="username" name="username" type="text" fluid />
          <label for="username">User Name</label>
        </FloatLabel>
        <Message v-if="$form.username?.invalid" severity="error" size="small" variant="simple">{{ $form.username.error?.message }}</Message>
      </div>

      <div class="flex flex-col gap-1">
        <FloatLabel variant="on">
          <Select id="city" name="city" :options="cities" fluid />
          <label for="city">City</label>
        </FloatLabel>
        <Message v-if="$form.city?.invalid" severity="error" size="small" variant="simple">{{ $form.city.error?.message }}</Message>
      </div>

      <div class="flex flex-col gap-1">
        <FloatLabel variant="on">
          <DatePicker id="date" name="date" fluid />
          <label for="date">Date</label>
        </FloatLabel>
        <Message v-if="$form.date?.invalid" severity="error" size="small" variant="simple">{{ $form.date.error?.message }}</Message>
      </div>

      <div class="flex flex-col gap-2">
        <RadioButtonGroup name="category" class="flex flex-wrap gap-4">
          <div v-for="cat in categories" :key="cat.key" class="flex items-center gap-2">
            <RadioButton :inputId="cat.key" name="dynamic" :value="cat.name" />
            <label :for="cat.key">{{ cat.name }}</label>
          </div>
        </RadioButtonGroup>
        <Message v-if="$form.category?.invalid" severity="error" size="small" variant="simple">{{ $form.category.error?.message }}</Message>
      </div>

      <div class="flex flex-col gap-2">
        <div class="flex items-center gap-2">
          <Checkbox name="isActive" inputId="isActive" binary />
          <label for="isActive"> Is Active </label>
        </div>
      </div>

      <Button type="submit" severity="secondary" label="Submit" />
    </Form>
  </Dialog>


</template>

<script setup>
  import { ref, reactive } from 'vue';
  import ApiService from '@/services/ApiService';

  const emit = defineEmits(['dataSubmitted'])

  const openDialog = ref(false);

  const initialValues = reactive({
    username: '',
    city: '',
    date: '',
    category: '',
    isActive: true,
  });


  const cities = ref([
    'New York',
    'Rome',
    'London',
    'Istanbul',
    'Paris'
  ]);

  const categories = ref([
    { name: 'Accounting', key: 'A' },
    { name: 'Marketing', key: 'M' },
    { name: 'Production', key: 'P' },
    { name: 'Research', key: 'R' }
  ]);

  const isActive = ref(false);


  const createButtonClick = () => {
    openDialog.value = true;
  }

  const resolver = ({ values }) => {
    const errors = {};


    if (!values.username) {
      errors.username = [{ message: 'User Name is required.' }];
    }

    if (!values.city) {
      errors.city = [{ message: 'City is required.' }];
    }

    if (!values.date) {
      errors.date = [{ message: 'Date is required.' }];
    }
    else {
      if (isNaN(new Date(values.date))) {
        errors.date = [{ message: 'Please enter valid date.' }];
      }
    }

    if (!values.category) {
      errors.category = [{ message: 'Category is required.' }];
    }


    return {
      values, // (Optional) Used to pass current form values to submit event.
      errors
    };
  };

  const onFormSubmit = async (event) => {
    if (event.valid) {

      await ApiService.saveData(event.values);
      openDialog.value = false;
      emit('dataSubmitted');

    }
  };



</script>

<style>
</style>
