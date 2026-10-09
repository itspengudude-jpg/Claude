using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace MassiveBattles
{
    public sealed class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => "MassiveBattles_v1";
        public override string DisplayName => "Massive Battles";
        public override string FolderName => "MassiveBattles";
        public override string FormatType => "json2";

        /// <summary>MCM instance, or defaults if MCM failed to load.</summary>
        public static Settings Current => Instance ?? Fallback;
        private static readonly Settings Fallback = new Settings();

        // ---- Battle size (troops alive at once). 0 = keep the game's own setting. ----

        [SettingPropertyInteger("Field battle size", 0, 2000, "0", Order = 0, RequireRestart = false,
            HintText = "Troops alive at once in field battles. 0 = use the game's setting. Automatically capped so troops + horses never exceed the engine's agent limit.")]
        [SettingPropertyGroup("Battle Size", GroupOrder = 0)]
        public int FieldBattleSize { get; set; } = 1400;

        [SettingPropertyInteger("Siege battle size", 0, 2000, "0", Order = 1, RequireRestart = false,
            HintText = "Troops alive at once in sieges. Sieges are much heavier on pathfinding; raise carefully. 0 = use the game's setting.")]
        [SettingPropertyGroup("Battle Size", GroupOrder = 0)]
        public int SiegeBattleSize { get; set; } = 900;

        [SettingPropertyInteger("Sally-out battle size", 0, 2000, "0", Order = 2, RequireRestart = false,
            HintText = "Troops alive at once in sally-out battles. 0 = use the game's setting.")]
        [SettingPropertyGroup("Battle Size", GroupOrder = 0)]
        public int SallyOutBattleSize { get; set; } = 900;

        [SettingPropertyInteger("Naval battle size", 0, 2000, "0", Order = 3, RequireRestart = false,
            HintText = "Troops alive at once in War Sails naval battles. 0 = use the game's setting.")]
        [SettingPropertyGroup("Battle Size", GroupOrder = 0)]
        public int NavalBattleSize { get; set; } = 0;

        // ---- Engine safety ----

        [SettingPropertyInteger("Horse budget", 0, 1500, "0", Order = 0, RequireRestart = false,
            HintText = "Max horses alive at once. Every horse is an engine agent; once the budget is used, further cavalry spawn on foot. Lords and the player always keep their horse. Battle size is capped to (agent limit - horse budget - safety margin).")]
        [SettingPropertyGroup("Engine Safety", GroupOrder = 1)]
        public int HorseBudget { get; set; } = 500;

        [SettingPropertyInteger("Agent safety margin", 50, 500, "0", Order = 1, RequireRestart = false,
            HintText = "Agent slots kept free below the engine limit. If live agents get within this margin, reinforcements pause until it frees up.")]
        [SettingPropertyGroup("Engine Safety", GroupOrder = 1)]
        public int AgentSafetyMargin { get; set; } = 100;

        // ---- Performance ----

        [SettingPropertyInteger("Max corpses", 0, 1000, "0", Order = 0, RequireRestart = false,
            HintText = "Max corpses kept on the field before the oldest fade out. Lower = faster in long battles. 0 = game default.")]
        [SettingPropertyGroup("Performance", GroupOrder = 2)]
        public int MaxCorpses { get; set; } = 150;

        [SettingPropertyInteger("Corpse fade time (seconds)", 0, 600, "0", Order = 1, RequireRestart = false,
            HintText = "Seconds before a corpse fades out. 0 = game default.")]
        [SettingPropertyGroup("Performance", GroupOrder = 2)]
        public int CorpseFadeSeconds { get; set; } = 30;

        // ---- Debug ----

        [SettingPropertyBool("Show battle info", Order = 0, RequireRestart = false,
            HintText = "Print the effective battle size and agent budget when a battle starts, and warn when reinforcements are paused.")]
        [SettingPropertyGroup("Debug", GroupOrder = 3)]
        public bool ShowBattleInfo { get; set; } = true;
    }
}
