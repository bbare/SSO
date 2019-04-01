
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
    isLogin: false,
    email: ""
  },
  isUserLogin(){
    if(token !== null){
      this.state.isLogin = true;
    }
    else{
      this.state.isLogin= false;
    }
  },
  getEmail(){
      axios.get(`${apiURL}/users/${localStorage.token}`,
      {
        params: {
          token: this.token
        }
      })
      .then(resp =>{
        this.state.email = resp.data
      })  
  }
};


export {
  register,
  signLaunch,
  submitLaunch,
  store
}
