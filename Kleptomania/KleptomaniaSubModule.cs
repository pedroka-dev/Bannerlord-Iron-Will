using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace xxKleptomania
{
    public class KleptomaniaSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (!(game.GameType is Campaign))
            {
                return;
            }

            try
            {
                CampaignGameStarter campaignGameStarter = (CampaignGameStarter)gameStarterObject;

                campaignGameStarter.AddBehavior(new StealSuppliesBehaviour());

                InformationManager.DisplayMessage(new InformationMessage("Sucessfully Loaded Iron Will - Kleptomania", Colors.Green));
                //Log.Info("Module intialization | Campaing Game Initialized.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Initialising Iron Will - Wound Experience:\n\n" + ex.Message);
            }
   
}
    }
}
