<template>
  <div>
    <v-btn block dark :color="button_color" :depressed="button_status">
      {{ title }}
    </v-btn>
    <div class="caption text-xs-center">{{ ui_data }}</div>
  </div>

  <!--<v-card class="text-xs-center px-0" :color="button_color">
    <v-card-text>
      
      <v-divider dark></v-divider>
        <span class="body-2 text-xs-center">{{ title }}</span>
        <span class="caption text-xs-center">{{ ui_data }}</span>
        <v-chip :color="button_color">
        <v-icon medium>{{ button_status }}</v-icon>
      </v-chip>

      
    </v-card-text>
  </v-card>-->
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

// The @Component decorator indicates the class is a Vue component
@Component({
  name: "elsuicard",
  computed: {},
  props: ["title", "button_color", "button_status", "ui_data"]
})
export default class ElsUiCard extends Vue {
  constructor() {
    super();
  }
}
</script>

