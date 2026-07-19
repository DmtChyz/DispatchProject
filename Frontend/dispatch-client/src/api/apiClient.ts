import axios from "axios";

export const apiClient = axios.create({
  baseURL: "https://localhost:7047/api",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});