<template>
  <div class="reset">
    <h1>Reset Password</h1>
    <br />
    {{message}}
    <br />
    <br />
    <div v-if="haveNetworkError">
      {{errorMessage}}
    <br/>
    </div>
      
    <div class="SecurityQuestions" v-if="securityQuestions.length">
      <br/>
      <div v-for="(securityQuestion, index) in securityQuestions" :key="index">
        {{securityQuestion}}
      </div>
      <br />
      <input name="SecurityAnswer1" type="text" v-model="securityAnswer1" placeholder="Answer for Question 1"/>
      <br />
      <input name="SecurityAnswer2" type="text" v-model="securityAnswer2" placeholder="Answer for Question 2"/>
      <br />
      <input name="SecurityAnswer3" type="text" v-model="securityAnswer3" placeholder="Answer for Question 3"/>
      <br />
      <button type="submit" v-on:click="submitAnswers">Submit Answers</button>
    </div>

    <br/>

    <div id="NewPassword" v-if="showPasswordResetField">
      Enter a new password into the field
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
      resetToken: this.$route.params.id,
      errorMessage: null,
      securityQuestions: {
        securityQuestion1: null,
        securityQuestion2: null,
        securityQuestion3: null
      },
      securityAnswer1: null,
      securityAnswer2: null,
      securityAnswer3: null,
      showPasswordResetField: null,
      newPassword: null,
      newPasswordSuccessful: null,
      networkErrorMessage: null,
      haveNetworkError: false,
      wrongAnswerCounter : 0
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
      .catch(e => { alert(e.response.data) })
    if(this.message === "Reset link is no longer valid"){
      this.$router.push("SendResetLink")
    }
  },
  methods: {
    submitAnswers: function () {
      if(this.wrongAnswerCounter === 3){
        this.errorMessage = "3 attempts have been made, reset link is no longer valid"
        this.$router.push("SendResetLink")
      }
      if (!this.securityAnswer1 || !this.securityAnswer2 || !this.securityAnswer3){
        alert("Security answers cannot be empty")
      } else {
        axios({
        method: 'POST',
        url: 'http://localhost:61348/api/reset/' + this.resetToken + '/checkanswers',
        data: { 
          securityA1: this.$data.securityAnswer1,
          securityA2: this.$data.securityAnswer2,
          securityA3: this.$data.securityAnswer3},
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        }
      })
        .then(response => (this.showPasswordResetField = response.data), this.wrongAnswerCounter = this.wrongAnswerCounter + 1)
        .catch(e => { alert(e.response.data + " Answer(s) incorrect") })
      }
    },
    submitNewPassword: function () {
      if(this.newPassword === null){
        alert("Password cannot be empty")
      } else if (this.newPassword.length < 12){
        alert("Password must be at least 12 characters")
      } else if (this.newPassword.length > 2000) {
        alert("Password must be less than 2000 characters")
      } else {
        axios({
        method: 'POST',
        url: 'http://localhost:61348/api/reset/' + this.resetToken + '/resetpassword',
        data: {newPassword: this.$data.newPassword},
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        }
      })
        .then(response => (alert(response.data)))
        .catch(e => { alert(e.response.data) })
    }
      }
  }
}
</script>

<style lang="css">
.reset{
  padding: 70px 0;
  text-align: center;
}

input[type=text] {
  border: 2px solid rgb(69, 72, 75);
  border-radius: 4px;
  width: 25%
}

</style>
