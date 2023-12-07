using Bloodstone.API;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;

namespace CastleHeartPolice.Utils;

public static class ClanUtil {

    public static bool TryFindClan(NetworkId clanId, out Entity clan) {
        var entityManager = VWorld.Server.EntityManager;

        var query = entityManager.CreateEntityQuery(new EntityQueryDesc() {
            All = new ComponentType[] {
                ComponentType.ReadOnly<ClanTeam>()
            },
        });

        var entities = query.ToEntityArray(Allocator.Temp);
        foreach (var entity in entities) {
            var networkId = entityManager.GetComponentData<NetworkId>(entity);
            if (networkId.Equals(clanId)) {
                clan = entity;
                return true;
            }
        }
        clan = default;
        return false;
    }

}