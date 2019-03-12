<template>
    <div class="generate-wrapper">
        <form class="form-generate" @submit.prevent="generate">
            <h2 class="form-generate-heading">Generate a New API Key</h2>
            <input v-model="title" id="title" class="form-control" v-if="!key" placeholder="Application Title" required autofocus>
            <input v-model="email" id="email" type="email" class="form-control" v-if="!key" placeholder="Email" required>
            <button class="button-generate" type="submit" v-if="!key">Generate</button>
        </form>
        <div v-if="key" id="hide">
            <h3>Your New API Key:</h3>
        </div>
        <p>{{ key }}</p>
    </div>
</template>

<script>
import axios from 'axios'
import dev_const from '../const.js'

export default {
  data () {
    return {
      key: null,
      title: '',
      email: ''
    }
  },
  methods: {
    generate: function () {
        // TODO: replace with SSO backend url
      const url = `${dev_const.DEV_ROUTE}/applications/generatekey`
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
        .catch(function (error) {
            console.log(error);
        })
    }
  }
}

</script>

<style lang="css">
.generate-wrapper {
    background: #fff;
    width: 70%;
    margin: 1px auto;
    text-align: center;
}

.form-generate {
    max-width: 330px;
    padding: 5% 10px;
    margin: 0 auto;
}

.form-generate .form-control {
    position: relative;
    height: auto;
    -webkit-box-sizing: border-box;
        box-sizing: border-box;
    padding: 10px;
    font-size: 16px;
}

.form-generate .form-control:focus {
    z-index: 2;
}

.form-generate input {
    margin-bottom: 10px;
    width: 100%;
}

.form-generate button {
    width: 100%;
    height: 40px;
}

.form-generate h3 {
    margin-top: 50px;
}

</style>
