const API_Endpoint = `${baseUrl}/api/generalInquiries`;

$(function () {
    connection.on("ReceiveMessage", function (generalInquiryDto) {
        const messagesMarkup = makeMessagesMarkup([generalInquiryDto]);
        $('.chat-list').append(messagesMarkup);
    });

    getGeneralInquiries();

    $("form").submit(async function (e) {
        e.preventDefault();
        const message = $('#txt-message').val();
        if (message.length > 0) {
            const { data } = await axios.post(API_Endpoint, { message });
            $('#txt-message').val('');
            $(".messages").animate({ scrollTop: $('.messages').prop("scrollHeight") }, 1000);
            connection.invoke("SendMessage", data.userId, data).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
        }
    });
});

async function getGeneralInquiries() {
    const { data } = await axios.get(`${API_Endpoint}/getGeneralInquiriesByUserId`);
    const messagesMarkup = makeMessagesMarkup(data);
    $('.chat-list').append(messagesMarkup);
}

function makeMessagesMarkup(messages) {
    var userName = $('.sidebar-header .user-name').text().trim();
    userName = userName.charAt(0).toUpperCase() + userName.slice(1);
    $('.user-profile').html(`<span class="user-profile-avatar">${userName[0]}</span><p>${userName}</p>`);
    if (messages.length > 0) {
        return messages.map((message) => {
            const positionClass = message.respondentId === null ? 'sent' : 'replies';
            const avatarKeyword = message.respondentId === null ? userName[0] : 'A';
            return `<li class="${positionClass}">
                        <div class="chat-message-container">
                            <div>
                                <span class="user-chat-avatar">${avatarKeyword}</span>
                                <p class="chat-message">${message.message}</p>
                            </div>
                            <div class="chat-message-time"><span>${message.sentTime} | ${message.sentDate}</span></div>
                        </div>
                      
            </li>`;
        }).join('');
    }
}

