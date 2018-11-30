var resourceName = "";

function controlLights(state, light, color) {
    if (!state) {
        $(light).removeClass("light-red").removeClass("light-blue").removeClass("light-amber").removeClass("light-white").addClass("light-off");
    } else {
        $(light).removeClass("light-off").addClass("light-" + color);
    }
}

function ToggleStage(stage) {
    switch (stage) {
        case 0:
            $('#stage1').removeClass('stage-on').addClass('stage-off');
            $('#stage2').removeClass('stage-on').addClass('stage-off');
            $('#stage3').removeClass('stage-on').addClass('stage-off');
            break;
        case 1:
            $('#stage1').removeClass('stage-off').addClass('stage-on');
            $('#stage2').removeClass('stage-on').addClass('stage-off');
            $('#stage3').removeClass('stage-on').addClass('stage-off');
            break;
        case 2:
            $('#stage1').removeClass('stage-off').addClass('stage-on');
            $('#stage2').removeClass('stage-off').addClass('stage-on');
            $('#stage3').removeClass('stage-on').addClass('stage-off');
            break;
        case 3:
            $('#stage1').removeClass('stage-off').addClass('stage-on');
            $('#stage2').removeClass('stage-off').addClass('stage-on');
            $('#stage3').removeClass('stage-off').addClass('stage-on');
            break;
    }
}


$(function () {
    window.addEventListener('message', function (event) {
        if (event.data.type == "initdata") {
            resourceName = event.data.name;
        }
        else if (event.data.type == "enableui") {
            document.body.style.display = event.data.enable ? "block" : "none";
        } else if (event.data.type == "click") {
            Click(cursorX - 1, cursorY - 1);
        } else if (event.data.type == "lightControl") {
            controlLights(event.data.state, event.data.light, event.data.color);
        } else if (event.data.type == "setuidesc") {
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
            var oppstate = event.data.state ? "off" : "on";
			switch (event.data.which) {
                case "PRML":
					$('#prmBox').removeClass('stage-' + oppstate).addClass('stage-' + state);
					$('#prmPatt').removeClass('text-stage-' + oppstate).addClass('text-stage-' + state);
                    break;
                case "SECL":
					$('#secBox').removeClass('stage-' + oppstate).addClass('stage-' + state);
					$('#secPatt').removeClass('text-stage-' + oppstate).addClass('text-stage-' + state);
                    break;
                case "WRNL":
					$('#wrnBox').removeClass('stage-' + oppstate).addClass('stage-' + state);
					$('#wrnPatt').removeClass('text-stage-' + oppstate).addClass('text-stage-' + state);
                    break;
                case "TDL":
					"off" == oppstate && (oppstate = "grey"), "off" == state && (state = "grey");
					$('#togTkdn').removeClass('stage-' + oppstate).addClass('stage-' + state);
					
					"off" == oppstate && (oppstate = "grey"), "on" == oppstate && (oppstate = "white"), "off" == state && (state = "grey"), "on" == state && (state = "white");
					$('#extra12-1').removeClass('light-' + oppstate).addClass('light-' + state);
					$('#extra12-2').removeClass('light-' + oppstate).addClass('light-' + state);
                    break;
                case "SCL":
					"off" == oppstate && (oppstate = "grey"), "off" == state && (state = "grey");
					$('#togScl').removeClass('stage-' + oppstate).addClass('stage-' + state);
                    break;
                case "CRS":
					"off" == oppstate && (oppstate = "grey"), "off" == state && (state = "grey");
					$('#togCrs').removeClass('stage-' + oppstate).addClass('stage-' + state);
                    break;
                case "HRN":
					"off" == oppstate && (oppstate = "grey"), "off" == state && (state = "grey");
					$('#hrnBox').removeClass('stage-' + oppstate).addClass('stage-' + state);
					$('#hrnType').removeClass('text-stage-' + oppstate).addClass('text-stage-' + state);
                    break;
                case "SRN":
					"off" == oppstate && (oppstate = "grey"), "off" == state && (state = "grey");
					$('#srnBox').removeClass('stage-' + oppstate).addClass('stage-' + state);
					$('#srnType').removeClass('text-stage-' + oppstate).addClass('text-stage-' + state);
                    break;
            }
        } else if (event.data.type == "togglestage") {
            ToggleStage(event.data.stage);
        }
    });

    document.onkeyup = function (data) {
        if (data.which == 27) {
            $.post(`http://${resourceName}/escape`, JSON.stringify({}));
        }
    };

});
