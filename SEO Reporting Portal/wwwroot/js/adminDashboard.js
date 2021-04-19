$(function () {
    connection.on("ReceiveMessage", function (generalInquiryDto) {
        var focusedUserId = $('.chat-list-container').find('.active').attr('id');
        if (focusedUserId == generalInquiryDto.userId) {
            const messagesMarkup = makeMessagesMarkup([generalInquiryDto]);
            $('.no-message-container').remove();
            $('.messages-container').append(messagesMarkup);
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
            const { data } = await axios.post("/api/generalInquiries", { message, userId });
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
        return `<span class="list-group-item list-group-item-action rounded-0 ${activeClasses}" id=${user.id}>
            <div class="media">
                <img src="/images/icons/icon-user-2.png" alt="user" width="50" class="rounded-circle">
                <div class="media-body ml-4">
                    <div class="d-flex align-items-center justify-content-between">
                        <h6 class="mb-0 user-name">${user.fullName}</h6>
                        <small class="small font-weight-bold recent-message-time">${recentMessage.sentTime ? recentMessage.sentTime : ''}</small>
                    </div>
                    <p class="font-italic mb-0 text-small user-email">${user.email}</p>
                    <p class="font-italic mb-0 text-small recent-message">${recentMessage.message || ''}</p>
                </div>
            </div>
        </span>`
    }).join('');
}

function makeMessagesMarkup(messages) {
    if (messages.length > 0) {
        return messages.map((message) => {
            if (message.respondentId == null) {
                return `<div class="media w-50 mb-3" id=${message.id}><img src="/images/icons/icon-user-2.png" alt="user" width="50" class="rounded-circle"><div class="media-body ml-3"><div class="bg-light rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-muted">${message.message}</p></div><p class="small text-muted">${message.sentTime} | ${message.sentDate}</p></div></div>`
            }
            return `<div class="media w-50 ml-auto mb-3" id=${message.id}><div class="media-body"><div class="bg-primary rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-white">${message.message}</p></div><p class="small text-muted">${message.sentTime} | ${message.sentDate}</p></div></div>`
        }).join('');
    }
    else {
        var userName = $('.chat-list-container').find('.active .user-name').text();
        var userEmail = $('.chat-list-container').find('.active .user-email').text();
        return `<div style="text-align:center"; class="no-message-container">
            <img src="/images/icons/icon-user-2.png" alt="user" width="70" class="rounded-circle">
            <h6 class="mt-2">${userName}<h6>
            <h6 class="mt-2">${userEmail}<h6>
        </div>`;
    }
}

function changeRecentMessage(message) {
    $(`#${message.userId} .recent-message-time`).text(message.sentTime);
    $(`#${message.userId} .recent-message`).text(message.message);
}

async function getGeneralInquiriesByUserId() {
    var userId = $(this).attr('id');
    $('.list-group-item').addClass('list-group-item-light');
    $(".list-group-item").removeClass("active text-white");
    $(this).removeClass('list-group-item-light');
    $(this).addClass('active text-white');

    const { data } = await axios.get(`/api/generalInquiries/getGeneralInquiriesByUserId/${userId}`);
    let messagesMarkup;
    $('.messages-container').empty();
    messagesMarkup = makeMessagesMarkup(data);
    $('.messages-container').append(messagesMarkup);
}