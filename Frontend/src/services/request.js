
import axios from 'axios';
import config from "../const.js";


let apiURL = localStorage.getItem('base') || config.API_ROUTE;

/* eslint-disable */
let token = localStorage.getItem('token');

const register = registrationData => {
  return axios.post(`${apiURL}/users/register`, registrationData)
    .then(response => {
      token = response.data.data.token;
      localStorage.setItem('token', token);
      return response;
    })
}

export { register }
