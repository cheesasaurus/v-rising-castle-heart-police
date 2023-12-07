using Bloodstone.API;
using CastleHeartPolice.Prefabs;
using CastleHeartPolice.Utils;
using HarmonyLib;
using ProjectM;
using ProjectM.Shared;
using Unity.Collections;
using Unity.Entities;

namespace CastleHeartPolice.Hooks;


[HarmonyPatch(typeof(PlaceTileModelSystem), nameof(PlaceTileModelSystem.OnUpdate))]
public static class CastleHeartWillBePlacedHook {

    public static void Prefix(PlaceTileModelSystem __instance) {
        var jobs = __instance._BuildTileQuery.ToEntityArray(Allocator.Temp);
        foreach (var job in jobs) {
            if (IsCastleHeart(job)) {
                HandleCastleHeartWillBePlaced(job);
            }
        }
    }
    
    private static bool IsCastleHeart(Entity job) {
        var entityManager = VWorld.Server.EntityManager;
        var buildTileModelData = entityManager.GetComponentData<BuildTileModelEvent>(job);
        return buildTileModelData.PrefabGuid.Equals(TileModelPrefabs.TM_BloodFountain_Pylon_Station);
    }

    private static void HandleCastleHeartWillBePlaced(Entity job) {
        var entityManager = VWorld.Server.EntityManager;
        var buildTileModelData = entityManager.GetComponentData<BuildTileModelEvent>(job);
        var worldPos = buildTileModelData.SpawnTranslation.Value;
        var foundTerritory = CastleTerritoryUtil.TryFindTerritoryContaining(worldPos, out var territoryInfo);
        if (!foundTerritory) {
            return;
        }
        Plugin.Logger.LogMessage($"Castle Heart about to be placed in territory#{territoryInfo.TerritoryId}");
        // todo: abort job if placement disallowed
    }



    private static void AbortJob(Entity entity) {
        DestroyUtility.CreateDestroyEvent(VWorld.Server.EntityManager, entity, DestroyReason.Default, DestroyDebugReason.ByScript);
    }

}
