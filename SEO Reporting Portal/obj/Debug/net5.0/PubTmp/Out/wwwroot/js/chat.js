var connection = new signalR.HubConnectionBuilder().withUrl(`${baseUrl}/chatHub`).build();

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveMessage", function (generalInquiryDto) {
    if (window.location.pathname !== '/' && window.location.pathname !== '/dashboard') {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-bottom-right",
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "300000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        toastr.info(`You received a message`);
    }
});

