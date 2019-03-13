<template>
  <div class="update">
     <h1>Update Password</h1>
    <br />
    {{message}}
    <br /><br />
      <p v-if="errors.length">
        <b>Please correct the following error(s):</b>
          <ul>
            <li v-for="(error, index) in errors" :key="index">
              {{ error }}
            </li>
          </ul>
      </p>
    <div class="submitPasswords">
        Old Password
        <input name="oldPassword" type="text" v-model="oldPassword"/>
        <br/>
        New Password
        <input name="newPassword" type="text" v-model="newPassword" />
        <br/>
        Confirm New Password
        <input name="confirmNewPassword" type="text" v-model="confirmNewPassword"/>
        <br />
        <br/>
        <button type="submit" v-on:click="submitPasswords">Update Password</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
export default {
  name: 'UpdatePassword',
  data () {
    return {
      message: 'Enter the new password:',
      errors: [],
      oldPassword: null,
      newPassword: null,
      confirmNewPassword: null
    }
  },
  methods: {
    submitNewPasswords: function (e) {
      if (this.newPassword.length < 12 || this.oldPassword.length < 12) {
        this.errors.push('Password does not meet minimum length of 12')
      } else if (this.newPassword.length > 2000 || this.oldPassword.length > 2000) {
        this.errors.push('Password exceeds maximum length of 2000')
      } else if (this.confirmNewPassword !== this.newPassword) {
        this.errors.push('Passwords do not match')
      } else {
        this.errors = []
        this.message = 'Updating Password'
        axios({
          method: 'POST',
          url: 'api.kfcsso.com/api/user/updatpPassword',
          data: {
            oldPassword: this.$data.oldPassword,
            newPassword: this.$data.confirmNewPassword,
            confirmNewPassword: this.$data.confirmNewPassword
          },
          headers: {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Credentials': true
          }
        })
      }
    }
  }
}
