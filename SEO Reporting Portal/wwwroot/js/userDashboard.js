$(function () {
    connection.on("ReceiveMessage", function (generalInquiryDto) {
        const messagesMarkup = makeMessagesMarkup([generalInquiryDto]);
        $('.messages-container').append(messagesMarkup);
    });

    getGeneralInquiries();

    $("form").submit(async function (e) {
        e.preventDefault();
        const message = $('#txt-message').val();
        if (message.length > 0) {
            const { data } = await axios.post("/api/GeneralInquiries", { message });
            $('#txt-message').val('');
            connection.invoke("SendMessage", data.userId, data).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
        }
    });
});

async function getGeneralInquiries() {
    const { data } = await axios.get(`/api/GeneralInquiries/GetGeneralInquiriesByUserId`);
    let messagesMarkup;
    if (data.length > 0) {
        $('.messages-container').empty();
        messagesMarkup = makeMessagesMarkup(data);
    }
    $('.messages-container').append(messagesMarkup);
}

function makeInboxMarkup(users) {
    return users.map((user, index) => {
        const activeClasses = index === 0 ? 'active text-white' : 'list-group-item-light';
        return `<span class="list-group-item list-group-item-action rounded-0 ${activeClasses}" id=${user.id}><div class="media"><img src="https://res.cloudinary.com/mhmd/image/upload/v1564960395/avatar_usae7z.svg" alt="user" width="50" class="rounded-circle"><div class="media-body ml-4"><div class="d-flex align-items-center justify-content-between mb-1"><h6 class="mb-0">${user.fullName}</h6><small class="small font-weight-bold">${user.recentMessage.sentDate} | ${user.recentMessage.sentTime}</small></div><p class="font-italic mb-0 text-small">${user.recentMessage.message}</p></div></div></span>`
    }).join('');
}

function makeMessagesMarkup(messages) {
    return messages.map((message) => {
        if (message.respondentId != null) {
            return `<div class="media w-50 mb-3" id=${message.id}><img src="https://res.cloudinary.com/mhmd/image/upload/v1564960395/avatar_usae7z.svg" alt="user" width="50" class="rounded-circle"><div class="media-body ml-3"><div class="bg-light rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-muted">${message.message}</p></div><p class="small text-muted">${message.sentTime} | ${message.sentDate}</p></div></div>`
        }
        return `<div class="media w-50 ml-auto mb-3" id=${message.id}><div class="media-body"><div class="bg-primary rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-white">${message.message}</p></div><p class="small text-muted">${message.sentTime} | ${message.sentDate}</p></div></div>`
    }).join('');
}

