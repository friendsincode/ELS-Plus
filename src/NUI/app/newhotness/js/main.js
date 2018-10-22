
var euro = false;

function controlLights(state, light, color) {
    if (!state) {
        $(light).removeClass("red").removeClass("blue").removeClass("amber").removeClass("white").addClass("off");
    } else {
        $(light).removeClass("off").addClass(color);
    }
}

function ToggleStage(stage) {
    switch (stage) {
        case 0:
            $('#stage1').removeClass('badge-primary').addClass('badge-secondary');
            $('#stage2').removeClass('badge-primary').addClass('badge-secondary');
            $('#stage3').removeClass('badge-primary').addClass('badge-secondary');
            if (euro) {
                $('#euro').removeClass('badge-primary').addClass('badge-secondary');
            }
            break;
        case 1:
            $('#stage1').removeClass('badge-secondary').addClass('badge-primary');
            $('#stage2').removeClass('badge-primary').addClass('badge-secondary');
            $('#stage3').removeClass('badge-primary').addClass('badge-secondary');
            break;
        case 2:
            $('#stage1').removeClass('badge-secondary').addClass('badge-primary');
            $('#stage2').removeClass('badge-secondary').addClass('badge-primary');
            $('#stage3').removeClass('badge-primary').addClass('badge-secondary');
            break;
        case 3:
            $('#stage1').removeClass('badge-secondary').addClass('badge-primary');
            $('#stage2').removeClass('badge-secondary').addClass('badge-primary');
            $('#stage3').removeClass('badge-secondary').addClass('badge-primary');
            if (euro) {
                $('#euro').removeClass('badge-secondary').addClass('badge-primary');
            }
            break;
    }
}


$(function () {
    window.addEventListener('message', function (event) {
       if (event.data.type == "lightControl") {
            controlLights(event.data.state, event.data.light, event.data.color);
        } else if (event.data.type == "setuidesc") {
            //console.log(`setting ${event.data.lighttype} to pattern ${event.data.desc}`);
            switch (event.data.uielement) {
                case "PRML":
                    $("#prmPatt").text(event.data.desc);
                    break;
                case "SECL":
                    $("#secPatt").text(event.data.desc);
                    break;
                case "WRNL":
                    $("#wrnPatt").text(event.data.desc);
                    break;
                case "SRN":
                    $("#srnType").text(event.data.desc);
                    break;
                case "HRN":
                    $("#hrnType").text(event.data.desc);
                    break;
            }
        } else if (event.data.type == "togglestate") {
            var state = event.data.state ? "on" : "off";
            //console.log("Toggle state " + state);
            switch (event.data.which) {
                case "PRML":
                    $("#togPri").bootstrapToggle(state);
                    break;
                case "SECL":
                    $("#togSec").bootstrapToggle(state);
                    break;
                case "WRNL":
                    $("#togWrn").bootstrapToggle(state);
                    break;
                case "TDL":
                    $("#togTkdn").bootstrapToggle(state);
                    break;
                case "SCL":
                    $("#togScl").bootstrapToggle(state);
                    break;
                case "CRS":
                    $("#togCrs").bootstrapToggle(state);
                    break;
                case "HRN":
                    $("#togHrn").bootstrapToggle(state);
                    break;
                case "SRN":
                    $("#togSrn").bootstrapToggle(state);
                    break;
            }
        } else if (event.data.type == "togglestage") {
            ToggleStage(event.data.stage);
        } else if (event.data.type == "seteuro") {
            euro = event.data.euro;
            if (euro) {
                $("#euro").show();
            } else {
                $("#euro").hide();
            }
        }
    });

    document.onkeyup = function (data) {
        if (data.which == 27) {
            $.post(`http://${resourceName}/escape`, JSON.stringify({}));
        }
    };

});


$(document).ready(function () {
    console.log("document ready running function");
    $("#togPri").bootstrapToggle({ on: 'PRM', off: 'PRM' });
    $("#togSec").bootstrapToggle({ on: 'SEC', off: 'SEC' });
    $("#togWrn").bootstrapToggle({ on: 'WRN', off: 'WRN' });
    $("#togHrn").bootstrapToggle({ on: 'HRN', off: 'HRN' });
    $("#togSrn").bootstrapToggle({ on: 'SRN', off: 'SRN' });
    $("#togSrm").bootstrapToggle({ on: 'SRM', off: 'SRM' });
    $("#euro").hide();
});
