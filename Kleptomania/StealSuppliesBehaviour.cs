using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

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
            //dataStore.SyncData<Dictionary<string, CampaignTime>>("_settlementLastStealDetectionTimeDictionary", ref this._settlementLastStealDetectionTimeDictionary);
        }

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            if(this._settlementLastStealDetectionTimeDictionary == null)
            {
                Dictionary<string, CampaignTime> _settlementLastStealDetectionTimeDictionary = new Dictionary<string, CampaignTime>();
            }
            
            this.AddTownStealMenu(campaignGameStarter);
            this.AddVillageStealMenu(campaignGameStarter);
        }

        
        private void AddTownStealMenu(CampaignGameStarter campaignGameStarter)  
        {
            try
            {
                campaignGameStarter.AddGameMenu("town_steal", TownStealMenuMsg, null, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("town", "town_steal", "Steal supplies from the traders", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods; 
                    return true;
                }, delegate (MenuCallbackArgs args)
                {
                    GameMenu.SwitchToMenu("town_steal");
                }, false, 6, false);


                campaignGameStarter.AddGameMenuOption("town_steal", "town_steal_atempt", "Look for opportunities to steal supplies",
                    new GameMenuOption.OnConditionDelegate(this.game_menu_settlement_steal_atempt_condition),
                    new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_steal_atempt_consequence), 
                false, 1, false);

                campaignGameStarter.AddGameMenuOption("town_steal", "town_steal_back", "Leave", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.Leave;      //Option icon
                    return true;
                }, delegate (MenuCallbackArgs args)
                {
                    GameMenu.SwitchToMenu(Hero.MainHero.CurrentSettlement.IsTown ? "town" : "village");
                }, false, -1, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Adding town steal menus:\n\n" + ex.Message);
            }
        }

        private void AddVillageStealMenu(CampaignGameStarter campaignGameStarter)
        {
            try
            {
                campaignGameStarter.AddGameMenu("village_steal", VillageStealMenuMsg, null, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("village", "village_steal", "Steal supplies from the peasants", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods;      //Option icon
                    return true;
                }, delegate (MenuCallbackArgs args)
                {
                    GameMenu.SwitchToMenu("village_steal");
                }, false, 5, false);


                campaignGameStarter.AddGameMenuOption("village_steal", "village_steal_atempt", "Look for opportunities to steal supplies",
                    new GameMenuOption.OnConditionDelegate(this.game_menu_settlement_steal_atempt_condition),
                    new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_steal_atempt_consequence), 
                false, 1, false);


                campaignGameStarter.AddGameMenuOption("village_steal", "village_steal_back", "Leave", delegate (MenuCallbackArgs x)
                {
                    x.optionLeaveType = GameMenuOption.LeaveType.Leave;      //Option icon
                    return true;
                }, delegate (MenuCallbackArgs args)
                {
                    GameMenu.SwitchToMenu(Hero.MainHero.CurrentSettlement.IsTown ? "town" : "village");
                }, false, -1, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Adding village steal menus:\n\n" + ex.Message);
            }
        }


        private bool game_menu_settlement_steal_atempt_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods;      //Option icon
            try
            {                                                                  //this.CheckSettlementAttackableHonorably(args);
                if (this._settlementLastStealDetectionTimeDictionary != null && this._settlementLastStealDetectionTimeDictionary.ContainsKey(Settlement.CurrentSettlement.StringId))
                {
                    if (this._settlementLastStealDetectionTimeDictionary[Settlement.CurrentSettlement.StringId].ElapsedDaysUntilNow <= 10f)
                    {
                        args.IsEnabled = false;
                        args.Tooltip = new TextObject("You have been detected stealing from this settlement recently.", null);
                    }
                    else
                    {
                        this._settlementLastStealDetectionTimeDictionary.Remove(Settlement.CurrentSettlement.StringId);
                    }
                }
                int num = MathF.Ceiling((float)Settlement.CurrentSettlement.ItemRoster.TotalFood * 0.2f);
                if (MobileParty.MainParty.CurrentSettlement != null && num <= 0)
                {
                    args.IsEnabled = false;
                    args.Tooltip = new TextObject("This settlement has no goods to steal.", null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on settlement_steal_atempt_condition:\n\n" + ex.Message);
            }
            return true;
        }

        private void game_menu_town_steal_atempt_consequence(MenuCallbackArgs args)
        {
            InformationManager.DisplayMessage(new InformationMessage("Steal attempt at town"));
        }

        private void game_menu_village_steal_atempt_consequence(MenuCallbackArgs args)
        {
            InformationManager.DisplayMessage(new InformationMessage("Steal attempt at village"));
        }


       
        public Dictionary<string, CampaignTime> _settlementLastStealDetectionTimeDictionary;

        public static string VillageStealMenuMsg = "In this Village, you see peasants working day and night in the fields to feed the community. There might be considerable amount of supplies  hidden somewhere, waiting to be yours.";
        public static string TownStealMenuMsg = "The Town is full of rich traders trying to sell goods in the street and bargaining for the most profitable deal. If you look long enough, there might appear a way to 'relieve' them from their supplies.";
    }
}
