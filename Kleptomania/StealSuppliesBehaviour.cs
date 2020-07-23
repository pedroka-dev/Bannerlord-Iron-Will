using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem.Actions;

namespace xxKleptomania
{
    class StealSuppliesBehaviour : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
            KleptomaniaSubModule.Log.Info("Behaviour intialization | Sucessfully added CampaignGameStarter Listener");
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData<Dictionary<string, CampaignTime>>("_settlementLastStealDetectionTimeDictionary", ref this._settlementLastStealDetectionTimeDictionary);
        }

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            this.AddTownStealMenu(campaignGameStarter);
            this.AddVillageStealMenu(campaignGameStarter);

            KleptomaniaSubModule.Log.Info("Behaviour intialization | Sucessfully added Steal Menus");
        }

        #region AddGameMenu       
        private void AddTownStealMenu(CampaignGameStarter campaignGameStarter)     
        {
            try
            {
                //Game Menus
                campaignGameStarter.AddGameMenu("town_steal", "{TOWN_STEAL_INTRO}", this.game_menu_steal_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenu("town_steal_receive", "{SETTLEMENT_STEAL_RECEIVE} {SETTLEMENT_STEAL_RECEIVE_LOOT} {SETTLEMENT_STEAL_RECEIVE_DETECT}", this.game_menu_steal_receive_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddWaitGameMenu("town_steal_wait", "Wait for some opportunity to steal the supplies to appear at {SETTLEMENT_NAME}...\n {SETTLEMENT_STEAL_INTRO_DETECTION} {SETTLEMENT_STEAL_INTRO_MIN_GOODS}", this.game_menu_steal_wait_on_init, this.game_menu_steal_wait_on_condition, this.game_menu_steal_wait_on_consequence, this.game_menu_steal_wait_on_tick, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameOverlays.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.none, null);


                //Game Menu Options
                campaignGameStarter.AddGameMenuOption("town", "town_steal", "Steal supplies from the traders.", this.game_menu_steal_on_condition, this.game_menu_steal_on_consequence, false, 7, false);
                campaignGameStarter.AddGameMenuOption("town_steal", "town_steal_atempt", "Look for opportunities to steal supplies.", this.game_menu_steal_atempt_on_condition, this.game_menu_steal_atempt_on_consequence, false, 1, false);
                campaignGameStarter.AddGameMenuOption("town_steal", "town_steal_back", "Leave.", this.game_menu_steal_leave_on_condition, this.game_menu_steal_leave_on_consequence, false, -1, false);
                campaignGameStarter.AddGameMenuOption("town_steal_wait", "town_steal_wait_back", "Forget it.", this.game_menu_steal_leave_on_condition, this.game_menu_steal_leave_on_consequence, false, -1, false);
                campaignGameStarter.AddGameMenuOption("town_steal_receive", "town_steal_loot", "Loot the supplies.", this.game_menu_steal_receive_on_condition, this.game_menu_steal_receive_on_consequence, false, -1, false);

                KleptomaniaSubModule.Log.Info("Behaviour intialization | Sucessfully added Town Steal Menus");
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("Something went wrong with Iron Will - Wound Experience: " + ex.Message, Colors.Red));
                KleptomaniaSubModule.Log.Error("Error on adding StealSuppliesBehaviour for Towns | " + ex.Message);
            }
        }

        private void AddVillageStealMenu(CampaignGameStarter campaignGameStarter)
        {
            try
            {
                //Game Menus
                campaignGameStarter.AddGameMenu("village_steal", "{VILLAGE_STEAL_INTRO}", this.game_menu_steal_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenu("village_steal_receive", "{SETTLEMENT_STEAL_RECEIVE} {SETTLEMENT_STEAL_RECEIVE_LOOT} {SETTLEMENT_STEAL_RECEIVE_DETECT}", this.game_menu_steal_receive_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddWaitGameMenu("village_steal_wait", "Wait for some opportunity to steal the supplies to appear at {SETTLEMENT_NAME}...\n {SETTLEMENT_STEAL_INTRO_DETECTION} {SETTLEMENT_STEAL_INTRO_MIN_GOODS}", this.game_menu_steal_wait_on_init, this.game_menu_steal_wait_on_condition, this.game_menu_steal_wait_on_consequence, this.game_menu_steal_wait_on_tick, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameOverlays.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.none, null);


                //Game Menu Options
                campaignGameStarter.AddGameMenuOption("village", "village_steal", "Steal supplies from the peasants.", this.game_menu_steal_on_condition, this.game_menu_steal_on_consequence, false, 4, false);
                campaignGameStarter.AddGameMenuOption("village_steal", "village_steal_atempt", "Look for opportunities to steal supplies.", this.game_menu_steal_atempt_on_condition, this.game_menu_steal_atempt_on_consequence, false, 1, false);
                campaignGameStarter.AddGameMenuOption("village_steal", "village_steal_back", "Leave.", this.game_menu_steal_leave_on_condition, this.game_menu_steal_leave_on_consequence, false, -1, false);
                campaignGameStarter.AddGameMenuOption("village_steal_wait", "village_steal_wait_back", "Forget it.", this.game_menu_steal_leave_on_condition, this.game_menu_steal_leave_on_consequence, false, -1, false);
                campaignGameStarter.AddGameMenuOption("village_steal_receive", "village_steal_loot", "Loot the supplies.", this.game_menu_steal_receive_on_condition, this.game_menu_steal_receive_on_consequence, false, -1, false);

                KleptomaniaSubModule.Log.Info("Behaviour intialization | Sucessfully added Village Steal Menus");
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("Something went wrong with Iron Will - Wound Experience: " + ex.Message, Colors.Red));
                KleptomaniaSubModule.Log.Error("Error on adding StealSuppliesBehaviour for Villages | " + ex.Message);
            }
        }
        #endregion

        #region CalculateSteal
        private void CalculateDetectionBonus()      //current bonuses: Night = -10%
        {
            currentDetectionChance = KleptomaniaSubModule.settings.BaseDetectionChance;
            if (CampaignTime.Now.IsNightTime)
            {
                currentDetectionChance = currentDetectionChance - 10;
                isNight = true;
            }
        }

        private void CalculateLootBonus()       //current bonuses:
        {
            currentMinimunGoods = KleptomaniaSubModule.settings.BaseMinimunGoods;
        }

        private void CalculateDetectionResult()     //to be undetected, random number stealDetectionResult should be bigger than the currentDetectionChance
        {
            int stealDetectionResult = MBRandom.RandomInt(1, 100);

            if (stealDetectionResult >= currentDetectionChance)
            {
                isDetectedResult = false;
            }
            else
            {
                _settlementLastStealDetectionTimeDictionary.Add(Settlement.CurrentSettlement.StringId, CampaignTime.Now);
                isDetectedResult = true;
            }
        }

        private void CalculateLootResult()      //get a random ammount of goods. but if its smaller than the currentMinimunGoods, gets currentMinimunGoods instead
        {

            int stealLootResult = MBRandom.RandomInt(1, 100);

            if (stealLootResult >= currentMinimunGoods)
            {
                lootQuantityResult = stealLootResult;
            }
            else
            {
                lootQuantityResult = currentMinimunGoods;
            }
        }
        #endregion

        #region on_init, on_condition and on_consequence
        //init
        private void game_menu_steal_on_init(MenuCallbackArgs args)
        {
            if (Hero.MainHero.CurrentSettlement.IsTown)
            {
                MBTextManager.SetTextVariable("TOWN_STEAL_INTRO", "The Town is full of rich traders trying to sell goods in the street and bargaining for the most profitable deal. If you look long enough, there might appear a way to 'relieve' them from their supplies. \n", false);
            }
            else if (Hero.MainHero.CurrentSettlement.IsVillage)
            {
                MBTextManager.SetTextVariable("VILLAGE_STEAL_INTRO", "In this Village, you see peasants working day and night in the fields to feed the community. There might be considerable amount of supplies  hidden somewhere, waiting to be yours. \n", false);
            }
        }

        private void game_menu_steal_wait_on_init(MenuCallbackArgs args)
        {
            string detectionMsg = "\n\n- Current chance of detection: " + currentDetectionChance.ToString() + "%";
            string minimunGoodsMsg = "\n- Current garanteed minimun goods: " + currentMinimunGoods.ToString() + "%";

            if (isNight)
            {
                detectionMsg = detectionMsg + "\n  * From Night Time : -10%";
            }

            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_INTRO_DETECTION", detectionMsg, false);
            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_INTRO_MIN_GOODS", minimunGoodsMsg, false);
        }

        private void game_menu_steal_receive_on_init(MenuCallbackArgs args)
        {
            CalculateDetectionResult();
            CalculateLootResult();
            
            string detectMsg;
            string lootMsg;

            if (isDetectedResult)
            {
                detectMsg = "\n\n  - Detected. Somebody witnessed you stealling the loot. ";
            }
            else
            {
                detectMsg = "\n\n  - Not Detected. Nobody saw you getting in and out with the loot. ";
            }


            if (lootQuantityResult >= 80)
            {
                lootMsg =  "\n  - Stole a huge ammount of supplies (" + lootQuantityResult.ToString() + "%).";
            }
            else if(lootQuantityResult >= 50)
            {
                lootMsg = "\n  - Stole a considerable ammount of supplies (" + lootQuantityResult.ToString() + "%).";
            }
            else if(lootQuantityResult >= 30){
                lootMsg = "\n  - Stole a small ammount of supplies (" + lootQuantityResult.ToString() + "%).";
            }
            else
            {
                lootMsg = "\n  - Stole a  pretty much nothing (" + lootQuantityResult.ToString() + "%).";
            }

            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_RECEIVE", "You were able to secure the supplies stolen from " + Hero.MainHero.CurrentSettlement.Name + "\n", false);
            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_RECEIVE_DETECT", detectMsg, false);
            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_RECEIVE_LOOT", lootMsg, false);
        }


        //condition
        private bool game_menu_steal_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods;      //Option icon
            return true;
        }

        private bool game_menu_steal_leave_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Leave;      //Option icon
            return true;
        }

        private bool game_menu_steal_wait_on_condition(MenuCallbackArgs args)
        {
            args.MenuContext.GameMenu.AllowWaitingAutomatically();
            GoalStealTime = CampaignTime.HoursFromNow(KleptomaniaSubModule.settings.HoursWaitingToSteal);       //Goal time is 4 hours wait by default
            MBTextManager.SetTextVariable("SETTLEMENT_NAME", Settlement.CurrentSettlement.Name, false);
            return true;
        }

        private bool game_menu_steal_atempt_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods;      //Option icon
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

            if(Hero.MainHero.Clan == Settlement.CurrentSettlement.OwnerClan)
            {
                args.IsEnabled = false;
                args.Tooltip = new TextObject("This settlement is owned by your clan.", null);
            }

            return true;
        }

        private bool game_menu_steal_receive_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods;      //Option icon
            return true;
        }

        //on_tick
        private void game_menu_steal_wait_on_tick(MenuCallbackArgs args, CampaignTime dt)
        {
            CampaignTime currentStealTime = CampaignTime.Now;
            if (currentStealTime.CompareTo(GoalStealTime) == 1)     //calls if in the future of the goal time
            {
                if (Settlement.CurrentSettlement.IsTown)
                {
                    GameMenu.SwitchToMenu("town_steal_receive");
                }
                else if (Settlement.CurrentSettlement.IsVillage)
                {
                    GameMenu.SwitchToMenu("village_steal_receive");
                }
            }
        }

        //on_consequence
        private void game_menu_steal_on_consequence(MenuCallbackArgs args)
        {
            if (Settlement.CurrentSettlement.IsTown)
            {
                GameMenu.SwitchToMenu("town_steal");
            }
            else if (Settlement.CurrentSettlement.IsVillage)
            {
                GameMenu.SwitchToMenu("village_steal");
            }
        }

        private void game_menu_steal_leave_on_consequence(MenuCallbackArgs args)
        {
            GameMenu.SwitchToMenu(Settlement.CurrentSettlement.IsTown ? "town" : "village");
        }

        private void game_menu_steal_wait_on_consequence(MenuCallbackArgs args)
        {
            PlayerEncounter.Finish(true);
        }

        private void game_menu_steal_atempt_on_consequence(MenuCallbackArgs args)
        {
            CalculateDetectionBonus();
            CalculateLootBonus();

            if (Settlement.CurrentSettlement.IsTown)
            {
                GameMenu.SwitchToMenu("town_steal_wait");
            }
            else if (Settlement.CurrentSettlement.IsVillage)
            {
                GameMenu.SwitchToMenu("village_steal_wait");
            }    
        }

        private void game_menu_steal_receive_on_consequence(MenuCallbackArgs args)
        {
            InformationManager.DisplayMessage(new InformationMessage("Steal received at settlement. Quantity: " + lootQuantityResult.ToString() + "%. Detected: "+ isDetectedResult.ToString()));
            KleptomaniaSubModule.Log.Info("Stealing | Steal sucessfull. Quantity: " + lootQuantityResult.ToString() + " %. Detected: "+ isDetectedResult.ToString());

            float currentTownStealCrimeRating = KleptomaniaSubModule.settings.TownStealCrimeRating;
            float villageStealCrimeRating = KleptomaniaSubModule.settings.VillageStealCrimeRating;
            int stealRelationPenalty = KleptomaniaSubModule.settings.StealRelationPenalty;

            if (isDetectedResult)
            {
                if (Settlement.CurrentSettlement.IsTown)
                {
                    ChangeCrimeRatingAction.Apply(Settlement.CurrentSettlement.MapFaction, currentTownStealCrimeRating, true);
                }
                else if (Settlement.CurrentSettlement.IsVillage)
                {
                    ChangeCrimeRatingAction.Apply(Settlement.CurrentSettlement.MapFaction, villageStealCrimeRating, true);
                }

                if(Hero.MainHero.MapFaction == Settlement.CurrentSettlement.MapFaction)     //if from player faction: realtion penalty x2, decreases relation with players faction leader
                {
                    stealRelationPenalty = stealRelationPenalty * 2;
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, Settlement.CurrentSettlement.MapFaction.Leader, stealRelationPenalty, true);
                }

                ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, Settlement.CurrentSettlement.OwnerClan.Leader, stealRelationPenalty, true);        //decreases relation with settlement owner

                foreach (Hero notableHero in Settlement.CurrentSettlement.Notables)     //decreases relation with notables
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, notableHero, stealRelationPenalty, true);
                }
            }
            
            PlayerEncounter.LeaveSettlement();
            PlayerEncounter.Finish(true);
        }
        #endregion


        private Dictionary<string, CampaignTime> _settlementLastStealDetectionTimeDictionary = new Dictionary<string, CampaignTime>();

        private int currentDetectionChance;
        private int currentMinimunGoods;
        private CampaignTime GoalStealTime;
        private bool isNight;

        private int lootQuantityResult;
        private bool isDetectedResult;
        
    }
}