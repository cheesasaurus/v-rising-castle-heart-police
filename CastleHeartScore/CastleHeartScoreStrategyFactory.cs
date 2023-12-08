using CastleHeartPolice.CastleHeartScore.Strategies;

namespace CastleHeartPolice.CastleHeartScore;

public class CastleHeartScoreStrategyFactory {

    public ICastleHeartScoreStrategy Strategy(string strategyString) {
        switch (strategyString) {
            case nameof(EveryHeartWorthOnePoint):
                return new EveryHeartWorthOnePoint();
            default:
                return new EveryHeartWorthOnePoint();
        }
    }

}
