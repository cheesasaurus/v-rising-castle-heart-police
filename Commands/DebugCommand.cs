using System;
using System.Text;
using Bloodstone.API;
using CastleHeartPolice.Utils;
using ProjectM.Terrain;
using Unity.Mathematics;
using Unity.Transforms;
using VampireCommandFramework;

namespace CastleHeartPolice.Commands;


public class DebugCommand {

    [Command("debug", description: "do debug things", adminOnly: false)]
    public void Execute(ChatCommandContext ctx) {
        ctx.Reply("doing debug things");
        //ClanUtil.FindClan();
    }

}
