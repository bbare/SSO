<template>
    <div class="register-wrapper">
        
        <h1>Register your Application</h1>

        <br />
        <v-form>
        <v-text-field
            name="title"
            id="title"
            v-model="title"
            type="title"
            label="Title" 
            v-if="!key"
            /><br />
        <v-text-field
            name="launchUrl"
            id="launchUrl"
            type="launchUrl"
            v-model="launchUrl"
            label="Launch Url" 
            v-if="!key"
            /><br />
        <v-text-field
            name="email"
            id="email"
            type="email"
            v-model="email"
            label="Email" 
            v-if="!key"
            /><br />
        <v-text-field
            name="deleteUrl"
            id="deleteUrl"
            type="deleteUrl"
            v-model="deleteUrl"
            label="User Deletion Url" 
            v-if="!key"
            /><br />

        
        <v-alert
            :value="error"
            type="error"
            transition="scale-transition"
        >
            {{error}}
        </v-alert>

        <div v-if="key">
            <h3>Successful Registration!</h3>
            <br />
            <h3>Your API Key:</h3>
            <p>{{ key }}</p>
        </div>
        <div v-if="secretKey">
            <h3>Your Secret Key</h3>
            <p>{{ secretKey }}</p>
        </div>

        <br />

        <v-btn color="success" v-if="!key" v-on:click="register">Register</v-btn>

        </v-form>
        
    </div>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'

export default {
  data () {
    return {
      key: null,
      secretKey: null,
      title: '',
      email: '',
      launchUrl: '',
      deleteUrl: '',
      error: ''
    }
  },
  methods: {
    register: function () {
      
      this.error = "";
      if (this.title.length == 0 || this.email.length == 0 || this.launchUrl.length == 0 || this.deleteUrl.length == 0) {
        this.error = "Fields Cannot Be Left Blank.";
      }

      if (this.error) return;

      const url = `${apiURL}/applications/create`
      axios.post(url, {
        title: document.getElementById('title').value,
        launchUrl: document.getElementById('launchUrl').value,
        email: document.getElementById('email').value,
        deleteUrl: document.getElementById('deleteUrl').value,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
        .then(response => {
            this.key = response.data.Key; // Retrieve api key from response
            this.secretKey = response.data.SharedSecretKey // Retrieve shared api key from response
        })
        .catch(err => {
            this.error = err.response.data.Message;
        })
    }
  }
}

</script>

<style lang="css">

.register-wrapper {
    width: 70%;
    margin: 1px auto;
}

</style>
