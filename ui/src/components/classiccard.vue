<template>
  <v-card>
    <v-card-title primary-title>
          <div>
            <div class="headline">{{ title }}</div>
          </div>
        </v-card-title>
        <v-card-actions>
          <v-btn flat></v-btn>
        </v-card-actions>
  </v-card>
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

// The @Component decorator indicates the class is a Vue component
@Component({
  name: "classiccard",
  computed:{},
  props: ['title']
})
export default class ClassicCard extends Vue {
    constructor() {
    super();
  }

  

  
}
</script>
