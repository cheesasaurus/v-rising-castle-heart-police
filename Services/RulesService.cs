
using Bloodstone.API;
using CastleHeartPolice.Models;
using CastleHeartPolice.Utils;
using ProjectM;
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
        /*
        return new CheckRuleResult()
            .AddViolation("bing")
            .AddViolation("bong")
            .AddViolation("bang");
        */
        return CheckRuleResult.Allowed();
    }

    public CheckRuleResult CheckRuleJoinClan(Entity character, Entity clan) {
        var clanHearts = ClanUtil.FindClanCastleHearts(clan);
        Plugin.Logger.LogMessage($"Found {clanHearts.Count} clan hearts");
        return new CheckRuleResult()
            .AddViolation("bing")
            .AddViolation("bong")
            .AddViolation("bang");
        //return CheckRuleResult.Allowed();
    }

    public CheckRuleResult CheckRuleClaimCastleHeart(Entity character, Entity heart) {
        return CheckRuleResult.Allowed();
    }

}
