var resourceName = "";
var sirenTone = "WL";
//var stageSlide;

function controlLights(state, light, color) {
    if (!state) {
        $(light).removeClass("red").removeClass("blue").removeClass("amber").removeClass("white").addClass("off");
    } else {
        $(light).removeClass("off").addClass(color);
    }
}

function buttonControl(btn, state) {
    if (state) {
        $(btn).addClass("red btn-glow-red").removeClass('btn-glow-neon-green neon-green');

    } else {
        $(btn).removeClass("red btn-glow-red").addClass('btn-glow-neon-green neon-green');
    }
}

function ToggleStage(stage) {
    switch (stage) {
        case 0:
            $('#stage1').removeClass('red btn-glow-red').addClass('off');
            $('#stage2').removeClass('red btn-glow-red').addClass('off');
            $('#stage3').removeClass('red btn-glow-red').addClass('off');
            $("#stageslider").attr("data-slider-value",0);
            break;
        case 1:
            $('#stage1').removeClass('off').addClass('red btn-glow-red');
            $('#stage2').removeClass('red btn-glow-red').addClass('off');
            $('#stage3').removeClass('red btn-glow-red').addClass('off');
            $("#stageslider").attr("data-slider-value",1);

            break;
        case 2:
            $('#stage1').removeClass('off').addClass('red btn-glow-red');
            $('#stage2').removeClass('off').addClass('red btn-glow-red');
            $('#stage3').removeClass('red btn-glow-red').addClass('off');
            $("#stageslider").attr("data-slider-value",2);
            break;
        case 3:
            $('#stage1').removeClass('off').addClass('red btn-glow-red');
            $('#stage2').removeClass('off').addClass('red btn-glow-red');
            $('#stage3').removeClass('off').addClass('red btn-glow-red');
            $("#stageslider").attr("data-slider-value",3);
            break;
    }
}


$(function () {
    window.addEventListener('message', function (event) {
        if (event.data.type == "initdata") {
            resourceName = event.data.name;
        } else if (event.data.type == "enableui") {
            document.body.style.display = event.data.enable ? "block" : "none";
        } else if (event.data.type == "click") {
            Click(cursorX - 1, cursorY - 1);
        } else if (event.data.type == "lightControl") {
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
                    sirenTone = event.data.desc;
                    switch (sirenTone) {
                        case "WL":
                            buttonControl("#wailInd", true);
                            buttonControl("#yelpInd", false);
                            buttonControl("#aux1Ind", false);
                            buttonControl("#aux2Ind", false);
                            break;
                        case "YP":
                            buttonControl("#wailInd", false);
                            buttonControl("#yelpInd", true);
                            buttonControl("#aux1Ind", false);
                            buttonControl("#aux2Ind", false);
                            break;
                        case "A1":
                            buttonControl("#wailInd", false);
                            buttonControl("#yelpInd", false);
                            buttonControl("#aux1Ind", true);
                            buttonControl("#aux2Ind", false);
                            break;
                        case "A2":
                            buttonControl("#wailInd", false);
                            buttonControl("#yelpInd", false);
                            buttonControl("#aux1Ind", false);
                            buttonControl("#aux2Ind", true);
                            break;
                    }
                    break;
                case "HRN":


                    break;
            }
        } else if (event.data.type == "togglestate") {
            switch (event.data.which) {
                case "PRML":
                case "SECL":
                case "WRNL":
                    break;
                case "MAN":
                    buttonControl('#man1Ind', event.data.state);
                    buttonControl('#man1', event.data.state);
                    break;
                case "WW":
                    buttonControl('#wigwagInd', event.data.state);
                    buttonControl('#wigwag', event.data.state);
                    break;
                case "DUAL":
                    buttonControl('#dualToneInd', event.data.state);
                    buttonControl('#dualTone', event.data.state);
                    break;
                case "TDL":
                    buttonControl('#tkdnInd', event.data.state);
                    buttonControl('#tkdn', event.data.state);
                    break;
                case "SCL":
                    buttonControl('#sceneInd', event.data.state);
                    buttonControl('#scene', event.data.state);
                    break;
                case "CRS":
                    buttonControl('#cruiseInd', event.data.state);
                    buttonControl('#cruise', event.data.state);
                    break;
                case "HRN":
                    buttonControl('#hornInd', event.data.state);
                    buttonControl('#horn', event.data.state);
                    break;
                case "SRN":
                    if (event.data.state) {
                        switch (sirenTone) {
                            case "WL":
                                buttonControl("#wail", true);
                                buttonControl("#yelp", false);
                                buttonControl("#aux1", false);
                                buttonControl("#aux2", false);
                                buttonControl("#standby", false);
                                break;
                            case "YP":
                                buttonControl("#wail", false);
                                buttonControl("#yelp", true);
                                buttonControl("#aux1", false);
                                buttonControl("#aux2", false);
                                buttonControl("#standby", false);
                                break;
                            case "A1":
                                buttonControl("#wail", false);
                                buttonControl("#yelp", false);
                                buttonControl("#aux1", true);
                                buttonControl("#aux2", false);
                                buttonControl("#standby", false);
                                break;
                            case "A2":
                                buttonControl("#wail", false);
                                buttonControl("#yelp", false);
                                buttonControl("#aux1", false);
                                buttonControl("#aux2", true);
                                buttonControl("#standby", false);
                                break;
                        }

                    } else {
                        buttonControl("#wail", false);
                        buttonControl("#yelp", false);
                        buttonControl("#aux1", false);
                        buttonControl("#aux2", false);
                        buttonControl("#standby", true);
                    }
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
    //stageSlide = $('#stageslider').slider();
});