using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using HarmonyLib;
using NLog;
using NLog.Config;
using NLog.Targets;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace xxWoundXP
{
    public class WoundXpSubModule : MBSubModuleBase
    {

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            settings = new ModuleSettings();

            LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            FileTarget target = new FileTarget(settings.LogFileTarget)
            {
                FileName = settings.LogFilePath
            };
            loggingConfiguration.AddRule(LogLevel.Debug, LogLevel.Fatal, target, "*");
            LogManager.Configuration = loggingConfiguration;

            try
            {
                if (!File.Exists(settings.SettingsFilePath))
                {
                    SerializeSettings(settings.SettingsFilePath);
                }

                settings = DeserializeSettings(settings.SettingsFilePath);
                Log.Info("Module intialization | Settings initialized sucessfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to Serialize/Deserialize for " + settings.SettingsFilePath); ;
            }

            try
            {
                Harmony harmony = new Harmony("mod.bannerlord.woundxp");
                harmony.PatchAll();

                Log.Info("Module intialization | Harmony Initialized sucessfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Initialising Iron Will - Wound Experience:\n\n" + ex.Message);
            }
        }


        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (!(game.GameType is Campaign))
            {
                return;
            }

            InformationManager.DisplayMessage(new InformationMessage("Sucessfully Loaded Iron Will - Wound Experience", Colors.Green));
            Log.Info("Module intialization | Campaing Game Initialized.");
        }

        public void SerializeSettings(string path)
        {
            XmlSerializer s = new XmlSerializer(typeof(ModuleSettings));
            TextWriter writer = new StreamWriter(path);

            s.Serialize(writer, settings);
            writer.Close();
        }


        public ModuleSettings DeserializeSettings(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            XmlSerializer x = new XmlSerializer(typeof(ModuleSettings));
            ModuleSettings ms = (ModuleSettings)x.Deserialize(fs);

            return ms;
        }

        public static readonly NLog.Logger Log = LogManager.GetCurrentClassLogger();
        public static ModuleSettings settings;

    }

}
