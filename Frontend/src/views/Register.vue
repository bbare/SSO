<template>
  <div class="register-wrapper">
    <h1>Register</h1>

    <br />
    <v-form>
      Credentials:<br />
      <v-text-field
        name="email"
        id="email"
        v-model="email"
        type="email"
        label="Email" /><br />
      <v-text-field
        name="password"
        id="password"
        type="password"
        v-model="password"
        label="Password" /><br />
      <v-text-field
        name="confirm"
        id="confirm"
        type="password"
        v-model="confirmPassword"
        label="Confirm Password" /><br />

      <br /><br />
      Personal Details:<br />
      <v-menu
        ref="menu"
        v-model="menu"
        :close-on-content-click="false"
        :nudge-right="40"
        lazy
        transition="scale-transition"
        offset-y
        full-width
        min-width="290px"
      >
        <template v-slot:activator="{ on }">
          <v-text-field
            v-model="dob"
            label="Date of Birth"
            prepend-icon="event"
            readonly
            v-on="on"
          ></v-text-field>
        </template>
        <v-date-picker
          ref="picker"
          v-model="dob"
          :max="new Date().toISOString().substr(0, 10)"
          min="1950-01-01"
          @change="updateDate"
        ></v-date-picker>
      </v-menu>

      <v-text-field
        name="city"
        id="city"
        v-model="city"
        label="City" /><br />
      <v-text-field
        name="state"
        id="state"
        v-model="state"
        label="State" /><br />
      <v-text-field
        name="country"
        id="country"
        v-model="country"
        label="Country" /><br />

      <br /><br />
      Security Questions:<br />
      <v-text-field
        name="securityq1"
        id="securityq1"
        v-model="securityQ1"
        label="Security Question 1" /><br />
      <v-text-field
        name="securitya1"
        id="securitya1"
        v-model="securityQ1Answer"
        label="Security Answer 1" /><br />

      <br />

      <v-text-field
        name="securityq2"
        id="securityq2"
        v-model="securityQ2"
        label="Security Question 2" /><br />
      <v-text-field
        name="securitya2"
        id="securitya2"
        v-model="securityQ2Answer"
        label="Security Answer 2" /><br />

      <br />

      <v-text-field
        name="securityq3"
        id="securityq3"
        v-model="securityQ3"
        label="Security Question 3" /><br />
      <v-text-field
        name="securitya3"
        id="securitya3"
        v-model="securityQ3Answer"
        label="Security Answer 3" /><br />

      <v-alert
        :value="error"
        type="error"
        transition="scale-transition"
      >
        {{error}}
      </v-alert>

      <br />

      <v-btn color="success" v-on:click="submit">Register</v-btn>

    </v-form>
  </div>
</template>

<script>
import { register } from '@/services/request';

export default {
  name: 'Register',
  data: () => {
    return {
      menu: false,
      error: "",

      email: '',
      password: '',
      confirmPassword: '',

      dob: '',
      city: '',
      state: '',
      country: '',

      securityQ1: '',
      securityQ1Answer: '',
      securityQ2: '',
      securityQ2Answer: '',
      securityQ3: '',
      securityQ3Answer: '',
    }
  },
  watch: {
    menu (val) {
      val && setTimeout(() => (this.$refs.picker.activePicker = 'YEAR'))
    }
  },
  methods: {
    updateDate(date) {
      this.$refs.menu.save(date)
    },
    submit: function() {
      this.error = "";
      if (this.password.length == 0) {
        this.error = "Password is required";
      } else if (this.password !== this.confirmPassword) {
        this.error = "Password entered does not match password confirmation";
      }

      if (this.error) return;

      register({
        email: this.email,
        password: this.password,
        confirmPassword: this.confirmPassword,

        dob: this.dob,
        city: this.city,
        state: this.state,
        country: this.country,

        securityQ1: this.securityQ1,
        securityQ1Answer: this.securityQ1Answer,
        securityQ2: this.securityQ2,
        securityQ2Answer: this.securityQ2Answer,
        securityQ3: this.securityQ3,
        securityQ3Answer: this.securityQ3Answer  
      }).then(() => {
        const params = new URLSearchParams(window.location.search)

        if (params.has('redirect')) {
          window.location.href = decodeURIComponent(params.get('redirect'));
        } else {
          this.$router.push('dashboard');
        }
      }).catch(err => {
        switch(err.response.status) {
          case 401:
            this.error = err.response.data;
            break;
          case 406:
            this.error = "Your email address does not follow a recognized format.";
            break;
          case 409:
            this.error = "This email address already belongs to an account.";
            break;
          case 412:
            this.error = "Please fill out all of the registration fields and try again.";
            break;
          case 500:
            this.error = "An unexpected server error occurred. Please try again momentarily.";
        }
      })
    }
  }
}
</script>


<style scoped>
.register-wrapper {
    width: 70%;
    margin: 1px auto;
}
</style>
