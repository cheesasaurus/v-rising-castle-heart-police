const response = await fetch('./data/territories.json');
const data = await response.json();

const backgroundImage = new Image();
await new Promise(resolve => backgroundImage.onload = resolve, backgroundImage.src = "./images/map-background.png");


const showBoundingRectangle = true;
const showTerritoryId = true;

const scaleDown = 2.13;
const offsetX = -452;
const offsetY = 166;

const canvas = document.getElementById("canvas");

const ctx = canvas.getContext("2d");
ctx.canvas.width = (data.Max.x / scaleDown) - offsetX;
ctx.canvas.height = (data.Max.y / scaleDown);

ctx.drawImage(backgroundImage, 0, 0);

for (const territory of data.Territories) {
    const min = territory.BoundingRectangle.Min;
    const max = territory.BoundingRectangle.Max;
    const width = (max.x - min.x) / scaleDown;
    const height = (max.y - min.y) / scaleDown;

    // the game y axis goes from bottom to top,
    // but the canvas y axis goes from top to bottom,
    const x = (min.x / scaleDown) + offsetX;
    const y = ctx.canvas.height - (max.y / scaleDown) + offsetY;

    // bounding rectangle
    if (showBoundingRectangle) {
        ctx.strokeStyle = "magenta";
        ctx.lineWidth = 1;
        ctx.beginPath();
        ctx.rect(x, y, width, height);
        ctx.stroke();
    }

    const centerX = x + (width / 2);
    const centerY = y + (height / 2);

    // score
    const scoreText = territory.Score;
    const scoreY = centerY - 5;
    ctx.font = "bold 26px Roboto";
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillStyle = "white";
    ctx.strokeStyle = "black";
    ctx.lineWidth = 2;
    ctx.strokeText(scoreText, centerX, scoreY);
    ctx.fillText(scoreText, centerX, scoreY);

    // territory id
    if (showTerritoryId) {
        const idText = '#' + territory.CastleTerritoryId;
        const idY = centerY + 15;
        ctx.font = "12px Arial";
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.fillStyle = "white";
        ctx.strokeStyle = "black";
        ctx.lineWidth = 1;
        ctx.strokeText(idText, centerX, idY);
        ctx.fillText(idText, centerX, idY);
    }
    
}
