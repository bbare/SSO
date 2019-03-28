<template>
    <div class="generate-wrapper">

        <h1 class="header">Generate a New API Key</h1>

        <br />
        <v-form>
        <v-text-field
            name="title"
            id="title"
            v-model="title"
            type="title"
            label="Application Title" 
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

        
        <v-alert
            :value="error"
            id="error"
            type="error"
            transition="scale-transition"
        >
            {{error}}
        </v-alert>

        <div v-if="key" id="keyMessage">
            <h3>Your New API Key:</h3>
            <p>{{ key }}</p>
        </div>

        <br />

        <v-btn id="btnGenerate" color="success" v-if="!key" v-on:click="generate">Generate</v-btn>

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
      title: '',
      email: '',
      error: ''
    }
  },
  methods: {
    generate: function () {
      
      this.error = "";
      if (this.title.length == 0 || this.email.length == 0) {
        this.error = "Fields Cannot Be Left Blank.";
      }

      if (this.error) return;

      const url = `${apiURL}/applications/generatekey`
      axios.post(url, {
        title: document.getElementById('title').value,
        email: document.getElementById('email').value,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
        .then(response => {
            this.key = response.data; // Retrieve api key from response
        })
        .catch(err => {
            this.error = err.response.data
        })
    }
  }
}

</script>

<style lang="css">
.generate-wrapper {
    width: 70%;
    margin: 1px auto;
}

</style>
