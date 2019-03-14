<template>
  <div class="reset">
     <h1>Reset Password</h1>
    <br />
    {{message}}
    <br /><br />
      <p v-if="errors.length">
        <b>Error(s):</b>
          <ul>
            <li v-for="(error, index) in errors" :key="index">
              {{ error }}
            </li>
          </ul>
      </p>
    <div class="SecurityQuestions" v-if="securityQuestions.length">
      <br/>
      <li v-for="(securityQuestion, index) in securityQuestions" :key="index">
        {{securityQuestion}}
      </li>
      <br />
      <input name="SecurityAnswer1" type="text" v-model="securityAnswer1" placeholder="Answer for Question 1"/>
      <br />
      <input name="SecurityAnswer2" type="text" v-model="securityAnswer2" placeholder="Answer for Question 2"/>
      <br />
      <input name="SecurityAnswer3" type="text" v-model="securityAnswer3" placeholder="Answer for Question 3"/>
      <br />
      <button type="submit" v-on:click="submitAnswers">Submit Answers</button>
    </div>
    <div class="NewPassword" v-if="showPasswordResetField">
      {{passwordMessage}}
      <br/>
      <input name="Password" type="text" v-model="newPassword"/>
      <br />

      <button type="submit" v-on:click="submitNewPassword">Submit New Password</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
export default {
  name: 'ResetPassword',
  data () {
    return {
      message: 'Fetching Security Questions',
      resetToken: this.$route.params.id,
      errors: [],
      securityQuestions: {
        securityQuestion1: null,
        securityQuestion2: null,
        securityQuestion3: null
      },
      securityAnswer1: null,
      securityAnswer2: null,
      securityAnswer3: null,
      showPasswordResetField: false,
      newPassword: null,
      newPasswordSuccessful: null,
      passwordMessage: 'Enter a new password in the field'
    }
  },
  created () {
    axios({
      method: 'GET',
      url: 'http://localhost:61348/api/reset/' + this.resetToken,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      }
    })
      .then(response => (this.securityQuestions = response.data),
        this.message = 'Enter your answers for the security questions, fields are case sensitive')
      .catch(e => { this.errors.push(e) })
  },
  methods: {
    submitAnswers: function () {
      axios({
        method: 'POST',
        url: 'http://localhost:61348/api/reset/' + this.resetToken + '/checkanswers',
        data: { securityA1: this.$data.securityAnswer1,
          securityA2: this.$data.securityAnswer2,
          securityA3: this.$data.securityAnswer3},
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        }
      })
        .then(response => (this.showPasswordResetField = response.data))
        .catch(e => { this.errors.push(e) })
    },
    submitNewPassword: function () {
      axios({
        method: 'POST',
        url: 'http://localhost:61348/api/reset/' + this.resetToken + '/resetpassword',
        data: {newPassword: this.$data.newPassword},
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        }
      })
        .then(response => (this.passwordMessage = response.data))
        .catch(e => { this.errors.push(e) })
      if (this.passwordMessage === 'Password has been reset') {
        this.showPasswordResetField = false
      }
    }
  }
}
</script>
