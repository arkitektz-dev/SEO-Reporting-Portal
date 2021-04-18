$(function () {
    connection.on("ReceiveReportComment", function (commentDto) {
        const messagesMarkup = makeMessagesMarkup([commentDto]);    
        console.log(messagesMarkup);
        changeRecentMessage({
            reportId: commentDto.reportId,
            sentTime: commentDto.sentTime,
            sentDate: commentDto.sentDate,
            text: commentDto.text
        });
        $('.messages-container').append(messagesMarkup);
    });

    getCommentsByReport();

    $(".chat-list-container").on("click", '.list-group-item', getCommentsByReportId);

    $("form").submit(async function (e) {
        e.preventDefault();
        const text = $('#txt-message').val();
        var reportId = $('.chat-list-container').find('.active').attr('id');
        if (text.length > 0) {
            const { data } = await axios.post("/api/reports/comment", { text, reportId });
            $('#txt-message').val('');
            connection.invoke("SendReportComment", reportId, data).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
        }
    });
});

async function getCommentsByReport() {
    const { data } = await axios.get("/api/reports/getComments");
    console.log(data);
    if (data.length > 0) {
        const inboxMarkup = makeInboxMarkup(data);
        $('.chat-list-container').append(inboxMarkup);
        const messagesMarkup = makeMessagesMarkup(data[0].comments);
        $('.messages-container').append(messagesMarkup);
    }
}

function makeInboxMarkup(reports) {
    return reports.map((report, index) => {
        const recentComment = {
            sentDate: report.recentComment !== null ? report.recentComment.sentDate : '',
            sentTime: report.recentComment !== null ? report.recentComment.sentTime : '',
            text: report.recentComment !== null ? report.recentComment.text : '',
        };
        const activeClasses = index === 0 ? 'active text-white' : 'list-group-item-light';
        return `<span class="list-group-item list-group-item-action rounded-0 ${activeClasses}" id=${report.id}><div class="media"><img src="https://res.cloudinary.com/mhmd/image/upload/v1564960395/avatar_usae7z.svg" alt="user" width="50" class="rounded-circle"><div class="media-body ml-4"><div class="d-flex align-items-center justify-content-between mb-1"><h6 class="mb-0">${report.name}</h6><small class="small font-weight-bold">${recentComment.sentDate} | ${recentComment.sentTime}</small></div><p class="font-italic mb-0 text-small">${recentComment.text}</p></div></div></span>`
    }).join('');
}

function makeMessagesMarkup(comments) {
    return comments.map((comment) => {
        if (comment.respondentId == null) {
            return `<div class="media w-50 mb-3" id=${comment.id}><img src="https://res.cloudinary.com/mhmd/image/upload/v1564960395/avatar_usae7z.svg" alt="user" width="50" class="rounded-circle"><div class="media-body ml-3"><div class="bg-light rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-muted">${comment.text}</p></div><p class="small text-muted">${comment.sentTime} | ${comment.sentDate}</p></div></div>`
        }
        return `<div class="media w-50 ml-auto mb-3" id=${comment.id}><div class="media-body"><div class="bg-primary rounded py-2 px-3 mb-2"><p class="text-small mb-0 text-white">${comment.text}</p></div><p class="small text-muted">${comment.sentTime} | ${comment.sentDate}</p></div></div>`
    }).join('');
}

function changeRecentMessage(comment) {
    $(`#${comment.reportId} .media-body small`).text(`${comment.sentTime} | ${comment.sentDate}`);
    $(`#${comment.reportId} .media-body p`).text(comment.text);
}

async function getCommentsByReportId() {
    var reportId = $(this).attr('id');
    $('.list-group-item').addClass('list-group-item-light');
    $(".list-group-item").removeClass("active text-white");
    $(this).removeClass('list-group-item-light');
    $(this).addClass('active text-white');

    const { data } = await axios.get(`/api/reports/GetCommentsByReportId/${reportId}`);
    console.log(data);
    let messagesMarkup;
    $('.messages-container').empty();
    if (data.length > 0) {
        messagesMarkup = makeMessagesMarkup(data);
    }
    $('.messages-container').append(messagesMarkup);
}