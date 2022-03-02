// console.log("hello world!");
const elementId = [
    "slimeRanks",
    "hadesRanks",
    "jungleRanks"
]
let subscribedNumber = -1;

window.onbeforeunload = unsubscribe;

$(function() {
    subscribe();
});

function unsubscribe() {
    // 랭크 가져오는걸 clearInterval
    if (subscribedNumber > 0) {
        clearInterval(subscribedNumber);
        subscribedNumber = -1;
    }
}

function subscribe() {
    // 주기적으로 랭크를 가져오기 setInterval
    if (subscribedNumber <= 0) {
        subscribedNumber = setInterval(getRanks, 1000);
    }
}

function getRanks() {
    $.ajax({
        url: "/rank",
        method: "GET",
        dataType: "json"
    })
    .done(json => {
        separateByStage(json);
    })
    .fail((jqXHR, textStatus) => {
        alert("순위를 가져오는데 실패했습니다.");
        console.error(textStatus);
    });
}

function separateByStage(data) {
    let separatedRanks = {};
    let stageTitles = [];
    data.ranks.forEach(e => {
        if (!separatedRanks[e.stage]) {
            separatedRanks[e.stage] = [];
            stageTitles.push(e.stage);
        }
        separatedRanks[e.stage].push(e);
    });

    stageTitles.forEach((e, i) => {
        applyToElement(separatedRanks[e], i);
    });
}

function applyToElement(data, stage) {
    console.log(data, stage);
    let newString = "";
    
    data?.forEach(e => {
        newString += `${e.name} : ${e.record / 1000}<br>`;
    });

    $("#" + elementId[stage])
    .html(newString);
}
