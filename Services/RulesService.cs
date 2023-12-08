
using Bloodstone.API;
using CastleHeartPolice.CastleHeartScore;
using CastleHeartPolice.Models;
using CastleHeartPolice.Utils;
using ProjectM;
using Unity.Entities;

namespace CastleHeartPolice.Services;

public class RulesService {

    public static RulesService Instance {get; private set; }

    public static void InitInstance() {
        var strategyFactory = new CastleHeartScoreStrategyFactory();
        var maxCastleHeartScorePerClan = CastleHeartPoliceConfig.MaxCastleHeartScorePerClan.Value;
        var scoreStrategy = strategyFactory.Strategy(CastleHeartPoliceConfig.CastleHeartScoreStrategy.Value);
        Instance = new RulesService(maxCastleHeartScorePerClan, scoreStrategy);
    }

    private int MaxCastleHeartScorePerClan;
    private ICastleHeartScoreStrategy CastleHeartScoreStrategy;

    public RulesService(int maxCastleHeartScorePerClan, ICastleHeartScoreStrategy castleHeartScoreStrategy) {
        MaxCastleHeartScorePerClan = maxCastleHeartScorePerClan;
        CastleHeartScoreStrategy = castleHeartScoreStrategy;
    }

    public CheckRuleResult CheckRulePlaceCastleHeartInTerritory(Entity character, CastleTerritoryInfo territoryInfo) {
        var teamHearts = CastleHeartUtil.FindCastleHeartsOfPlayerTeam(character);
        var score = 0;
        foreach (var heart in teamHearts) {
            score += CastleHeartScoreStrategy.HeartScore(heart);
        }
        Plugin.Logger.LogMessage($"Found {teamHearts.Count} team hearts worth a total of {score} points");
        // todo: score territory too
        
        /*
        return new CheckRuleResult()
            .AddViolation("bing")
            .AddViolation("bong")
            .AddViolation("bang");
        */
        return CheckRuleResult.Allowed();
    }

    public CheckRuleResult CheckRuleJoinClan(Entity character, Entity clan) {
        var playerHearts = CastleHeartUtil.FindCastleHeartsOfPlayer(character);
        Plugin.Logger.LogMessage($"Found {playerHearts.Count} player hearts");
        var clanHearts = CastleHeartUtil.FindCastleHeartsOfClan(clan);
        Plugin.Logger.LogMessage($"Found {clanHearts.Count} clan hearts");
        /*
        return new CheckRuleResult()
            .AddViolation("bing")
            .AddViolation("bong")
            .AddViolation("bang");
        */
        return CheckRuleResult.Allowed();
    }

    public CheckRuleResult CheckRuleClaimCastleHeart(Entity character, Entity heart) {
        return CheckRuleResult.Allowed();
    }

}
