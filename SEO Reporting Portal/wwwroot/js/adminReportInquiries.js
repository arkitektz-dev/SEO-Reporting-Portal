﻿const API_Endpoint = `${baseUrl}/api/reports`;

$(function () {

    connection.on("ReceiveReportComment", function (commentDto) {
        var focusedReportId = $('.contact.active-user').attr('id');
        if (focusedReportId == commentDto.reportId) {
            const messagesMarkup = makeMessagesMarkup([commentDto]);
            //$('.no-message-container').remove();
            $('.chat-list').append(messagesMarkup);
        }
        changeRecentMessage({
            reportId: commentDto.reportId,
            sentTime: commentDto.sentTime,
            sentDate: commentDto.sentDate,
            text: commentDto.text
        });
    });

    getInquiriesByReport();

    $(".chat-list-container").on("click", '.list-group-item', getInquiriesByReportId);

    $("form").submit(async function (e) {
        e.preventDefault();
        const text = $('#txt-message').val();
        var reportId = $('.contact.active-user').attr('id');
        if (text.length > 0) {
            const { data } = await axios.post(`${API_Endpoint}/inquiry`, { text, reportId });
            $('#txt-message').val('');
            $(".messages").animate({ scrollTop: $('.messages').prop("scrollHeight") }, 1000);
            connection.invoke("SendReportComment", reportId, data).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
        }
    });
});

async function getInquiriesByReport() {
    const { data } = await axios.get(`${API_Endpoint}/getInquiries`);
    if (data.length > 0) {
        const inboxMarkup = makeInboxMarkup(data);
        $('.inbox-users').append(inboxMarkup);
        const messagesMarkup = makeMessagesMarkup(data[0].comments);
        $('.chat-list').append(messagesMarkup);
    }
}

async function getInquiriesByReportId() {
    var reportId = $(this).attr('id');
    $(".contact").removeClass("active-user");
    $(this).addClass('active-user');

    const { data } = await axios.get(`${API_Endpoint}/getInquiriesByReportId/${reportId}`);
    let messagesMarkup;
    $('.chat-list').empty();
    messagesMarkup = makeMessagesMarkup(data);
    $('.chat-list').append(messagesMarkup);
}

function makeInboxMarkup(reports) {
    return reports.map((report, index) => {
        const activeClass = index === 0 ? 'active-user' : '';
        const icon = extractFormatIconPath(report.format);
        const recentMessage = {
            sentDate: report.recentComment !== null ? report.recentComment.sentDate : '',
            sentTime: report.recentComment !== null ? report.recentComment.sentTime : '',
            text: report.recentComment !== null ? report.recentComment.text : '',
        };
        return `<li class="contact ${activeClass}" id=${report.id}>
                   <div class="wrap">
                        <div class="avatar-container" style="height: 65px;">
                            <img src="${icon}" alt="report">
                        </div>
                        <div class="meta">
                           <p class="name user-name">${report.userFullName}</p>
                           <p class="user-email">${report.userEmail}</p>
                           <p class="report-name">${report.name}</p>
                           <p class="preview recent-message-text">${recentMessage.text}</p>
                        </div>
                   </div>
        </li>`;
    }).join('');
}

function makeMessagesMarkup(comments) {
    const icon = $('.contact.active-user img').attr('src');
    const reportName = $('.contact.active-user .report-name').text();
    $('.user-profile').html(`<img src="${icon}" alt="user" class="rounded-circle" /><p>${reportName}</p>`);

    if (comments.length > 0) {
        return comments.map((comment) => {
            const positionClass = comment.respondentId !== null ? 'sent' : 'replies';
            return `<li class="${positionClass}">
                        <div class="chat-message-container">
                            <div>
                               <img src="${icon}" alt="user" class="rounded-circle" />
                               <p class="chat-message">${comment.text}</p>
                            </div>
                           <div class="chat-message-time"><span>${comment.sentTime} | ${comment.sentDate}</span></div>
                        </div>
            </li>`;
        }).join('');
    }
}

function changeRecentMessage(comment) {
    $(`#${comment.reportId} .chat_date`).text(comment.sentTime);
    $(`#${comment.reportId} .recent-message-text`).text(comment.text);
}

function extractFormatIconPath(format) {
    let iconPath = ''
    switch (format) {
        case '.jpg':
            iconPath = `${baseUrl}/images/icons/icon-jpg-64.png`;
            break;
        case '.png':
            iconPath = `${baseUrl}/images/icons/icon-png-64.png`;
            break;
        case '.pdf':
            iconPath = `${baseUrl}/images/icons/icon-pdf-64.png`;
            break;
        case '.doc':
            iconPath = `${baseUrl}/images/icons/icon-word-64.png`;
            break;
        default:
            // code block
            break;
    }

    return iconPath;
}
