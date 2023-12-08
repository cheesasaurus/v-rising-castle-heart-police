import MapPainter from './MapPainter.js';
import MapPainterConfig from './MapPainterConfig.js';

const backgroundImage = new Image();
await new Promise(resolve => backgroundImage.onload = resolve, backgroundImage.src = "./images/map-background.png");

const response = await fetch('./data/territories.json');
const territoryData = await response.json();

const mapPainter = new MapPainter(document.getElementById("canvas"), backgroundImage, territoryData);
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