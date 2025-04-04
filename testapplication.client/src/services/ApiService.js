import axios from 'axios'

const axiosService = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL
});

export const ApiService = {
  getData(searchQuery) {

    const params = new URLSearchParams();

    if (searchQuery)
      params.append('searchQuery', encodeURIComponent(searchQuery))

    return axiosService.get('api/data', { params }).then(response => response.data).catch(error => console.error(error));
  },

  saveData(data) {
    return axiosService.post('api/data', data);
  }
}

export default ApiService;
