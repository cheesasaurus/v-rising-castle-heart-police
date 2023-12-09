using System.Collections.Generic;
using System.Text.Json;

namespace CastleHeartPolice.Config;


class TerritoryScoresConfig : AbstractJsonConfig {

    private Dictionary<int, int> ScoreByTerritoryId = new();

    public TerritoryScoresConfig(string filepath) : base(filepath) {

    }

    protected override void InitDefaults() {
        var territoryCount = 140;
        var defaultScore = 1;
        for (var id = 0; id < territoryCount; id++) {
            ScoreByTerritoryId.Add(id, defaultScore);
        }
    }

    public override string ToJson() {
        return JsonSerializer.Serialize(ScoreByTerritoryId);
    }

    public static TerritoryScoresConfig Init(string filename) {
        return Init<TerritoryScoresConfig>(filename);
    }

}
