using System;
using Bloodstone.API;
using CastleHeartPolice.Utils;
using Unity.Mathematics;
using Unity.Transforms;
using VampireCommandFramework;

namespace CastleHeartPolice.Commands;


public class TerritoryInfoCommand {

    [Command("territory-info", shortHand: "ti", description: "get information about the territory your character is in", adminOnly: false)]
    public void Execute(ChatCommandContext ctx) {
        var worldPos = WorldPositionOfPlayerCharacter(ctx);
        var blockCoords = TerritoryUtil.BlockCoordinatesFromWorldPosition(worldPos);

        ctx.Reply($"WorldPosition: {FormatFloat(worldPos.x)} {FormatFloat(worldPos.y)} {FormatFloat(worldPos.z)}");
        ctx.Reply($"BlockCoordinates: {blockCoords.x} {blockCoords.y}");
        // todo: zone id, etc
    }

    private static float3 WorldPositionOfPlayerCharacter(ChatCommandContext ctx) {
        var translationData = VWorld.Server.EntityManager.GetComponentData<Translation>(ctx.User.LocalCharacter._Entity);
        return translationData.Value;
    }

    private static string FormatFloat(float number) {
        return String.Format("{0:0.00}", number);
    }

}
