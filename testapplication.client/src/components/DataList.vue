<template>

  <div class="card">
    <DataTable :value="dataItems" tableStyle="min-width: 50rem" :loading="loading">
      <template #header>
        <div class="flex justify-between">
          <create-data-dialog @data-submitted="refreshData" />
          <IconField>
            <InputIcon>
              <i class="pi pi-search" />
            </InputIcon>
            <InputText v-model="searchQuery" placeholder="Keyword Search" @input="onFilter" />
          </IconField>
        </div>
      </template>
      <template #empty>
        No data found.
      </template>
      <template #loading>
        Loading... Please refresh once the ASP.NET backend has started.
      </template>

      <!--<Column v-for="col of columns" :key="col.field" :field="col.field" :header="col.header"></Column>-->

      <Column v-for="(key, value) in tableColumns" :key="key" :field="key" :header="key">
        <template #body="slotProps">
          <span v-if="typeof slotProps.data[key] === 'boolean'">{{ slotProps.data[key] ? 'Yes' : 'No' }}</span>
          <span v-else>{{ formatDate(slotProps.data[key]) }}</span>
        </template>
      </Column>

    </DataTable>
  </div>

</template>

<script setup>
  import { ref, onMounted } from 'vue'
  import _ from 'lodash';
  import ApiService from '@/services/ApiService';

  const searchQuery = ref(null);

  const dataItems = ref(null);
  const loading = ref(false);

  const tableColumns = ref([]);



  const onFilter = _.debounce(async () => {
    refreshData();
  }, 500);


  onMounted(() => {
    refreshData();
  })

  const refreshData = async () => {
    dataItems.value = null;
    loading.value = true;

    const data = await ApiService.getData(searchQuery.value);
    dataItems.value = data;
    loading.value = false;

    tableColumns.value = dataItemsColumns(dataItems.value);
  };

  const dataItemsColumns = (dataItems) => {
    const columns = new Set();
    if (dataItems) {
      dataItems.forEach(dataItem => {
        Object.keys(dataItem).forEach(key => {
          columns.add(key);
        });
      });
    }
    return Array.from(columns); // Convert Set to array for Vue iteration
  }

  const isDate = (value) => {
    return !isNaN(Date.parse(value));
  }

  const formatDate = (value) => {
    if (!value || !isDate(value) || typeof value !== 'string') return value;
    return new Date(Date.parse(value)).toLocaleDateString();
  }



</script>

<style>
</style>
