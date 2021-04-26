const API_Endpoint = `${baseUrl}/api/generalInquiries`;

$(function () {

    connection.on("ReceiveMessage", function (generalInquiryDto) {
        var focusedUserId = $('.contact.active-user').attr('id');
        if (focusedUserId == generalInquiryDto.userId) {
            const messagesMarkup = makeMessagesMarkup([generalInquiryDto]);
            //$('.no-message-container').remove();
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
        var userId = $('.contact.active-user').attr('id');
        if (message.length > 0) {
            const { data } = await axios.post(API_Endpoint, { message, userId });
            $('#txt-message').val('');
            $(".messages").animate({ scrollTop: $('.messages').prop("scrollHeight") }, 1000);
            connection.invoke("SendMessage", userId, data).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
        }
    });
});

async function getGeneralInquiries() {
    const { data } = await axios.get(API_Endpoint);
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

        return `<li class="contact ${activeClass}" id=${user.id}>
                   <div class="wrap">
                        <div class="avatar-container">
                            <span class="user-avatar ">${user.fullName[0]}</span>
                        </div>
                        <div class="meta">
                           <p class="name user-name">${user.fullName}</p>
                           <p class="user-email">${user.email}</p>
                           <p class="preview recent-message-text">${recentMessage.message}</p>
                        </div>
                   </div>
        </li>`;
    });
}

function makeMessagesMarkup(messages) {
    var userName = $('.contact.active-user .user-name').text();

    $('.user-profile').html(`<span class="user-profile-avatar">${userName[0]}</span><p>${userName}</p>`);

    if (messages.length > 0) {
        return messages.map((message) => {
            const positionClass = message.respondentId !== null ? 'sent' : 'replies';
            const avatarKeyword = message.respondentId !== null ? 'Y' : userName[0];
            return `<li class="${positionClass}">
                       <span class="user-chat-avatar">${avatarKeyword}</span>
                       <p class="chat-message">${message.message}</p>
                       <div class="chat-time"><p>${message.sentTime} | ${message.sentDate}</p></div>
            </li>`;
        }).join('');
    }

}

function changeRecentMessage(message) {
    $(`#${message.userId} .chat_date`).text(message.sentTime);
    $(`#${message.userId} .recent-message-text`).text(message.message);
}

async function getGeneralInquiriesByUserId() {
    var userId = $(this).attr('id');
    $(".contact").removeClass("active-user");
    $(this).addClass('active-user');

    const { data } = await axios.get(`${API_Endpoint}/getGeneralInquiriesByUserId/${userId}`);
    let messagesMarkup;
    $('.chat-list').empty();
    messagesMarkup = makeMessagesMarkup(data);
    $('.chat-list').append(messagesMarkup);
}