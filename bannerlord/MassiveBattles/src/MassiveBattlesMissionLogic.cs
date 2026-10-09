using System;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace MassiveBattles
{
    /// <summary>
    /// Added to every battle that uses the standard spawn logic. Tracks horses for the
    /// horse budget, pauses reinforcements near the engine agent limit, and applies corpse limits.
    /// </summary>
    internal sealed class MassiveBattlesMissionLogic : MissionLogic
    {
        private const float ScanInterval = 0.5f;

        private static readonly AccessTools.FieldRef<DefaultBattleMissionAgentSpawnLogic, bool> ReinforcementsEnabledRef = TryFieldRef();

        public static MassiveBattlesMissionLogic Active { get; private set; }

        public int MountCount { get; private set; }

        private DefaultBattleMissionAgentSpawnLogic _spawnLogic;
        private float _sinceScan;
        private bool _pausedReinforcements;
        private int _dismounted;
        private int _peakAgents;

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            Active = this;
            _spawnLogic = Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
        }

        public override void AfterStart()
        {
            base.AfterStart();
            Settings s = Settings.Current;
            try
            {
                if (s.MaxCorpses > 0)
                    Mission.SetOverrideCorpseCount(s.MaxCorpses);
                if (s.CorpseFadeSeconds > 0)
                    Mission.SetMissionCorpseFadeOutTimeInSeconds(s.CorpseFadeSeconds);
            }
            catch (Exception e)
            {
                Log.Error("applying corpse limits", e);
            }

            if (s.ShowBattleInfo && _spawnLogic != null)
            {
                Log.Info($"battle size {_spawnLogic.BattleSize} (safe cap {AgentBudget.SafeTroopCap}), " +
                         $"horse budget {s.HorseBudget}, agent limit {AgentBudget.MaxAgents}");
            }
        }

        public override void OnMissionTick(float dt)
        {
            _sinceScan += dt;
            if (_sinceScan < ScanInterval)
                return;
            _sinceScan = 0f;

            int live = 0, mounts = 0;
            foreach (Agent agent in Mission.Agents)
            {
                live++;
                if (agent.IsMount)
                    mounts++;
            }
            MountCount = mounts;
            if (live > _peakAgents)
                _peakAgents = live;

            GuardAgentLimit(live);
        }

        // Pause reinforcements when live agents approach the engine limit, resume once there is room.
        // We only ever re-enable reinforcements that we paused ourselves.
        private void GuardAgentLimit(int live)
        {
            if (_spawnLogic == null || ReinforcementsEnabledRef == null)
                return;
            Settings s = Settings.Current;
            int max = AgentBudget.MaxAgents;
            bool enabled = ReinforcementsEnabledRef(_spawnLogic);

            if (!_pausedReinforcements && enabled && live >= max - s.AgentSafetyMargin)
            {
                _spawnLogic.SetReinforcementsSpawnEnabled(false, resetTimers: false);
                _pausedReinforcements = true;
                if (s.ShowBattleInfo)
                    Log.Info($"{live} agents alive, near the limit of {max}: reinforcements paused");
            }
            else if (_pausedReinforcements && live < max - 2 * s.AgentSafetyMargin)
            {
                if (!enabled)
                    _spawnLogic.SetReinforcementsSpawnEnabled(true, resetTimers: false);
                _pausedReinforcements = false;
                if (s.ShowBattleInfo)
                    Log.Info("reinforcements resumed");
            }
        }

        public void NoteMountSpawned() => MountCount++;

        public void NoteDismounted() => _dismounted++;

        protected override void OnEndMission()
        {
            if (Settings.Current.ShowBattleInfo)
                Log.Info($"peak agents {_peakAgents}/{AgentBudget.MaxAgents}, {_dismounted} cavalry spawned on foot");
            if (Active == this)
                Active = null;
            base.OnEndMission();
        }

        private static AccessTools.FieldRef<DefaultBattleMissionAgentSpawnLogic, bool> TryFieldRef()
        {
            try
            {
                return AccessTools.FieldRefAccess<DefaultBattleMissionAgentSpawnLogic, bool>("_reinforcementSpawnEnabled");
            }
            catch (Exception e)
            {
                Log.Error("finding reinforcement switch (agent-limit guard disabled)", e);
                return null;
            }
        }
    }
}
