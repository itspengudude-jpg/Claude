using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MassiveBattles
{
    // Every horse is an engine agent. Once the horse budget is used up, further
    // cavalry spawn on foot so troops + horses stay under the agent limit.
    // The player and heroes (lords, companions) always keep their horse.
    [HarmonyPatch(typeof(Mission), nameof(Mission.SpawnTroop))]
    internal static class HorseBudgetPatch
    {
        private static void Prefix(IAgentOriginBase troopOrigin, ref bool spawnWithHorse)
        {
            if (!spawnWithHorse)
                return;
            MassiveBattlesMissionLogic logic = MassiveBattlesMissionLogic.Active;
            if (logic == null)
                return;
            BasicCharacterObject troop = troopOrigin?.Troop;
            if (troop != null && (troop.IsPlayerCharacter || troop.IsHero))
            {
                logic.NoteMountSpawned();
                return;
            }
            if (logic.MountCount >= Settings.Current.HorseBudget)
            {
                spawnWithHorse = false;
                logic.NoteDismounted();
            }
            else
            {
                logic.NoteMountSpawned();
            }
        }
    }
}
