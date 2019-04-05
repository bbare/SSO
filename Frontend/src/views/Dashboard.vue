<template>
  <div id="portal">
    <v-card>
      <h1 id="applicationPortal">Application Portal</h1>
      <v-container fluid grid-list-md>
        <v-layout row wrap>
          <v-flex xs12 sm6 md4 lg3 v-for="(app, index) in applications" :key="index">
            <v-card hover>
              <v-card-title primary-title>
                <!-- If there is no logo, then a default image will be shown -->
                <img
                  v-if="app.LogoUrl === null"
                  src="https://www.freeiconspng.com/uploads/no-image-icon-15.png"
                >
                <img v-else :src="app.LogoUrl">
                <div id="content">
                  <!-- Launching to an app can be done by clicking the app title -->
                  <h3 class="headline mb-0" @click="launchLoading = true; launch(app.Id)">
                    <strong>{{ app.Title }}</strong>
                  </h3>
                </div>
                <!-- Allows expansion or shrinkage of app description -->
                <read-more
                  more-str="read more"
                  :text="app.Description"
                  less-str="read less"
                  :max-chars="150"
                ></read-more>
              </v-card-title>
            </v-card>
            <!-- Loads only if app is in progress of launching -->
            <div v-if="launchLoading">
              <Loading :dialog="launchLoading"/>
            </div>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card>

    <v-alert :value="error" type="error" transition="scale-transition">{{error}}</v-alert>
  </div>
</template>

<script>
import Vue from "vue";
import Loading from "@/components/Dialogs/Loading.vue";
import { signLaunch, submitLaunch } from "@/services/request";
import { apiURL } from "@/const.js";
import axios from "axios";
import ReadMore from "vue-read-more";

Vue.use(ReadMore);

export default {
  components: { Loading },
  data() {
    return {
      applications: [],
      launchLoading: false,
      error: ""
    };
  },
  watch: {
    // Loading animation will need to be modified to finish when the app finishes launching
    launchLoading(val) {
      if (!val) return;
      setTimeout(() => (this.launchLoading = false), 3000);
    }
  },
  methods: {
    launch(appId) {
      this.error = "";

      signLaunch(appId)
        .then(launchData => {
          submitLaunch(launchData)
            .then(launchResponse => {
              window.location.href = launchResponse.redirectURL;
            })
            .catch(err => {
              let code = err.response.status;

              switch (code) {
                case 500:
                  this.error =
                    "An unexpected server error occurred. Please try again momentarily.";
                  break;
                default:
                  this.error =
                    "An unexpected server error occurred. Please try again momentarily.";
                  break;
              }
            });
        })
        .catch(err => {
          let code = err.response.status;

          switch (code) {
            case 500:
              this.error =
                "An unexpected server error occurred. Please try again momentarily.";
              break;
            default:
              this.error =
                "An unexpected server error occurred. Please try again momentarily.";
              break;
          }
        });
    }
  },
  async mounted() {
    await axios
      .get(`${apiURL}/applications`)
      .then(response => (this.applications = response.data));
  }
};
</script>

<style scoped>
.v-card {
  margin: 1em;
}

#applicationPortal {
  padding: 1em;
  font-size: 38px;
  text-decoration: underline;
}

#content {
  margin-left: 1em;
}

img {
  width: 55px;
  height: 55px;
}

.headline:hover {
  text-decoration: underline;
}
</style>
