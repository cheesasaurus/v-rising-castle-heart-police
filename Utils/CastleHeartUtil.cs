using System.Collections.Generic;
using Bloodstone.API;
using ProjectM;
using ProjectM.CastleBuilding;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;

namespace CastleHeartPolice.Utils;

public static class CastleHeartUtil {

    

    public static List<Entity> FindCastleHeartOfClan(Entity clanTeam) {
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