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
    if (data.length > 0) {
        const messagesMarkup = makeMessagesMarkup(data);
        $('.chat-list').append(messagesMarkup);
    }
}

function makeMessagesMarkup(messages) {
    var userName = $('.sidebar-header .user-name').text().trim();
    if (messages.length > 0) {
        return messages.map((message) => {
            if (message.respondentId == null) {
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
                        <span class="user-avatar">A</span>
                        <div class="chat-name">Admin</div>
                    </div>
                    <div class="chat-text">${message.message}</div>
                    <div class="chat-hour">${message.sentTime}</div>
                </li>`;
            }
        }).join('');
    }
    else {
        return `<div style = "text-align:center"; class="no-message-container">
                <span class="user-avatar">${userName[0]}</span>
                <h6 class="mt-2">${userName}<h6>
        </div>`;
    }
}

