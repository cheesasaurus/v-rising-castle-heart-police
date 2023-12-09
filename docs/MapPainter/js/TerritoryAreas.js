export default class TerritoryAreas {
    _scoreUpdated;
    territoryLookup;
    alignment;

    constructor(territoryLookup, alignment) {
        this.territoryLookup = territoryLookup;
        this.alignment = alignment;
    }

    init() {
        const alignment = this.alignment;
        const canvasContainer = document.getElementById('canvas-container');
        canvasContainer.style.width = canvas.width + "px";
        canvasContainer.style.height = canvas.height + "px";
        const scaleDown = alignment.scaleDown;
        for (const territory of Object.values(this.territoryLookup)) {
            const territoryId = territory.CastleTerritoryId;

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
            area.onclick = () => this.promptEditScore(territoryId);
            canvasContainer.appendChild(area);
        }
    }

    promptEditScore(territoryId) {
        const territory = this.territoryLookup[territoryId];
        const message = "Set value of territory#" + territoryId;
        const newScore = parseInt(prompt(message, territory.Score));
        if (Number.isNaN(newScore)) {
            return;
        }
        territory.Score = newScore;
        if (typeof this._scoreUpdated === 'function') {
            this._scoreUpdated(territoryId, newScore);
        }
    }

    onScoreEdited(callback) {
        this._scoreUpdated = callback;
    }

}
