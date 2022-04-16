
$(document).ready(function () {
    $("*[onclick]").hover(
        function () {
            $(this).addClass('shadow').css('cursor', 'pointer');
        },
        function () {
            $(this).removeClass('shadow');
        }
    );
});

function toIsoString(date) {
    var tzo = -date.getTimezoneOffset(),
        dif = tzo >= 0 ? '+' : '-',
        pad = function (num) {
            return (num < 10 ? '0' : '') + num;
        };

    return date.getFullYear() +
        '-' + pad(date.getMonth() + 1) +
        '-' + pad(date.getDate()) +
        'T' + pad(date.getHours()) +
        ':' + pad(date.getMinutes()) +
        ':' + pad(date.getSeconds()) +
        dif + pad(Math.floor(Math.abs(tzo) / 60)) +
        ':' + pad(Math.abs(tzo) % 60);
}

String.prototype.format = function () {
    a = this;
    for (k in arguments) {
        a = a.replace("#" + k + "#", arguments[k])
    }
    return a
}