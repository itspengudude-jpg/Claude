using System;
using TaleWorlds.Library;

namespace MassiveBattles
{
    internal static class Log
    {
        private static readonly Color InfoColor = Color.FromUint(0xFFE0B040);
        private static readonly Color ErrorColor = Color.FromUint(0xFFFF5050);

        public static void Info(string text) =>
            InformationManager.DisplayMessage(new InformationMessage("[Massive Battles] " + text, InfoColor));

        public static void Error(string what, Exception e)
        {
            Debug.Print("[Massive Battles] Error " + what + ": " + e);
            InformationManager.DisplayMessage(new InformationMessage("[Massive Battles] Error " + what + ": " + e.Message, ErrorColor));
        }
    }
}
