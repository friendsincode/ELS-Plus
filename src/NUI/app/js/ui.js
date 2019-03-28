var resourceName = "";
var soundPlayer = null;
function ChangeUi(uiname) {
    console.log(`Changing ui to ${uiname}`);  
    $.get(`${uiname}/index.html`, function(data) {
        $("#elsui").html(data);
    }).fail(function(){console.log("we has fail")});   
}

$(function () {
    window.addEventListener('message', function (event) {
        if (event.data.type == "initdata") {
            console.log("Setting up ELS UI");
            resourceName = event.data.name;
            ChangeUi(event.data.currentUI);
        } else if (event.data.type == "enableui") {
            document.body.style.display = event.data.enable ? "block" : "none";
        } else if (event.data.type == "click") {
            Click(cursorX - 1, cursorY - 1);
        } else if (event.data.type == "reload") {
            console.log("ELS UI Reload");
            document.location.reload();
        } else if (event.data.type == "playsound") {
            if (soundPlayer != null) {
                soundPlayer.pause();
            }
            soundPlayer = new Howl({ src: [`./js/${event.data.sound}.ogg`] });
            soundPlayer.volume(event.data.volume);
            soundPlayer.play();
        }
    });

    document.onkeyup = function (data) {
        if (data.which == 27) {
            $.post(`http://${resourceName}/escape`, JSON.stringify({}));
        }
    };

});