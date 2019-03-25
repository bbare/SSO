<template>
    <div class="delete-wrapper">

        <h1>Delete Your Application</h1>

        <br />
        <v-form>
        <v-text-field
            name="title"
            id="title"
            v-model="title"
            type="title"
            label="Application Title" 
            v-if="!validation"
            /><br />
        <v-text-field
            name="email"
            id="email"
            type="email"
            v-model="email"
            label="Email" 
            v-if="!validation"
            /><br />

        
        <v-alert
            :value="error"
            type="error"
            transition="scale-transition"
        >
            {{error}}
        </v-alert>

        <div v-if="validation" id="hide">
            <h3>Successful Deletion!</h3>
        </div>
        <p>{{ validation }}</p>

        <br />

        <v-btn color="success" v-if="!validation" v-on:click="deleteApp">Delete</v-btn>

        </v-form>

    </div>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'

export default {
  data () {
    return {
      validation: null,
      title: '',
      email: '',
      error: ''
    }
  },
  methods: {
    deleteApp: function () {
      
      this.error = "";
      if (this.title.length == 0 || this.email.length == 0) {
        this.error = "Fields Cannot Be Left Blank.";
      }

      if (this.error) return;

      const url = `${apiURL}/applications/delete`
      axios.post(url, {
        title: document.getElementById('title').value,
        email: document.getElementById('email').value,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
        .then(response => {
            this.validation = response.data; // Retrieve deletion validation
        })
        .catch(err => {
            this.error = err.response.data
        })
    }
  }
}

</script>

<style lang="css">
.delete-wrapper {
    width: 70%;
    margin: 1px auto;
}

</style>
