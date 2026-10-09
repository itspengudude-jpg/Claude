using System;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MassiveBattles
{
    // The options slider stops at 1000. These postfixes run last (Priority.Last)
    // so they win over other mods that patch the same methods.

    [HarmonyPatch(typeof(BannerlordConfig), nameof(BannerlordConfig.GetRealBattleSize))]
    internal static class FieldBattleSizePatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(ref int __result) =>
            __result = AgentBudget.Resolve(__result, Settings.Current.FieldBattleSize);
    }

    [HarmonyPatch(typeof(BannerlordConfig), nameof(BannerlordConfig.GetRealBattleSizeForSiege))]
    internal static class SiegeBattleSizePatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(ref int __result) =>
            __result = AgentBudget.Resolve(__result, Settings.Current.SiegeBattleSize);
    }

    [HarmonyPatch(typeof(BannerlordConfig), nameof(BannerlordConfig.GetRealBattleSizeForSallyOut))]
    internal static class SallyOutBattleSizePatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(ref int __result) =>
            __result = AgentBudget.Resolve(__result, Settings.Current.SallyOutBattleSize);
    }

    [HarmonyPatch(typeof(BannerlordConfig), nameof(BannerlordConfig.GetRealBattleSizeForNaval))]
    internal static class NavalBattleSizePatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(ref int __result) =>
            __result = AgentBudget.Resolve(__result, Settings.Current.NavalBattleSize);
    }

    // Vanilla caps troops at half the agent limit (assumes every troop might bring a horse).
    // We budget horses explicitly instead, so the troop cap can be raised.
    [HarmonyPatch(typeof(DefaultBattleMissionAgentSpawnLogic), nameof(DefaultBattleMissionAgentSpawnLogic.MaxNumberOfTroopsForMission), MethodType.Getter)]
    internal static class MaxTroopsPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(ref int __result) =>
            __result = MathF.Max(__result, AgentBudget.SafeTroopCap);
    }

    // Small getters like the one above can be inlined by the JIT, which makes their
    // patch a no-op. As a backstop, set the spawn logic's battle size directly.
    [HarmonyPatch(typeof(DefaultBattleMissionAgentSpawnLogic), MethodType.Constructor,
        new[] { typeof(IMissionTroopSupplier[]), typeof(BattleSideEnum), typeof(Mission.BattleSizeType) })]
    internal static class SpawnLogicCtorPatch
    {
        private static readonly AccessTools.FieldRef<DefaultBattleMissionAgentSpawnLogic, int> BattleSizeRef =
            AccessTools.FieldRefAccess<DefaultBattleMissionAgentSpawnLogic, int>("_battleSize");

        [HarmonyPriority(Priority.Last)]
        private static void Postfix(DefaultBattleMissionAgentSpawnLogic __instance, Mission.BattleSizeType battleSizeType)
        {
            try
            {
                Settings s = Settings.Current;
                int configured = battleSizeType switch
                {
                    Mission.BattleSizeType.Siege => s.SiegeBattleSize,
                    Mission.BattleSizeType.SallyOut => s.SallyOutBattleSize,
                    _ => Mission.Current != null && Mission.Current.IsNavalBattle ? s.NavalBattleSize : s.FieldBattleSize,
                };
                if (configured > 0)
                    BattleSizeRef(__instance) = AgentBudget.Resolve(BattleSizeRef(__instance), configured);
            }
            catch (Exception e)
            {
                Log.Error("setting battle size", e);
            }
        }
    }
}
