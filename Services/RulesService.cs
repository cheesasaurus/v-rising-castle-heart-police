
using System.Text;
using Bloodstone.API;
using CastleHeartPolice.CastleHeartScore;
using CastleHeartPolice.Models;
using CastleHeartPolice.Utils;
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
        var currentScore = 0;
        var teamHearts = CastleHeartUtil.FindCastleHeartsOfPlayerTeam(character);
        foreach (var heart in teamHearts) {
            currentScore += CastleHeartScoreStrategy.HeartScore(heart);
        }

        var territoryScore = CastleHeartScoreStrategy.TerritoryScore(territoryInfo);
        
        var result = CheckRuleResult.Allowed();
        if ((currentScore + territoryScore) > MaxCastleHeartScorePerClan) {
            var message = new StringBuilder();
            message.AppendLine($"Claiming this territory would put you and your clan over the allowed limit of {LabeledScore(MaxCastleHeartScorePerClan)}.");
            message.AppendLine($"Currently at:\t\t{LabeledScore(currentScore)}");
            message.AppendLine($"Territory value:\t{LabeledScore(territoryScore)}");
            result.AddViolation(message.ToString());
        }
        return result;
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

    private string LabeledScore(int score) {
        if (score == 1) {
            return $"{score} Point";
        }
        return $"{score} Points";
    }

}
