

/*
 *
 *   ELS-FiveM
 *   Application Javascript File
 *
 */

/*
 *   CONFIGURE UI
 */

window.CONFIG = {};
window.CONFIG.Dev = true;


/*
 *   MODIFICATIONS BELOW THIS LINE ARE NOT SUPPORTED. DO NOT TOUCH!
 */

let elsInterface = new Vue({

    el: '#elsInterface',

    data: {
        CONFIG: window.CONFIG
    },

    methods: {

        getConfig: function (key) {
            return this.CONFIG[key];
        }

    },

    mounted: function () {
        if (this.getConfig(Dev)) {
            console.log("ELS-FiveM NUI Mounted & Ready (Dev Mode)");
        } else {
            console.log("ELS-FiveM NUI Mounted & Ready (Dev Mode)");
        }
    }

});

window.addEventListener('message', (event) => {



});