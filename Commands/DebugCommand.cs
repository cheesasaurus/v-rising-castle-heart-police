using VampireCommandFramework;

namespace CastleHeartPolice.Commands;


public class DebugCommand {

    [Command("debug", description: "do debug things", adminOnly: false)]
    public void Execute(ChatCommandContext ctx) {
        ctx.Reply("doing debug things");
        ctx.Reply(nameof(CastleHeartPolice.CastleHeartScore.Strategies.EveryHeartWorthOnePoint));
    }

}
