
import axios from 'axios';

let apiURL = localStorage.getItem('base') || `${window.location.protocol}//${window.location.hostname}:50803/api`;

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
