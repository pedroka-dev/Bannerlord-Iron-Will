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

        private string TextPrefixFromValue(int value)       //to add string prefixes to a given value
        {
            string message;
            if (value > 80)
            {
                message = "Very High";
            }
            else if(value > 60)
            {
                message = "High";
            }
            else if(value > 40)
            {
                message = "Medium";
            }
            else if(value > 20)
            {
                message = "Low";
            }
            else
            {
                message = "Very Low";
            }
            return message;
        }

        #region AddGameMenu       
        private void AddTownStealMenu(CampaignGameStarter campaignGameStarter)     
        {
            try
            {
                //Game Menus
                campaignGameStarter.AddGameMenu("town_steal", "{TOWN_STEAL_INTRO}  \n{SETTLEMENT_STEAL_INTRO_CONSEQUENCE}", this.game_menu_steal_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenu("town_steal_receive", "{SETTLEMENT_STEAL_RECEIVE}  \n{SETTLEMENT_STEAL_RECEIVE_DETECT}  \n{SETTLEMENT_STEAL_RECEIVE_LOOT}", this.game_menu_steal_receive_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddWaitGameMenu("town_steal_wait", "{SETTLEMENT_STEAL_WAIT}  \n{SETTLEMENT_STEAL_WAIT_DETECTION}  \n{SETTLEMENT_STEAL_WAIT_MIN_GOODS}", this.game_menu_steal_wait_on_init, this.game_menu_steal_wait_on_condition, this.game_menu_steal_wait_on_consequence, this.game_menu_steal_wait_on_tick, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameOverlays.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.none, null);


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
                campaignGameStarter.AddGameMenu("village_steal", "{VILLAGE_STEAL_INTRO}  \n{SETTLEMENT_STEAL_INTRO_CONSEQUENCE}", this.game_menu_steal_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenu("village_steal_receive", "{SETTLEMENT_STEAL_RECEIVE}  \n{SETTLEMENT_STEAL_RECEIVE_DETECT}  \n{SETTLEMENT_STEAL_RECEIVE_LOOT}", this.game_menu_steal_receive_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddWaitGameMenu("village_steal_wait", "{SETTLEMENT_STEAL_WAIT}  \n{SETTLEMENT_STEAL_WAIT_DETECTION}  \n{SETTLEMENT_STEAL_WAIT_MIN_GOODS}", this.game_menu_steal_wait_on_init, this.game_menu_steal_wait_on_condition, this.game_menu_steal_wait_on_consequence, this.game_menu_steal_wait_on_tick, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameOverlays.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.none, null);


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
        private void CalculateDetectionBonus()      //current bonuses: Night = -10%, From Skill = Roguery LvL /5
        {
            currentDetectionChance = KleptomaniaSubModule.settings.BaseDetectionChance;

            detectionSkillBonus = MathF.Ceiling(Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) / 5);
            if(detectionSkillBonus > 50)        //Max detection bonus from roguery skill = -50%
            {
                detectionSkillBonus = 50;
            }
            currentDetectionChance = currentDetectionChance - detectionSkillBonus;

            if (CampaignTime.Now.IsNightTime)
            {
                currentDetectionChance = currentDetectionChance - 10;
                isNight = true;
            }
        }

        private void CalculateLootBonus()       //current bonuses: From Skill = Roguery LvL /10
        {
            currentMinimunGoods = KleptomaniaSubModule.settings.BaseMinimunGoods;

            minimunGoodsSkillBonus = MathF.Ceiling(Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) / 10);
            if (minimunGoodsSkillBonus > 30)        //Max minimun goods bonus from roguery skill = +30%
            {
                minimunGoodsSkillBonus = 30;
            }
            currentMinimunGoods = currentMinimunGoods + minimunGoodsSkillBonus;
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
            string settlementIntroMsg;
            if (Settlement.CurrentSettlement.IsTown)
            {
                settlementIntroMsg = "The Town is full of rich traders trying to sell goods in the street and bargaining for the most profitable deal. By looking long enough, there might appear a way to 'relieve' them from their supplies. \n";
                MBTextManager.SetTextVariable("TOWN_STEAL_INTRO", settlementIntroMsg, false);
            }
            else if (Settlement.CurrentSettlement.IsVillage)
            {
                settlementIntroMsg = "In this Village, you see peasants working day and night in the fields to feed the community. Perhaps there is a considerable amount of supplies hidden somewhere, waiting to be yours. \n";
                MBTextManager.SetTextVariable("VILLAGE_STEAL_INTRO", settlementIntroMsg, false);
            }

            string settlementIntroConsequenceMsg = "\nIf you get caught stealing, your criminal rating with the faction will increase and ";

            if(Hero.MainHero.MapFaction != Settlement.CurrentSettlement.MapFaction)
            {
                settlementIntroConsequenceMsg = settlementIntroConsequenceMsg + "your relation will decrease with the settlement owner and influent people living here.";
            }
            else
            {
                settlementIntroConsequenceMsg = settlementIntroConsequenceMsg + "as this is owned by your faction, your relation will decrease a lot with the faction leader, settlement owner and influent people living here.";
            }

            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_INTRO_CONSEQUENCE", settlementIntroConsequenceMsg, false);
        }

        private void game_menu_steal_wait_on_init(MenuCallbackArgs args)
        {
            string detectionMsg ;
            string minimunGoodsMsg;

            detectionMsg = "\n - " + TextPrefixFromValue(currentDetectionChance) + " chance of detection during the steal attempt (" + currentDetectionChance.ToString() + "% probability of detection).";
            detectionMsg = detectionMsg + "\n  * From Roguery skill level (-" + detectionSkillBonus.ToString() + "%)  ";
            if (isNight)
            {
                detectionMsg = detectionMsg + "\n  * From night time (-10%)  ";
            }

           minimunGoodsMsg = "\n - " + TextPrefixFromValue(currentDetectionChance) + " ammount of garanteed minimun goods (at least " + currentMinimunGoods.ToString() + "% of the storage).";
           minimunGoodsMsg = minimunGoodsMsg + "\n  * From Roguery skill level (+" + minimunGoodsSkillBonus.ToString() + "%)  ";

           MBTextManager.SetTextVariable("SETTLEMENT_STEAL_WAIT", "Wait for some opportunity to steal the supplies to appear at " + Settlement.CurrentSettlement.Name + "...", false);
           MBTextManager.SetTextVariable("SETTLEMENT_STEAL_WAIT_DETECTION", detectionMsg, false);
           MBTextManager.SetTextVariable("SETTLEMENT_STEAL_WAIT_MIN_GOODS", minimunGoodsMsg, false);
        }

        private void game_menu_steal_receive_on_init(MenuCallbackArgs args)
        {
            CalculateDetectionResult();
            CalculateLootResult();
            
            string detectMsg;
            string lootMsg;

            if (isDetectedResult)
            {
                detectMsg = "\nAs you leave you hear someone yell at you: 'Thief!'. You run off as quickly as you can. ";
            }
            else
            {
                detectMsg = "\nYou sneak out of the village without being seen by anyone. ";
            }

            lootMsg = "\nIn your bag, there is a " + TextPrefixFromValue(lootQuantityResult) + " ammount of stolen supplies (" + lootQuantityResult.ToString() + "% of the storage).";

            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_RECEIVE", "You were able to secure the supplies stolen from " + Settlement.CurrentSettlement.Name + "\n", false);
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

        private int detectionSkillBonus;
        private int minimunGoodsSkillBonus;
        private int currentDetectionChance;
        private int currentMinimunGoods;


        private CampaignTime GoalStealTime;
        private bool isNight;

        private int lootQuantityResult;
        private bool isDetectedResult;
    }
}