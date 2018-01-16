function controlLights(state, light, color) {
    if (!state) {
        $(light).removeClass("red").removeClass("blue").addClass("off");
    } else {
        $(light).removeClass("off").addClass(color);
    }
}
let resourceName = "";

$(function () {
    window.addEventListener('message', function (event) {
        if (event.data.type == "initdata") {
            resouceName = event.data.resourceName;
        }
        else if (event.data.type == "enableui") {
            //console.log("we are going to " + event.data.enable + " UI");
            //cursor.style.display = event.data.enable ? "block" : "none";
            document.body.style.display = event.data.enable ? "block" : "none";
        } else if (event.data.type == "click") {
            // Avoid clicking the cursor itself, click 1px to the top/left;
            Click(cursorX - 1, cursorY - 1);
        } else if (event.data.type == "lightControl") {
            //console.log("Got Light Data " + event.data);
            controlLights(event.data.state, event.data.light, event.data.color);
        } else if (event.data.type == "setpatternnumber") {
            switch (event.data.lighttype) {
                case "PRML":
                    $("#togPri").bootstrapSwitch("labelWidth", 40).bootstrapSwitch("labelText", event.data.pattern);
                    break;
                case "SECL":
                    $("#togSec").bootstrapSwitch("labelWidth", 40).bootstrapSwitch("labelText", event.data.pattern);
                    break;
                case "WRNL":
                    $("#togWrn").bootstrapSwitch("labelWidth", 40).bootstrapSwitch("labelText", event.data.pattern);
                    break;
            }
        } else if (event.data.type == "togglestate") {
            switch (event.data.which) {
                case "PRML":
                    $("#togPri").prop("checked", event.data.state);
                    break;
                case "SECL":
                    $("#togSec").prop("checked", event.data.state);
                    break;
                case "WRNL":
                    $("#togWrn").prop("checked", event.data.state);
                    break;
            }
        }
    });

    window.addEventListener('keyup', function (e) {
        console.log(event)

        if (event.which == 74) {
            $.post(`http://${resourceName}/keyPress`, JSON.stringify({}));
        }
    });

    document.onkeyup = function (data) {
        if (data.which == 27) { // Escape key
            $.post(`http://${resourceName}/escape`, JSON.stringify({}));
            //console.log("Escape pressed");
        }
    };

});


$(document).ready(function () {
    $("#togPri").bootstrapSwitch({
        size: "mini",
        onColor: "danger",
        offColor: "success",
        labelWidth: 40
    });
    $("#togPri").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            //$("#togPri").bootstrapSwitch("labelText", "0000");
            $.post(`http://${resourceName}/togglePrimary`, JSON.stringify({ state: state }));
        } else {
            //$("#togPri").bootstrapSwitch("labelText", "0001");
            $.post(`http://${resourceName}/togglePrimary`, JSON.stringify({ state: state }));
        }
    });
    $("#togSec").bootstrapSwitch({
        size: "mini",
        onColor: "danger",
        offColor: "success",
        labelWidth: 40
    });
    $("#togSec").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            $("#togSec").bootstrapSwitch("labelText", "0000");
            $.post(`http://${resourceName}/toggleSecondary`, JSON.stringify({ state: state }));
        } else {
            $("#togSec").bootstrapSwitch("labelText", "0001");
            $.post(`http://${resourceName}/toggleSecondary`, JSON.stringify({ state: state }));
        }
    });
    $("#togWrn").bootstrapSwitch({
        size: "mini",
        onColor: "danger",
        offColor: "success",
        labelWidth: 40
    });
    $("#togWrn").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            $("#togWrn").bootstrapSwitch("labelText", "0000");
            $.post(`http://${resourceName}/toggleWarn`, JSON.stringify({ state: state }));
        } else {
            $("#togWrn").bootstrapSwitch("labelText", "0001");
            $.post(`http://${resourceName}/toggleWarn`, JSON.stringify({ state: state }));
        }
    });
    $("#togHrn").bootstrapSwitch({
        size: "mini",
        onColor: "danger",
        offColor: "success",
        labelWidth: 40
    });
    $("#togHrn").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            $("#togHrn").bootstrapSwitch("labelText", "0000");
        } else {
            $("#togHrn").bootstrapSwitch("labelText", "0001");
        }
    });
    $("#togSrn").bootstrapSwitch({
        size: "mini",
        onColor: "danger",
        offColor: "success",
        labelWidth: 40
    });
    $("#togSrn").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            $("#togSrn").bootstrapSwitch("labelText", "SCAN");
        } else {
            $("#togSrn").bootstrapSwitch("labelText", "SCAN");
        }
    });
    $("#togSrm").bootstrapSwitch({
        size: "mini",
        onColor: "danger",
        offColor: "success",
        labelWidth: 40
    });
    $("#togSrm").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            $("#togSrm").bootstrapSwitch("labelText", "0000");
        } else {
            $("#togSrm").bootstrapSwitch("labelText", "0001");
        }
    });

    $("#togTkdn").bootstrapSwitch({
        size: "mini",
        onColor: "info",
        offColor: "primary",
        labelText: "TKDN"
    });
    $("#togTkdn").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            $("#tk1").removeClass("off").addClass("white");
            $("#tk2").removeClass("off").addClass("white");
        } else {
            $("#tk1").removeClass("white").addClass("off");
            $("#tk2").removeClass("white").addClass("off");
        }
    });

    $("#togCrs").bootstrapSwitch({
        size: "mini",
        onColor: "info",
        offColor: "primary",
        labelText: "CRS"
    });
    $("#togCrs").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            $("#frt1").removeClass("red").removeClass("blue").addClass("off");
            $("#frt2").removeClass("red").removeClass("blue").addClass("off");
            $("#lb3").removeClass("red").removeClass("blue").addClass("off");
            $("#lb1").removeClass("red").removeClass("blue").removeClass("off").addClass("red");
            $("#lb2").removeClass("red").removeClass("blue").addClass("off");
            $("#lb3").removeClass("red").removeClass("blue").addClass("off");
            $("#lb4").removeClass("red").removeClass("blue").removeClass("off").addClass("blue");
            $("#r1").removeClass("red").removeClass("blue").addClass("off");
            $("#r2").removeClass("red").removeClass("blue").addClass("off");
        } else {
            $("#lb1").removeClass("red").removeClass("blue").addClass("off");
            $("#lb2").removeClass("red").removeClass("blue").addClass("off");
            $("#lb3").removeClass("red").removeClass("blue").addClass("off");
            $("#lb4").removeClass("red").removeClass("blue").addClass("off");
            $("#r1").removeClass("red").removeClass("blue").addClass("off");
            $("#r2").removeClass("red").removeClass("blue").addClass("off");
            $("#frt1").removeClass("red").removeClass("blue").addClass("off");
            $("#frt2").removeClass("red").removeClass("blue").addClass("off");
        }
    });

    $("#togTkdn").bootstrapSwitch({
        size: "mini",
        onColor: "info",
        offColor: "primary",
    });
    $("#togTkdn").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            $("#tk1").removeClass("off").addClass("white");
            $("#tk2").removeClass("off").addClass("white");
        } else {
            $("#tk1").removeClass("white").addClass("off");
            $("#tk2").removeClass("white").addClass("off");
        }
    });

    $("#togCrs").bootstrapSwitch({
        size: "mini",
        onColor: "info",
        offColor: "primary",
    });
    $("#togCrs").on("switchChange.bootstrapSwitch", function (event, state) {
        if (state) {
            $("#pri1").removeClass().addClass("red");
            $("#pri2").removeClass("red").removeClass("blue").addClass("off");
            $("#pri3").removeClass("red").removeClass("blue").addClass("off");
            $("#pri4").removeClass().addClass("blue");
        } else {
            $("#pri1").removeClass("red").removeClass("blue").addClass("off");
            $("#pri2").removeClass("red").removeClass("blue").addClass("off");
            $("#pri3").removeClass("red").removeClass("blue").addClass("off");
            $("#pri4").removeClass("red").removeClass("blue").addClass("off");
        }
    });
});
