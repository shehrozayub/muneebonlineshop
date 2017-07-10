$(document).ready(function () {
    $(".naviList > ul > li > a").click(function () {
        if (!$(this).hasClass('active')) {
            $(".subnav").slideUp(300);
            $(this).next(".subnav").slideDown(300);
            $(".naviList > ul > li > a").removeClass("active");
            $(this).addClass("active");
        }
        else if ($(this).hasClass('active')) {
            $(this).next(".subnav").slideUp(300);
            $(this).removeClass("active");
        }
    });

    /*-------------- Make the link selected ------------*/
    var SelectedElement = $('a[href="' + window.location.pathname.toLowerCase() + '"]');
    if ($(SelectedElement).parent().parent().parent().attr("class") == "naviList")
        SelectedElement.addClass('active')
    else {
        $(SelectedElement).parent().parent().prev().click();
        //  $(SelectedElement).parent().parent().prev().trigger("click");
        $(SelectedElement).css("color", "blue")
    }

    /*--------------Setting right------------*/

    $(".settingIcon").click(function () {
        $(".settingList").slideToggle();
    });

    // Date picker controlls
});

$(document).mouseup(function (e) {
    var container = $(".settingIcon ");
    var container1 = $(".settingList");
    var container2 = $(".settingList *");
    if (!container.is(e.target) && !container1.is(e.target) && !container2.is(e.target) && container.has(e.target).length === 0) {
        $(".settingList").slideUp(300);
    }
});

function convertDateFormat(input) {
    var datePart = input.match(/\d+/g);
    var day = datePart[0]
    var month = datePart[1]
    var year = datePart[2];
    return month + '/' + day + '/' + year;
}

function ajax_call(PageURL, PostData, OnSuccessFunction, OnErrorFunction) {
    show_progress();
    $.ajax({
        type: "POST",
        url: PageURL,
        data: PostData,
        async: false,
        contentType: "application/json; charset=utf-8",
        success: OnSuccessFunction,
        error: OnErrorFunction
    }).done(hide_progress);
}

function chkValidEmail(emailtext) {
    var pattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    if (pattern.test(emailtext)) {
        return true;
    }
    else {
        alert("Please enter valid email");
        return false;
    }
}

function alphanumeric_only(key) {
    var keycode = (key.which) ? key.which : key.keyCode;
    if ((keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) || (keycode >= 48 && keycode <= 57))
        return true;
    else
        alert("Please enter only alpha numeric");
    return false;
}


$(document).ready(function () {
    $(".howImage").click(function () {
        $("#videoCon1").fadeIn(200);
        $(".videoBg").get(0).play();
    });
    $(".xclose").click(function () {
        $("#videoCon1").fadeOut(200);
        $(".videoBg").get(0).pause();
    });
    $(".videocall").click(function () {
        $(".arooetabaudioo").addClass("arooetabvideo");
        $(this).addClass("active");
        $(".audiocall").removeClass("active");
    });
    $(".audiocall").click(function () {
        $(".arooetabaudioo").removeClass("arooetabvideo");
        $(this).addClass("active");
        $(".videocall").removeClass("active");
    });

    //$(".inbox").niceScroll();


    checkWidthheight();

    //var nice = false;

    //nice = $(".inbox").niceScroll({ cursorcolor: "#ddd", autohidemode: false });

    //var nice1 = false;

    //nice1 = $(".inbox1").niceScroll({ cursorcolor: "#ddd", autohidemode: false });

});

function checkWidthheight() {
    var getheight = $(window).height();
    var topleft = $(".aside-user").height() + 60;
    setTimeout(function () {
        $(".inbox").css("height", getheight - topleft);
    }, 500);
    setTimeout(function () {
        $(".inbox1").css("height", getheight - 360);
    }, 500);

    var getwidth = $(window).width();
    $(".rightaside").css("width", getwidth - 310);


    setTimeout(function () {
        var topleftacc = $(".myaccfornt").height();

        //alert(topleftacc);

        if (topleftacc >= 810) {
            $(".myacc_left").css({ minHeight: topleftacc });
        }
    }, 500);

}


$(window).resize(function () {
    checkWidthheight();
});

