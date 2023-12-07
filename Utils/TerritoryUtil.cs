using Bloodstone.API;
using CastleHeartPolice.Models;
using Il2CppInterop.Runtime;
using ProjectM;
using ProjectM.CastleBuilding;
using ProjectM.Terrain;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CastleHeartPolice.Utils;

public static class TerritoryUtil {

    // One unit on the block grid is equivalent to 5 units on the world grid
    private static int BlockSize = 5;

    // I have no idea why, but the block coordinates origin and world coordinates origin don't exactly line up
    private static int2 BlockOffsetFromWorld = new int2(639, 639);

    public static int2 BlockCoordinatesFromWorldPosition(float3 worldPos) {
        var intWorldPos = new int2((int)worldPos.x, (int)worldPos.z);
        return (intWorldPos / BlockSize) + BlockOffsetFromWorld;
    }

    public static bool TryFindTerritoryContaining(float3 worldPos, out TerritoryInfo territoryInfo) {
        var blockCoords = BlockCoordinatesFromWorldPosition(worldPos);
        float2 worldPos2 = worldPos.xz;
        var entityManager = VWorld.Server.EntityManager;
        
        var mapZoneCollectionSystem = VWorld.Server.GetExistingSystem<MapZoneCollectionSystem>();
        var mapZoneCollection = mapZoneCollectionSystem.GetMapZoneCollection();
        foreach (var spatialZone in mapZoneCollection._MapZoneLookup.GetValueArray(Allocator.Temp)) {
            if ((MapZoneFlags.CastleTerritory & spatialZone.ZoneFlags) == 0) {
                // not a castle territory
                continue;
            }

            // rough check (bounding rectangle, sometimes nearby territories' rectangles overlap)
            if (!spatialZone.WorldBounds.Contains(worldPos2)) {
                continue;
            }
            
            // detailed check (all the blocks where a castle floor could be placed. never overlaps with another territory)
            var blocks = entityManager.GetBuffer<CastleTerritoryBlocks>(spatialZone.ZoneEntity);
            foreach (var block in blocks) {
                if (block.BlockCoordinate.Equals(blockCoords)) {
                    territoryInfo = new TerritoryInfo() {
                        ZoneId = spatialZone.ZoneId,
                        ZoneEntity = spatialZone.ZoneEntity,
                        BlockCount = blocks.Length
                    };
                    return true;
                }
            }
        }
        territoryInfo = default;
        return false;
    }

}
