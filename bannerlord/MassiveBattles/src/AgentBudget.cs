using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MassiveBattles
{
    /// <summary>Works out how many troops fit under the engine's agent limit.</summary>
    internal static class AgentBudget
    {
        /// <summary>Engine agent limit (2048 in current builds), read from the game.</summary>
        public static int MaxAgents => DefaultBattleMissionAgentSpawnLogic.MaxNumberOfAgentsForMission;

        /// <summary>Largest troop count that leaves room for the horse budget and the safety margin.</summary>
        public static int SafeTroopCap
        {
            get
            {
                Settings s = Settings.Current;
                return MathF.Max(200, MaxAgents - s.HorseBudget - s.AgentSafetyMargin);
            }
        }

        /// <summary>Applies our setting for one battle type; 0 keeps the game's value.</summary>
        public static int Resolve(int vanilla, int configured)
        {
            if (configured <= 0)
                return vanilla;
            return MathF.Min(configured, SafeTroopCap);
        }
    }
}
