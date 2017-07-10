
//$('#memberAddress_Country').change(function () {
//        show_progress();
//        ajax_call('/Common/GetStatesByCountryId?countryId=' + $('#memberAddress_Country').val(), null, OnSuccess, OnError);
//    })

//function OnSuccess(data) {
//        var StatesHtml = '<option value="0">Select State</option>';
//        $(data).each(function () {
//            StatesHtml += '<option value="' + this.StateId + '">' + this.StateName + '</option>';
//        })
//        $('#memberAddress_State').html(StatesHtml);
//        hide_progress();
//}
//function OnError() { alert('Error') }

var autocomplete;
var componentForm =
    {
        street_number: 'short_name', //street number
        route: 'long_name', //street address
        locality: 'long_name', //city
        administrative_area_level_1: 'long_name', //state
        country: 'long_name', //country
        postal_code: 'short_name', //zip
        utc_offset: 'utc_offset'
    };
function initialize() {
    var input = document.getElementById('City');
    autocomplete = new window.google.maps.places.Autocomplete(input, {
        types: ['(cities)']
    });
    window.google.maps.event.addListener(autocomplete, 'place_changed', function () {
        fillInAddress();
    });
}
function fillInAddress() {
    var place = autocomplete.getPlace();
    for (var i = 0; i < place.address_components.length; i++) {
        var addressType = place.address_components[i].types[0];
        var addrval = place.address_components[i][componentForm[addressType]];
        switch (addressType) {
            case 'locality':
                $("#City").val(addrval);
                break;
            case 'administrative_area_level_1':
                $("#State").val(addrval);
                break;
            case 'country':
                $("#country").val(addrval);
                break;
            case 'postal_code':
                $("#PostalCode").val(addrval);
                break;
        }
    }
    $("#TimeZone").val(GetTimeZone(place.utc_offset));
}

function GetTimeZone(utcOffset) {
    debugger
    var TimeZoneVal = '';
    if (utcOffset < 0)
        TimeZoneVal = '';
    else
        TimeZoneVal = '+';

    var hr = '' + parseInt(utcOffset / 60) + '';
    if (hr < 0)
    {
        if (hr.length == 2)
            hr = [hr.slice(0, 1), 0, hr.slice(1)].join('');
    }
    if (hr.length == 1)
        hr = '0' + hr;


    var mint = '' + (utcOffset - (parseInt(utcOffset / 60) * 60)) + '';
    if (mint.length == 1)
        mint = '0' + mint;

    TimeZoneVal = TimeZoneVal + '' + hr + '' + ':' + mint + '';
    return TimeZoneVal;
}
google.maps.event.addDomListener(window, 'load', initialize);


//https://www.igorkromin.net/index.php/2016/06/24/google-maps-now-forcing-developers-to-use-an-api-key-returning-missingkeymaperror/
//https://www.latecnosfera.com/2016/06/google-maps-api-error-missing-keymap-error-solved.html
