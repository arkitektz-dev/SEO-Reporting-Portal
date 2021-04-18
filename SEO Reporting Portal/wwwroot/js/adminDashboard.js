$(function () {
    connection.on("ReceiveMessage", function (generalInquiryDto) {
        var focusedUserId = $('.chat-list-container').find('.active').attr('id');
        if (focusedUserId == generalInquiryDto.userId) {
            const messagesMarkup = makeMessagesMarkup([generalInquiryDto]);
            $('.messages-container').append(messagesMarkup);
            //toastr.info(`You received a message`, { "positionClass": "toast-bottom-right", "closeButton": true });
        }
        changeRecentMessage({
            userId: generalInquiryDto.userId,
            sentTime: generalInquiryDto.sentTime,
            sentDate: generalInquiryDto.sentDate,
            message: generalInquiryDto.message
        });
    });

    getGeneralInquiries();

    $(".chat-list-container").on("click", '.list-group-item', getGeneralInquiriesByUserId);

    $("form").submit(async function (e) {
        e.preventDefault();
        const message = $('#txt-message').val();
        var userId = $('.chat-list-container').find('.active').attr('id');
        if (message.length > 0) {
            const { data } = await axios.post("/api/GeneralInquiries", { message, userId });
            $('#txt-message').val('');
            connection.invoke("SendMessage", userId, data).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
        }
    });
});

async function getGeneralInquiries() {
    const { data } = await axios.get("/api/generalInquiries");
    if (data.length > 0) {
        const inboxMarkup = makeInboxMarkup(data);
        $('.chat-list-container').append(inboxMarkup);
        const messagesMarkup = makeMessagesMarkup(data[0].messages);
        $('.messages-container').append(messagesMarkup);
    }
}

function makeInboxMarkup(users) {
    return users.map((user, index) => {
        const recentMessage = {
            sentDate: user.recentMessage !== null ? user.recentMessage.sentDate : '',
            sentTime: user.recentMessage !== null ? user.recentMessage.sentTime : '',
            message: user.recentMessage !== null ? user.recentMessage.message : '',
        };
        const activeClasses = index === 0 ? 'active text-white' : 'list-group-item-light';
        return `<span class="list-group-item list-group-item-action rounded-0 ${activeClasses}" id=${user.id}><div class="media"><img src="https://res.cloudinary.com/mhmd/image/upload/v1564960395/avatar_usae7z.svg" alt="user" width="50" class="rounded-circle"><div class="media-body ml-4"><div class="d-flex align-items-center justify-content-between mb-1"><h6 class="mb-0">${user.fullName}</h6><small class="small font-weight-bold">${recentMessage.sentDate} | ${recentMessage.sentTime || ''}</small></div><p class="font-italic mb-0 text-small">${recentMessage.message || ''}</p></div></div></span>`
    }).join('');
}

function makeMessagesMarkup(messages) {
    return messages.map((message) => {
        if (message.respondentId == null) {
            return `<div class="media w-50 mb-3" id=${message.id}><img src="https://res.cloudinary.com/mhmd/image/upload/v1564960395/avatar_usae7z.svg" alt="user" width="50" class="rounded-circle"><div class="media-body ml-3"><div class="bg-light rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-muted">${message.message}</p></div><p class="small text-muted">${message.sentTime} | ${message.sentDate}</p></div></div>`
        }
        return `<div class="media w-50 ml-auto mb-3" id=${message.id}><div class="media-body"><div class="bg-primary rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-white">${message.message}</p></div><p class="small text-muted">${message.sentTime} | ${message.sentDate}</p></div></div>`
    }).join('');
}

function changeRecentMessage(message) {
    $(`#${message.userId} .media-body small`).text(`${message.sentTime} | ${message.sentDate}`);
    $(`#${message.userId} .media-body p`).text(message.message);
}

async function getGeneralInquiriesByUserId() {
    var userId = $(this).attr('id');
    $('.list-group-item').addClass('list-group-item-light');
    $(".list-group-item").removeClass("active text-white");
    $(this).removeClass('list-group-item-light');
    $(this).addClass('active text-white');

    const { data } = await axios.get(`/api/GeneralInquiries/GetGeneralInquiriesByUserId/${userId}`);
    let messagesMarkup;
    $('.messages-container').empty();
    if (data.length > 0) {
        messagesMarkup = makeMessagesMarkup(data);
    }
    $('.messages-container').append(messagesMarkup);
}