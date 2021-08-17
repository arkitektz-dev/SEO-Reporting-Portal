const API_Endpoint = `${baseUrl}/api/generalInquiries`;

$(function () {
    connection.on("ReceiveMessage", function (generalInquiryDto) {
        const focusedUserId = $('.contact.active-user').attr('id');
        if (focusedUserId == generalInquiryDto.userId) {
            const elems = $(".chat-message i").parent().filter((i, elem) => elem.innerText.trim() === generalInquiryDto.message);
            $($(elems[0]).find('i')).remove();
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
            $('#txt-message').val('');
            $(".messages").animate({ scrollTop: $('.messages').prop("scrollHeight") }, 1000);
            const currentTime = moment();
            const sentDate = currentTime.format("DD-mm-yyyy");
            const sentTime = currentTime.format("hh:mm A");
            const messageInfo = {
                positionClass: 'replies',
                avatarKeyword: 'Y',
                message,
                sentTime: sentTime,
                sentDate: sentDate,
                status: 'pending'
            };
            const messageToAppend = messageMarkup(messageInfo);
            $('.chat-list').append(messageToAppend);

            try {
                const { data } = await axios.post(API_Endpoint, { message, userId });

                connection.invoke("SendMessage", userId, data)
                    .catch(function (err) {
                        return console.error(err.toString());
                    });
            } catch (e) {
                const elems = $(".chat-message i").parent().filter((i, elem) => elem.innerText.trim() === message);
                elems.each((i, elem) => {
                    var icon = $(elem).find('i');
                    icon.attr('class', 'fas fa-times');
                    icon.css("color", "red")
                });
            }
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

function makeInboxMarkup(users) {
    return users.map((user, index) => {
        const activeClass = index === 0 ? 'active-user' : '';
        var userName = user.fullName.charAt(0).toUpperCase() + user.fullName.slice(1);
        const recentMessage = {
            sentDate: user.recentMessage !== null ? user.recentMessage.sentDate : '',
            sentTime: user.recentMessage !== null ? user.recentMessage.sentTime : '',
            message: user.recentMessage !== null ? user.recentMessage.message : '',
        };

        return `<li class="contact ${activeClass}" id=${user.id}>
                   <div class="wrap">
                        <div class="avatar-container">
                            <span class="user-avatar ">${userName[0]}</span>
                        </div>
                        <div class="meta">
                           <p class="name user-name">${userName}</p>
                           <p class="user-email">${user.email}</p>
                           <p class="preview recent-message-text">${recentMessage.message}</p>
                        </div>
                   </div>
        </li>`;
    });
}

function makeMessagesMarkup(messages) {
    var userName = $('.contact.active-user .user-name').text();
    userName = userName.charAt(0).toUpperCase() + userName.slice(1);

    $('.user-profile').html(`<span class="user-profile-avatar">${userName[0]}</span><p>${userName}</p>`);

    if (messages.length > 0) {
        return messages.map((message) => {
            const positionClass = message.respondentId !== null ? 'replies' : 'sent';
            const avatarKeyword = message.respondentId !== null ? 'Y' : userName[0];
            const messageInfo = {
                positionClass,
                avatarKeyword,
                message: message.message,
                sentTime: message.sentTime,
                sentDate: message.sentDate
            };

            return messageMarkup(messageInfo);

        }).join('');
    }

}

function changeRecentMessage(message) {
    $(`#${message.userId} .chat_date`).text(message.sentTime);
    $(`#${message.userId} .recent-message-text`).text(message.message);
}

function messageMarkup(messageInfo) {
    return `<li class="${messageInfo.positionClass}">
       <div class="chat-message-container">
          <div>
            <span class="user-chat-avatar">${messageInfo.avatarKeyword}</span>
            <p class="chat-message">
                ${messageInfo.message}
                ${messageInfo.status === 'pending' ? `<i class="far fa-clock"></i>` : ``}
            </p>
          </div>
          <div class="chat-message-time"><span>${messageInfo.sentTime} | ${messageInfo.sentDate}</span></div>
       </div>
    </li>`;
}