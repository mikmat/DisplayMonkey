﻿var square = 500;
var scale = 1.0;
var url = "/media/thumb/nnn?width=qqq&height=qqq&mode=2".replace("qqq", square);

$(document).ready(function () {

    $('#BackgroundImage').change(function () {
        var i = this.value;
        if (i > 0) {
            $('#canvas img:first')
                .show()
                .attr("src", url.replace("nnn", i))
            ;
        }
        else {
            $('#canvas img:first')
                .hide()
                .attr("src", "")
            ;
        }
    });

    $('#BackgroundColor').change(function () {
        $('#canvas').css('background-color', this.value);
    });

    $('#Width, #Height').change(function () {
        var w = getWidth() + "px";
        var h = getHeight() + "px";
        $('#canvas').css('width', w).css('height', h);
        $('#canvas img:first').css('max-width', w).css('max-height', h);
    });

    // init #canvas
    $('#Width').change();
    $('#BackgroundColor').change();
    $('#BackgroundImage').change();
});

function getHeight() {
    var w = Number($('#Width')[0].value);
    var h = Number($('#Height')[0].value);
    w = w <= 0 ? 1 : w; h = h <= 0 ? 1 : h;
    return h > w ? square : Math.round(square * h / w);
}

function getWidth() {
    var w = Number($('#Width')[0].value);
    var h = Number($('#Height')[0].value);
    w = w <= 0 ? 1 : w; h = h <= 0 ? 1 : h;
    return w > h ? square : Math.round(square * w / h);
}
