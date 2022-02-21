import express from 'express';
import { readFileSync, writeFileSync } from 'fs';

/* Rank file load */
const fileName = './ranks.json';
const jsonFile = readFileSync(fileName, 'utf8');
const jsonData = JSON.parse(jsonFile);
console.log(jsonData);

function saveRankDataToJson() {
    writeFileSync(fileName, JSON.stringify(jsonData));
}

/* Rank array control */
let isAppendingData = false;
function addToRankData(recursive) {
    if (inputQueue.length === 0) {
        return;
    } else if (!isAppendingData || recursive) {
        isAppendingData = true;
        let data = (inputQueue.splice(0, 1))[0];
        jsonData.ranks.push(data);
        jsonData.ranks.sort((a, b) => a.record - b.record);

        if (inputQueue.length === 0) {
            saveRankDataToJson();
        } else {
            addToRankData(true);
        }
        isAppendingData = false;
    }
}

/* REST server start */
const app = express();
const PORT = 5454;
const inputQueue = [];

app.use(express.json());

app.get("/rank", (req, res) => {
    res.send(jsonFile);
});

app.post("/rank", (req, res) => {
    console.log(req.body);
    try {
        inputQueue.push({
            timeStamp: Date.now(),
            name: req.body.name,
            stage: req.body.stage,
            record: req.body.record
        });
        addToRankData();

        res.status(200).send();
    } catch (e) {
        console.warn(e);
        res.status(400).send();
    }
    res.status(500).send();
});

function handleListening (){
    console.log(`Listening on: http://localhost:${PORT}`);
}
app.listen(PORT, handleListening);