
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
    });
}

const signLaunch = appId => {
  return axios.get(`${apiURL}/launch`, {
    params: {
      token,
      appId
    }
  }).then(response => {
    return response.data;
  });
}

const submitLaunch = launchData => {
  return axios.post(launchData.url, launchData.launchPayload)
    .then(response => {
      return response.data;
    });
}

const store = {
  state: {
    isLogin: false
  },
  isLogin(){
    this.state.isLogin = true;
  },
  isLogout(){
    this.state.isLogin = false;
  }
};

export {
  register,
  signLaunch,
  submitLaunch,
  store
}
