$(function () {
    connection.on("ReceiveMessage", function (generalInquiryDto) {
        var focusedUserId = $('.person.active-user').attr('id');
        if (focusedUserId == generalInquiryDto.userId) {
            const messagesMarkup = makeMessagesMarkup([generalInquiryDto]);
            $('.no-message-container').remove();
            $('.chat-list').append(messagesMarkup);
        }
        changeRecentMessage({
            userId: generalInquiryDto.userId,
            sentTime: generalInquiryDto.sentTime,
            sentDate: generalInquiryDto.sentDate,
            message: generalInquiryDto.message
        });
    });

    getGeneralInquiries();

    $(".inbox-users").on("click", 'li', getGeneralInquiriesByUserId);

    $("form").submit(async function (e) {
        e.preventDefault();
        const message = $('#txt-message').val();
        var userId = $('.person.active-user').attr('id');
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
        $('.inbox-users').append(inboxMarkup);
        const messagesMarkup = makeMessagesMarkup(data[0].messages);
        $('.chat-list').append(messagesMarkup);
    }
}

function makeInboxMarkup(users) {
    return users.map((user, index) => {
        const activeClass = index === 0 ? 'active-user' : '';
        const recentMessage = {
            sentDate: user.recentMessage !== null ? user.recentMessage.sentDate : '',
            sentTime: user.recentMessage !== null ? user.recentMessage.sentTime : '',
            message: user.recentMessage !== null ? user.recentMessage.message : '',
        };

        return `<li class="person ${activeClass}" id=${user.id}>
            <div class="chat_people">
                <div class="chat_img"> 
                    <span class="user-avatar ">${user.fullName[0]}</span>
                </div>
                <div class="chat_ib">
                    <h5 class="user-name">${user.fullName} 
                        <span class="chat_date">${recentMessage.sentDate ? recentMessage.sentDate : ''}</span>
                    </h5>
                    <p class="user-email">${user.email}</p>
                    <p class="recent-message-text">${recentMessage.message}</p>
                </div>
            </div>
        </li>`;
    });
}

function makeMessagesMarkup(messages) {
    var userName = $('.person.active-user .user-name').text();

    if (messages.length > 0) {
        return messages.map((message) => {
            if (message.respondentId !== null) {
                return `<li class="chat-right">
                      <div class="chat-hour">${message.sentTime}</div>
                      <div class="chat-text">${message.message}</div>
                      <div class="chat-avatar">
                        <span class="user-avatar">Y</span>
                          <div class="chat-name">You</div>
                      </div>
                </li>`;
            } else {
                return `<li class="chat-left">
                    <div class="chat-avatar">
                        <span class="user-avatar">${userName[0]}</span>
                        <div class="chat-name">${userName}</div>
                    </div>
                    <div class="chat-text">${message.message}</div>
                    <div class="chat-hour">${message.sentTime}</div>
                </li>`;
            }
        }).join('');
    }
    else {
        var userEmail = $('.person.active-user .user-email').text();
        return `<div style = "text-align:center"; class="no-message-container">
                <span class="user-avatar">${userName[0]}</span>
                <h6 class="mt-2">${userName}<h6>
                <h6 class="mt-2">${userEmail}<h6>
        </div>`;
    }
}

function changeRecentMessage(message) {
    $(`#${message.userId} .chat_date`).text(message.sentTime);
    $(`#${message.userId} .recent-message-text`).text(message.message);
}

async function getGeneralInquiriesByUserId() {
    var userId = $(this).attr('id');
    $(".person").removeClass("active-user");
    $(this).addClass('active-user');

    const { data } = await axios.get(`/api/generalInquiries/getGeneralInquiriesByUserId/${userId}`);
    let messagesMarkup;
    $('.chat-list').empty();
    messagesMarkup = makeMessagesMarkup(data);
    $('.chat-list').append(messagesMarkup);
}