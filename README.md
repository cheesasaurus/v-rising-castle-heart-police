# Castle Heart Police

Restricts how many / where castle hearts can be placed, on a per-clan basis.


## The system

Definitions
- let MaxCastleHeartScorePerClan be a number, specified by the configuration of the server
- let CastleHeartScoreStrategy be a selectable strategy, specified by the configuration of the server
  - e.g. EveryHeartWorthOnePoint (probably the simplest thing to start with)
  - e.g. EachHeartValuedByTerritory (with the value of each territory configured in a json file)
- let the value of any castle heart be determined by the selected CastleHeartScoreStrategy

Rules
- castle heart placement:
  - let CastleHeartScore be the value of hearts owned by the player and all other players in their current clan
  - if CastleHeartScore exceeds MaxCastleHeartScorePerClan, the player cannot place the heart
- joining a clan:
  - let CastleHeartScore be the value of hearts owned by the player and all players in the clan they would join
  - if CastleHeartScore exceeds MaxCastleHeartScorePerClan, the player cannot join the clan
- claiming an exposed heart
  - let CastleHeartScore be the value of the exposed heart, hearts owned by the player and all other players in their current clan
  - if CastleHeartScore exceeds MaxCastleHeartScorePerClan, the player cannot claim the exposed heart


## Config

todo