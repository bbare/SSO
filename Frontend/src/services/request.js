
import axios from 'axios';
import dev_const from "../const.js";


let apiURL = localStorage.getItem('base') || `${window.location.protocol}//${window.location.hostname}:${dev_const.DEV_PORT}/api`;

/* eslint-disable */
let token = localStorage.getItem('token');

const register = registrationData => {
  return axios.post(`${apiURL}/users/register`, registrationData)
    .then(response => {
      token = response.data.token;
      return response;
    })
}

export { register }
