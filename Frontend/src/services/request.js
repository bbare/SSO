
import axios from 'axios';
import { apiURL } from "@/const.js";

/* eslint-disable */
let token = localStorage.getItem('token');

const register = registrationData => {
  return axios.post(`${apiURL}/users/register`, registrationData)
    .then(response => {
      token = response.data.token;
      localStorage.setItem('token', token);
      return response;
    })
}

export { register }
