import MapPainter from './MapPainter.js';
import MapPainterConfig from './MapPainterConfig.js';
import Alignment from './Alignment.js';

const backgroundImage = new Image();
await new Promise(resolve => backgroundImage.onload = resolve, backgroundImage.src = "./images/map-background.png");

const response = await fetch('./data/territories.json');
const territoryData = await response.json();

const alignment = new Alignment();
const canvas = document.getElementById("canvas");
const mapPainter = new MapPainter(canvas, backgroundImage, territoryData, alignment);
const config = new MapPainterConfig();

mapPainter.paint(config);

document.getElementById("showBoundingRectangles").addEventListener("change", (event) => {
    config.showBoundingRectangles = !!event.currentTarget.checked;
    mapPainter.paint(config);
});

document.getElementById("showTerritoryIds").addEventListener("change", (event) => {
    config.showTerritoryIds = !!event.currentTarget.checked;
    mapPainter.paint(config);
});


const territoryById = {};

const jsonOutputEl = document.getElementById("output-json");
let jsonOutput = "";
const updateJsonOutput = () => {
    const output = {};
    for (const [territoryId, territory] of Object.entries(territoryById)) {
        output[territoryId] = territory.Score;
    }
    jsonOutput = JSON.stringify(output, null, "  ");
    jsonOutputEl.value = jsonOutput;
};

const editScore = (territoryId) => {
    const territory = territoryById[territoryId];
    const message = "Set value of territory#" + territoryId;
    const newScore = parseInt(prompt(message, territory.Score));
    territory.Score = Number.isNaN(newScore) ? 69 : newScore;
    mapPainter.paint(config);
    updateJsonOutput();
}

const canvasContainer = document.getElementById('canvas-container');
canvasContainer.style.width = canvas.width + "px";
canvasContainer.style.height = canvas.height + "px";
const scaleDown = alignment.scaleDown;
for (const territory of territoryData.Territories) {
    const territoryId = territory.CastleTerritoryId;
    territoryById[territoryId] = territory;

    const min = territory.BoundingRectangle.Min;
    const max = territory.BoundingRectangle.Max;
    const width = (max.x - min.x) / scaleDown;
    const height = (max.y - min.y) / scaleDown;

    // the game y axis goes from bottom to top,
    // but the canvas y axis goes from top to bottom,
    const x = (min.x / scaleDown) + alignment.offsetX;
    const y = canvas.height - (max.y / scaleDown) + alignment.offsetY;

    // bounding rectangle
    const area = document.createElement('div');
    area.className = 'territory-area';
    area.style.top = y + "px";
    area.style.left = x + "px";
    area.style.width = width + "px";
    area.style.height = height + "px";
    area.dataset.territoryId = territoryId;
    area.onclick = () => editScore(territoryId);
    canvasContainer.appendChild(area);
}

document.getElementById("download-json").addEventListener('click', () => {
    const file = new File([jsonOutput], 'territoryScores.json', {
        type: 'text/plain',
    });
    const link = document.createElement('a')
    const url = URL.createObjectURL(file);

    link.href = url;
    link.download = file.name;
    //document.body.appendChild(link);
    link.click();

    //document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
});

updateJsonOutput();