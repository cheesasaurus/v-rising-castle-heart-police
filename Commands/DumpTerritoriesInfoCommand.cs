using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BepInEx;
using Bloodstone.API;
using ProjectM;
using ProjectM.CastleBuilding;
using ProjectM.Terrain;
using Unity.Collections;
using VampireCommandFramework;

namespace CastleHeartPolice.Commands;


public class DumpTerritoriesInfoCommand {

    private class Data {
        public Coord Min { get; set; } = new Coord(Int32.MaxValue, Int32.MaxValue);
        public Coord Max { get; set; } = new Coord(Int32.MinValue, Int32.MinValue);
        public List<CastleTerritoryData> Territories { get; set; } = new List<CastleTerritoryData>();
    }

    private class CastleTerritoryData {
        public int CastleTerritoryId { get; set; }
        public int BlockCount { get; set; }
        public Rectangle BoundingRectangle { get; set; }
    }

    private class Rectangle {
        public Coord Min { get; set; }
        public Coord Max { get; set; }
    }

    private class Coord {
        public int x { get; set; }
        public int y { get; set; }

        public Coord(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    [Command("dump-territories-info", shortHand:"dti", description: "dump territories info", adminOnly: true)]
    public void Execute(ChatCommandContext ctx) {
        ctx.Reply("dumping territory info");
        
        var bepinexPath = Path.GetFullPath(Path.Combine(Paths.ConfigPath, @"..\"));
        var dir = Path.Combine(bepinexPath, @"PluginDumps\CastleHeartPolice");
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "territories.json");
        ctx.Reply($"Dumping Territories Info to {path}");

        var dataToDump = GatherData();
        WriteJsonFile(dataToDump, path);
        
        ctx.Reply("dump complete");
    }

    private Data GatherData() {
        var data = new Data();
        var territories = new List<CastleTerritoryData>();

        var entityManager = VWorld.Server.EntityManager;
        var mapZoneCollectionSystem = VWorld.Server.GetExistingSystem<MapZoneCollectionSystem>();
        var mapZoneCollection = mapZoneCollectionSystem.GetMapZoneCollection();
        foreach (var spatialZone in mapZoneCollection._MapZoneLookup.GetValueArray(Allocator.Temp)) {
            if ((MapZoneFlags.CastleTerritory & spatialZone.ZoneFlags) == 0) {
                continue;
            }
            
            var blocks = entityManager.GetBuffer<CastleTerritoryBlocks>(spatialZone.ZoneEntity);
            var castleTerritory = entityManager.GetComponentData<CastleTerritory>(spatialZone.ZoneEntity);

            var min = spatialZone.WorldBounds.Min;
            if (min.x < data.Min.x) {
                data.Min.x = min.x;
            }
            if (min.y < data.Min.y) {
                data.Min.y = min.y;
            }

            var max = spatialZone.WorldBounds.Max;
            if (max.x > data.Max.x) {
                data.Max.x = max.x;
            }
            if (max.y > data.Max.y) {
                data.Max.y = max.y;
            }

            var territoryData = new CastleTerritoryData(){
                CastleTerritoryId = castleTerritory.CastleTerritoryIndex,
                BlockCount = blocks.Length,
                BoundingRectangle = new Rectangle() {
                    Min = new Coord(min.x, min.y),
                    Max = new Coord(max.x, max.y),
                },
            };
            data.Territories.Add(territoryData);
        }
        return data;
    }

    private void WriteJsonFile(Data dataToDump, string path) {
        using (StreamWriter outputFile = new StreamWriter(path)) {
            var options = new JsonSerializerOptions {
                WriteIndented = true,
            };
            outputFile.Write(JsonSerializer.Serialize(dataToDump, options));
        }
    }

}
