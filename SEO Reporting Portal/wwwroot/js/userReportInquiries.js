$(function () {
    connection.on("ReceiveReportComment", function (commentDto) {
        var focusedReportId = $('.chat-list-container').find('.active').attr('id');
        if (focusedReportId == commentDto.reportId) {
            console.log(commentDto);
            const messagesMarkup = makeMessagesMarkup([commentDto]);
            $('.no-message-container').remove();
            $('.messages-container').append(messagesMarkup);
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
        var reportId = $('.chat-list-container').find('.active').attr('id');
        if (text.length > 0) {
            const { data } = await axios.post("/api/reports/inquiry", { text, reportId });
            $('#txt-message').val('');
            connection.invoke("SendReportComment", reportId, data).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
        }
    });
});

async function getInquiriesByReport() {
    const { data } = await axios.get("/api/reports/getInquiries");
    if (data.length > 0) {
        const inboxMarkup = makeInboxMarkup(data);
        $('.chat-list-container').append(inboxMarkup);
        const messagesMarkup = makeMessagesMarkup(data[0].comments);
        $('.messages-container').append(messagesMarkup);
    }
}

function makeInboxMarkup(reports) {
    return reports.map((report, index) => {
        const icon = extractFormatIconPath(report.format);

        const recentMessage = {
            sentDate: report.recentComment !== null ? report.recentComment.sentDate : '',
            sentTime: report.recentComment !== null ? report.recentComment.sentTime : '',
            text: report.recentComment !== null ? report.recentComment.text : '',
        };
        const activeClasses = index === 0 ? 'active text-white' : 'list-group-item-light';
        return `<span class="list-group-item list-group-item-action rounded-0 ${activeClasses}" id=${report.id}>
            <div class="media">
                <img src="${icon}" alt="user" width="50">
                <div class="media-body ml-4">
                    <div class="d-flex align-items-center justify-content-between">
                        <h6 class="mb-0 user-name">${report.userFullName}</h6>
                        <small class="small font-weight-bold recent-message-time">${recentMessage.sentTime ? recentMessage.sentTime : ''}</small>
                    </div>
                    <p class="font-italic mb-0 text-small user-email">${report.userEmail}</p>
                    <p class="font-italic mb-0 text-small report-name">${report.name}</p>
                    <p class="font-italic mb-0 text-small recent-message">${recentMessage.text || ''}</p>
                </div>
            </div>
        </span>`
    }).join('');
}

function makeMessagesMarkup(comments) {
    const icon = $('.chat-list-container').find('.active .media img').attr('src');
    if (comments.length > 0) {
        return comments.map((comment) => {
            if (comment.respondentId != null) {
                return `<div class="media w-50 mb-3" id=${comment.id}><img src="/images/icons/icon-user-2.png" alt="user" width="50" class="rounded-circle"><div class="media-body ml-3"><div class="bg-light rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-muted">${comment.text}</p></div><p class="small text-muted">${comment.sentTime} | ${comment.sentDate}</p></div></div>`
            }
            return `<div class="media w-50 ml-auto mb-3" id=${comment.id}><div class="media-body"><div class="bg-primary rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-white">${comment.text}</p></div><p class="small text-muted">${comment.sentTime} | ${comment.sentDate}</p></div></div>`
        }).join('');
    }
    else {
        var userName = $('.chat-list-container').find('.active .user-name').text();
        var userEmail = $('.chat-list-container').find('.active .user-email').text();
        var reportName = $('.chat-list-container').find('.active .report-name').text();
        return `<div style="text-align:center;" class="no-message-container">
           <img src="${icon}" alt="user" width="70">
           <h6 class="mt-2">${userName}<h6>
           <h6 class="mt-2">${userEmail}<h6>
           <h6 class="mt-2">${reportName}<h6>
        </div>`;
    }
}

function changeRecentMessage(comment) {
    $(`#${comment.reportId} .recent-message-time`).text(comment.sentTime);
    $(`#${comment.reportId} .recent-message`).text(comment.text);
}

async function getInquiriesByReportId() {
    var reportId = $(this).attr('id');
    $('.list-group-item').addClass('list-group-item-light');
    $(".list-group-item").removeClass("active text-white");
    $(this).removeClass('list-group-item-light');
    $(this).addClass('active text-white');

    const { data } = await axios.get(`/api/reports/getInquiriesByReportId/${reportId}`);
    console.log(data);
    let messagesMarkup;
    $('.messages-container').empty();
    messagesMarkup = makeMessagesMarkup(data);
    $('.messages-container').append(messagesMarkup);
}

function extractFormatIconPath(format) {
    let iconPath = ''
    switch (format) {
        case '.jpg':
            iconPath = '/images/icons/icon-jpg-64.png';
            break;
        case '.png':
            iconPath = '/images/icons/icon-png-64.png';
            break;
        case '.pdf':
            iconPath = '/images/icons/icon-pdf-64.png';
            break;
        case '.doc':
            iconPath = '/images/icons/icon-word-64.png';
            break;
        default:
            // code block
            break;
    }

    return iconPath;
}
