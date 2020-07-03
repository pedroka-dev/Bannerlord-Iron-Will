using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Core;

namespace xxKleptomania
{
    class StealSuppliesBehaviour : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
        }

        public override void SyncData(IDataStore dataStore)
        {
           
        }

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            this.AddStealMenu(campaignGameStarter);
        }

        private void AddStealMenu(CampaignGameStarter campaignGameStarter)      //MUITO GRANDE. dividir em dois metodos
        {
            try
            {
                //Village
                campaignGameStarter.AddGameMenu(VillageStealMenu, VillageStealMenuMsg, null, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("village", VillageStealMenu, "Steal supplies from the peasants", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.Continue; // ???
                return true;
                }, delegate (MenuCallbackArgs args)
                {
                    GameMenu.SwitchToMenu(VillageStealMenu);
                }, false, 5, false);


                campaignGameStarter.AddGameMenuOption(VillageStealMenu, VillageStealAttemptMenu, "Look for opportunities to steal supplies", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.Continue; // ???
                return true;
                }, delegate (MenuCallbackArgs args)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Steal attempt at village"));
                }, false, 1, false);

                campaignGameStarter.AddGameMenuOption(VillageStealMenu, VillageStealBacktMenu, "Leave", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.Leave;
                    return true;
                }, delegate (MenuCallbackArgs args)
                {
                    GameMenu.SwitchToMenu(Hero.MainHero.CurrentSettlement.IsTown ? "town" : "village");
                }, false, 0, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Adding village steal menus:\n\n" + ex.Message);
            }


            try
            {
                //Town
                campaignGameStarter.AddGameMenu(TownStealMenu, TownStealMenuMsg, null, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("town", TownStealMenu, "Steal supplies from the traders", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.Continue; // ???
                return true;
                }, delegate (MenuCallbackArgs args)
                {
                    GameMenu.SwitchToMenu(TownStealMenu);
                }, false, 5, false);

                campaignGameStarter.AddGameMenuOption(TownStealMenu, TownStealAttemptMenu, "Look for opportunities to steal supplies", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.Continue;  // ???
                return true;
                }, delegate (MenuCallbackArgs args)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Steal attempt at village"));
                }, false, 1, false);

                campaignGameStarter.AddGameMenuOption(TownStealMenu, TownStealBacktMenu, "Leave", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.Leave;
                    return true;
                }, delegate (MenuCallbackArgs args)
                {
                    GameMenu.SwitchToMenu(Hero.MainHero.CurrentSettlement.IsTown ? "town" : "village");
                }, false, 0, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Adding town steal menus:\n\n" + ex.Message);
            }

        }

        public static string VillageStealMenu = "village_steal";
        public static string VillageStealAttemptMenu = "village_steal_atempt";
        public static string VillageStealBacktMenu = "village_steal_back";
        public static string VillageStealMenuMsg = "In this Village, you see peasants are working day and night in the fields to feed the community. There might be considerable amount of supplies  hidden somewhere, waiting to be yours.";

        public static string TownStealMenu = "town_steal";
        public static string TownStealAttemptMenu = "town_steal_atempt";
        public static string TownStealBacktMenu = "town_steal_back";
        public static string TownStealMenuMsg = "The Town is full of rich traders trying to sell goods in the street and bargaining for the most profitable deal. If you look long enough, there might appear a way to 'relieve' them from their supplies.";
    }
}
