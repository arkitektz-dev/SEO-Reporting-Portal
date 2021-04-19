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
            const { data } = await axios.post("/api/generalInquiries", { message });
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
    const { data } = await axios.get(`/api/generalInquiries/getGeneralInquiriesByUserId`);
    let messagesMarkup;
    $('.messages-container').empty();
    messagesMarkup = makeMessagesMarkup(data);
    $('.messages-container').append(messagesMarkup);
}

function makeMessagesMarkup(messages) {
    return messages.map((message) => {
        if (message.respondentId != null) {
            return `<div class="media w-50 mb-3" id=${message.id}><img src="/images/icons/icon-user-2.png" alt="user" width="50" class="rounded-circle"><div class="media-body ml-3"><div class="bg-light rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-muted">${message.message}</p></div><p class="small text-muted">${message.sentTime} | ${message.sentDate}</p></div></div>`
        }
        return `<div class="media w-50 ml-auto mb-3" id=${message.id}><div class="media-body"><div class="bg-primary rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-white">${message.message}</p></div><p class="small text-muted">${message.sentTime} | ${message.sentDate}</p></div></div>`
    }).join('');
}

