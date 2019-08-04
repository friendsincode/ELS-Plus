<template>
  <v-app dark v-if="$store.state.isVisible" id="app">
    <v-content>
      <router-view />
    </v-content>
  </v-app>
</template>

<style>
html, body {
  overflow: hidden;
}
.theme--light.application {
  background-color: transparent;
}
.theme--dark.application {
  background-color: transparent;
}

</style>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import router from './router';

@Component({
  components: {
  }
})
export default class App extends Vue {
    constructor() {
      super()
      if (localStorage.getItem("elsplus_ui_page")) {
        this.$store.commit("CHANGE_PAGE", localStorage.getItem("elsplus_ui_page"))
      }
      if (localStorage.getItem("elsplus_ui_loc")) {
        this.$store.commit("SET_LOCATION", localStorage.getItem("elsplus_ui_loc"))
      }
      if (localStorage.getItem("elsplus_ui_alpha")) {
        this.$store.commit("SET_PANEL_ALPHA", localStorage.getItem("elsplus_ui_alpha"))
      }
    }
}
</script>