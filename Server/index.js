import express from 'express';
import { readFileSync } from 'fs';

/* Rank file load */
const jsonFile = readFileSync('./ranks.json', 'utf8');
const jsonData = JSON.parse(jsonFile);
console.log(jsonData);

/* REST server start */
const app = express();
const PORT = 5454;
const inputQueue = [];

app.get("/rank", (req, res) => {
    res.send(jsonFile);
});

app.post("/rank", (req, res) => {
    console.log(req.body);
    // inputQueue에 넣고, 차례대로 정리할 수 있도록
    res.status(200);
});

function handleListening (){
    console.log(`Listening on: http://localhost:${PORT}`);
}
app.listen(PORT, handleListening);