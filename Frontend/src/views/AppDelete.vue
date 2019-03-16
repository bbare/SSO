<template>
    <div class="delete-wrapper">
        <form class="form-delete" @submit.prevent="deleteApp">
            <h2 class="form-delete-heading">Delete Your Application</h2>
            <input v-model="title" id="title" class="form-control" v-if="!validation" placeholder="Application Title" required autofocus>
            <input v-model="email" id="email" type="email" class="form-control" v-if="!validation" placeholder="Email" required>
            <button class="button-delete" type="submit" v-if="!validation">Delete</button>
        </form>
        <div v-if="validation" id="hide">
            <h3>Successful Deletion!</h3>
        </div>
        <p>{{ validation }}</p>
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
      email: ''
    }
  },
  methods: {
    deleteApp: function () {
        // TODO: replace with SSO backend url
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
        .catch(function (error) {
            console.log(error);
        })
    }
  }
}

</script>

<style lang="css">
.delete-wrapper {
    background: #fff;
    width: 70%;
    margin: 1px auto;
    text-align: center;
}

.form-delete {
    max-width: 330px;
    padding: 5% 10px;
    margin: 0 auto;
}

.form-delete .form-control {
    position: relative;
    height: auto;
    -webkit-box-sizing: border-box;
        box-sizing: border-box;
    padding: 10px;
    font-size: 16px;
}

.form-delete .form-control:focus {
    z-index: 2;
}

.form-delete input {
    margin-bottom: 10px;
    width: 100%;
}

.form-delete button {
    width: 100%;
    height: 40px;
}

.form-delete h3 {
    margin-top: 50px;
}

</style>
