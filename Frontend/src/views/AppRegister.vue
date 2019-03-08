<template>
    <div class="register-wrapper">
        <form class="form-register" @submit.prevent="register">
            <h2 class="form-register-heading">Register Your Application</h2>
            <input v-model="title" id="title" class="form-control" v-if="!key" placeholder="Title" required autofocus>
            <input v-model="launchUrl" id="launchUrl" class="form-control" v-if="!key" placeholder="Launch Url" required>
            <input v-model="email" id="email" type="email" class="form-control" v-if="!key" placeholder="Email" required>
            <input v-model="deleteUrl" id="deleteUrl" class="form-control" v-if="!key" placeholder="User Deletion Url" required>
            <button class="button-register" type="submit" v-if="!key">Register</button>
        </form>
        <div v-if="key" id="hide">
            <h3>Successful Registration!</h3>
            <h3>Your API Key:</h3>
        </div>
        <p>{{ key }}</p>
    </div>
</template>

<script>
import axios from 'axios'

export default {
  data () {
    return {
      key: null,
      title: '',
      email: '',
      launchUrl: '',
      deleteUrl: ''
    }
  },
  methods: {
    register: function () {
        // TODO: replace with SSO backend url
      const url = 'http://localhost:50803/api/applications/create'
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
            this.key = response.data; // Retrieve api key from response
        })
        .catch(function (error) {
            console.log(error);
        })
    }
  }
}

</script>

<style lang="css">
.register-wrapper {
    background: #fff;
    width: 70%;
    margin: 1px auto;
    text-align: center;
}

.form-register {
    max-width: 330px;
    padding: 5% 10px;
    margin: 0 auto;
}

.form-register .form-control {
    position: relative;
    height: auto;
    -webkit-box-sizing: border-box;
        box-sizing: border-box;
    padding: 10px;
    font-size: 16px;
}

.form-register .form-control:focus {
    z-index: 2;
}

.form-register input {
    margin-bottom: 10px;
    width: 100%;
}

.form-register button {
    width: 100%;
    height: 40px;
}

.form-register h3 {
    margin-top: 50px;
}

</style>
