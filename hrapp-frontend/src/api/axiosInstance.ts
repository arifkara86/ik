// src/api/axiosInstance.ts
import axios from 'axios';

// Vite'dan environment variable'ı oku
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

if (!API_BASE_URL) {
  console.error("VITE_API_BASE_URL is not defined in .env file");
}

const axiosInstance = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000, // İstek zaman aşımı (10 saniye)
  headers: {
    'Content-Type': 'application/json',
    // İleride Authorization header'ı (JWT token) buraya eklenebilir
    // 'Authorization': `Bearer ${token}`
  }
});

// İsteğe bağlı: Interceptor'lar eklenebilir (örn: her istek öncesi token ekleme, her yanıtta hata yönetimi)
// axiosInstance.interceptors.request.use(...)
// axiosInstance.interceptors.response.use(...)

export default axiosInstance;