using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace MassiveBattles
{
    public sealed class SubModule : MBSubModuleBase
    {
        private readonly List<string> _failedPatches = new List<string>();
        private bool _reported;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            var harmony = new Harmony("mod.massivebattles");
            // Patch each class on its own so one broken patch (e.g. after a game update) doesn't disable the rest.
            foreach (Type type in typeof(SubModule).Assembly.GetTypes().Where(t => t.IsDefined(typeof(HarmonyPatch), false)))
            {
                try
                {
                    harmony.CreateClassProcessor(type).Patch();
                }
                catch (Exception e)
                {
                    _failedPatches.Add(type.Name);
                    TaleWorlds.Library.Debug.Print("[Massive Battles] Patch " + type.Name + " failed: " + e);
                }
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            if (_reported)
                return;
            _reported = true;
            if (_failedPatches.Count > 0)
                Log.Info("some patches failed to apply: " + string.Join(", ", _failedPatches) + ". Check rgl_log for details.");
            else
                Log.Info("loaded");
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);
            if (mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>() != null)
                mission.AddMissionBehavior(new MassiveBattlesMissionLogic());
        }
    }
}
