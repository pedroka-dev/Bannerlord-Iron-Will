using System;
using System.Windows;
using HarmonyLib;
using NLog;
using NLog.Config;
using NLog.Targets;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.Core;
using TaleWorlds.GauntletUI.PrefabSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace xxWoundXP
{
    public class WoundXpSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            FileTarget target = new FileTarget(LogFileTarget)
            {
                FileName = LogFilePath
            };
            loggingConfiguration.AddRule(LogLevel.Debug, LogLevel.Fatal, target, "*");
            LogManager.Configuration = loggingConfiguration;

            try
            {
                Harmony harmony = new Harmony("mod.bannerlord.woundxp");
                harmony.PatchAll();
                Log.Info("Harmony Initialized sucessfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Initialising Iron Will - Wound Experience:\n\n" + ex.Message);
                Log.Error(ex, "Failed to initialize Harmony.");
            }

            
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (!(game.GameType is Campaign))
            {
                return;
            }

            InformationManager.DisplayMessage(new InformationMessage("Sucessfully Loaded Iron Will - Wound Experience", Colors.Green));
            Log.Info("Campaing Game Initialized.");
        }

        public static readonly string LogFileTarget = "WoundExperienceLogFile";
        public static readonly string LogFilePath = "WoundExperienceLog.txt";
        
        public static readonly NLog.Logger Log = LogManager.GetCurrentClassLogger();

        public static ModuleSettings settings = new ModuleSettings();
    }

}
