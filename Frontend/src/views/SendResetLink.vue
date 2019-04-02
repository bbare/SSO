<template>
  <div class="sendLink">
    <h1>Reset Password</h1>
    <br />
        <v-form>
        <v-text-field
            name="email"
            id="email"
            v-model="email"
            type="email"
            label="Email" 
            /><br 
        />
        <v-alert
          :value="errorMessage"
          dismissible=""
          type="error"
          transition="scale-transition"
        >
        {{errorMessage}}
        </v-alert>
        </v-form>

      <v-alert
        :value="message"
        dismissible
        type="success"
        transition="scale-transition"
      >
      {{message}}
      </v-alert>

<v-btn color="success" v-on:click="submitEmail">Send Email</v-btn>
  </div>
</template>

<script>
import axios from 'axios';
import { apiURL } from '@/const.js';

export default {
  name: 'SendResetLink',
  data () {
    return {
      errorMessage: "",
      message: "",
      email: ""
    }
  },
  methods: {
    submitEmail: function () {
      if (!this.email) {
        this.errorMessage = 'Email field cannot be empty'
      } else if (!this.validEmail(this.email)) {
        this.errorMessage = 'Valid email required'
      } else {
        axios({
          method: 'POST',
          url: `${apiURL}/reset/send`,
          data: {email: this.$data.email, url: 'https://kfc-sso.com/#/resetpassword/'},
          headers: {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Credentials': true
          }
        })
          .then(response => {this.message = response.data}, this.errorMessage = '', 
          setTimeout(() => this.redirectToLogin(), 3000))
          .catch(e => { this.errorMessage = e.responsed.data })
      }
    },
    validEmail: function (email) {
      var re = /^(([^<>()\\\\.,;:\s@"]+(\.[^<>()\\\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      return re.test(email)
    }

  }
}
</script>

<style>
.sendLink{
  width: 70%;
  margin: 1px auto;
}
</style>
