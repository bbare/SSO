<template>
  <div id="portal">
    <!-- Feel free to hardcode an app ID here until dashboard logic is complete -->
    <!-- <button v-on:click="launch('D146A971-593A-47E4-ABCB-8B3AAF686ECD')">Fake App (hardcoded ID)</button> -->

    <v-card>
      <h1 id="applicationPortal">Application Portal</h1>
      <v-container fluid grid-list-md>
        <v-layout row wrap>
          <v-flex xs12 sm6 md4 lg3 v-for="(app, index) in applications" :key="index">
            <v-card hover @mouseover="app.showInfo = true" @mouseleave="app.showInfo = false">
              <v-icon
                v-if="app.showInfo"
                large
                color="orange"
                style="float: right"
                @click="showInfo = true"
              >info</v-icon>

              <v-card-title primary-title @click="launchLoading = true; launch(app.Id)">
                <img src="https://www.freeiconspng.com/uploads/no-image-icon-15.png">
                <div id="content">
                  <h3 class="headline mb-0">
                    <strong>{{ app.Title }}</strong>
                  </h3>
                </div>
              </v-card-title>
            </v-card>
            <div v-if="launchLoading">
              <Loading :dialog="launchLoading"/>
            </div>
            <div v-if="showInfo">
              <AppInfo :dialog="showInfo" :appDetails="app" @exitAppInfo="exitAppInfo"/>
            </div>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card>

    <v-alert :value="error" type="error" transition="scale-transition">{{error}}</v-alert>
  </div>
</template>

<script>
import Loading from "@/components/Dialogs/Loading.vue";
import AppInfo from "@/components/Dialogs/AppInfo.vue";
import { signLaunch, submitLaunch } from "@/services/request";
import { apiURL } from "@/const.js";
import axios from "axios";

export default {
  components: { Loading, AppInfo },
  data() {
    return {
      applications: [],
      showInfo: false,
      launchLoading: false,
      error: ""
    };
  },
  watch: {
    launchLoading(val) {
      if (!val) return;
      setTimeout(() => (this.launchLoading = false), 3000);
    }
  },
  methods: {
    exitAppInfo(hideInfo) {
      this.showInfo = hideInfo;
    },
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
      .get(`${apiURL}/api/applications`)
      .then(response => (this.applications = response.data));

    // Add attribute for displaying info icon
    // Add attribute for editing app description
    for (var i = 0; i < this.applications.length; i++) {
      this.$set(this.applications[i], "showInfo", false);
      this.$set(this.applications[i], "editDescription", false);
    }
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
</style>
