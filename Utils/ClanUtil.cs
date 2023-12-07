using System.Collections.Generic;
using Bloodstone.API;
using ProjectM;
using ProjectM.CastleBuilding;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;

namespace CastleHeartPolice.Utils;

public static class ClanUtil {

    public static bool TryFindClan(NetworkId clanId, out Entity clanTeam) {
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
                clanTeam = entity;
                return true;
            }
        }
        clanTeam = default;
        return false;
    }

    public static List<Entity> FindClanCastleHearts(Entity clanTeam) {
        var entityManager = VWorld.Server.EntityManager;
        var clanTeamData = entityManager.GetComponentData<ClanTeam>(clanTeam);
        var clanHearts = new List<Entity>();

        var query = entityManager.CreateEntityQuery(new EntityQueryDesc() {
            All = new ComponentType[] {
                ComponentType.ReadOnly<Pylonstation>(),
                ComponentType.ReadOnly<CastleHeart>(),
                ComponentType.ReadOnly<Team>(),
            },
        });
        var heartEntities = query.ToEntityArray(Allocator.Temp);
        foreach (var heartEntity in heartEntities) {
            var heartTeam = entityManager.GetComponentData<Team>(heartEntity);
            if (clanTeamData.TeamValue.Equals(heartTeam.Value)) {
                clanHearts.Add(heartEntity);
            }
        }
        return clanHearts;
    }

}