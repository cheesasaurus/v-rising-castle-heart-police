const response = await fetch('./data/territories.json');
const data = await response.json();

const backgroundImage = new Image();
await new Promise(resolve => backgroundImage.onload = resolve, backgroundImage.src = "./images/map-background.png");


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

    ctx.strokeStyle = "magenta"
    ctx.beginPath();
    ctx.rect(x, y, width, height);
    ctx.stroke();

    const centerX = x + (width / 2);
    const centerY = y + (height / 2);
    const text = territory.CastleTerritoryId;

    ctx.font = "20px Arial";
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillStyle = "white"
    ctx.strokeStyle = "black"
    ctx.strokeText(text, centerX, centerY);
    ctx.fillText(text, centerX, centerY);
}
