
using CastleHeartPolice.Models;
using Unity.Entities;

namespace CastleHeartPolice.Services;

public class RulesService {

    public static RulesService Instance {get; private set; }

    public static void InitInstance() {
        Instance = new RulesService();
    }

    public RulesService() {
        // todo: feed in CastleHeartScoreStrategy
    }

    public CheckRuleResult CheckRulePlaceCastleHeartInTerritory(Entity character, CastleTerritoryInfo territoryInfo) {
        return new CheckRuleResult()
            .AddViolation("bing")
            .AddViolation("bong")
            .AddViolation("bang");
        //return CheckRuleResult.Allowed();
    }

    public CheckRuleResult CheckRuleJoinClan(Entity character) { // todo: clan param
        return CheckRuleResult.Allowed();
    }

    public CheckRuleResult CheckRuleClaimCastleHeart(Entity character, Entity heart) {
        return CheckRuleResult.Allowed();
    }

}
