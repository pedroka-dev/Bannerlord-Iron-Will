using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HarmonyLib;
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
        public static int troopWoundXpValue = 20;
        public static int heroWoundXpValue = 20;
        public static bool debugInfo = false;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            //Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Message",
            //     new TextObject("Message", null), 9990,
            //     () => { InformationManager.DisplayMessage(new InformationMessage("WoundXp loaded sucessfully.")); },
            //false));
            
            try
            {
                Harmony harmony = new Harmony("mod.bannerlord.woundxp");
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Initialising Bannerlord Tweaks:\n\n" + ex.Message);
            }

            //InformationManager.DisplayMessage(new InformationMessage("WoundXp loaded sucessfully."));
        }
    }
}
