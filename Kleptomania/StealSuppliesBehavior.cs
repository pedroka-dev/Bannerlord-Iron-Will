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
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using System.Windows.Documents;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using System.Linq;

namespace xxKleptomania
{
    class StealSuppliesBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
            KleptomaniaSubModule.Log.Info("Behaviour intialization | Sucessfully added CampaignGameStarter Listener");
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData<Dictionary<string, CampaignTime>>("_settlementLastStealDetectionTimeDictionary", ref this._settlementLastStealDetectionTimeDictionary);
            dataStore.SyncData<Dictionary<IFaction, int>>("_recentFactionStealAttemptPenalty", ref this._recentFactionStealAttemptPenalty);
        }

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            this.AddTownStealMenu(campaignGameStarter);
            this.AddVillageStealMenu(campaignGameStarter);

            KleptomaniaSubModule.Log.Info("Behaviour intialization | Sucessfully added Steal Menus");
        }

        private void OnDailyTick()
        {
            try
            {
                if (this._recentFactionStealAttemptPenalty != null)
                {
                    foreach (IFaction RecentStolenFaction in this._recentFactionStealAttemptPenalty.Keys.ToList())
                    {
                        this._recentFactionStealAttemptPenalty.TryGetValue(RecentStolenFaction, out int updatedValue);
                        updatedValue -= 5;
                        this._recentFactionStealAttemptPenalty.Remove(RecentStolenFaction);
                        if (updatedValue > 0)
                        {
                            this._recentFactionStealAttemptPenalty.Add(RecentStolenFaction, updatedValue);

                            if (KleptomaniaSubModule.settings.DebugInfo)
                            {
                                InformationManager.DisplayMessage(new InformationMessage("Decreased " + RecentStolenFaction.Name + " recent steal penalty by " + 5 + " during OnDailyTick", Colors.Yellow));
                            }
                            KleptomaniaSubModule.Log.Info("Stealing | Decreased " + RecentStolenFaction.Name + " recent steal penalty by " + 5 + " during OnDailyTick");
                        }
                        else
                        {
                            if (KleptomaniaSubModule.settings.DebugInfo)
                            {
                                InformationManager.DisplayMessage(new InformationMessage("Removed " + RecentStolenFaction.Name + "from recent steal penalty dictionary during OnDailyTick", Colors.Yellow));
                            }
                            KleptomaniaSubModule.Log.Info("Stealing | Removed " + RecentStolenFaction.Name + "from recent steal penalty dictionary during OnDailyTick");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("Kleptomania: An error occured OnDailyTick. Check the Log file.", Colors.Red));
                KleptomaniaSubModule.Log.Info("Stealing | Exception OnDailyTick: " + ex.Message);
            }
        }

        public void OnStealAttempt(Settlement currentSettlement, bool wasDetected, int ammountFromProsperity, int quantityStolen)
        {
            if (KleptomaniaSubModule.settings.DebugInfo)
            {
                InformationManager.DisplayMessage(new InformationMessage("Detected = " + wasDetected + ". Steal Quantity = " + quantityStolen, Colors.Yellow));
            }
            KleptomaniaSubModule.Log.Info("Stealing | Steal sucessfull. Quantity: " + quantityStolen.ToString() + " %. Detected: " + wasDetected.ToString());

            if (currentSettlement.IsTown)
            {
                GameMenu.SwitchToMenu("town");
                if (quantityStolen > 0)
                {
                    currentSettlement.Prosperity -= ammountFromProsperity * 5;
                }
            }
            else if (currentSettlement.IsVillage)
            {
                GameMenu.SwitchToMenu("village");
                if (quantityStolen > 0)
                {
                    currentSettlement.Village.Hearth -= ammountFromProsperity;
                }
            }
            if (KleptomaniaSubModule.settings.DebugInfo)
            {
                InformationManager.DisplayMessage(new InformationMessage("Settlement Hearth / prosperity decreased by " + ammountFromProsperity.ToString(), Colors.Yellow));
            }
            KleptomaniaSubModule.Log.Info("Stealing | Settlement " + currentSettlement.Name + " hearth/prosperity decreased by " + ammountFromProsperity.ToString());

            IncreaseRecentStealPenalty(currentSettlement.MapFaction);
            if (wasDetected)
            {
                OnDetection(currentSettlement);
                if (this._settlementLastStealDetectionTimeDictionary != null)
                {
                    _settlementLastStealDetectionTimeDictionary.Add(currentSettlement.StringId, CampaignTime.Now);
                }
            }

            LootStolenGoods(currentSettlement, ammountFromProsperity, quantityStolen);
            stealUtils.GiveRogueryXp(Hero.MainHero);
        }

        public void OnDetection(Settlement settlement)
        {
            try
            {
                int stealRelationPenalty = KleptomaniaSubModule.settings.StealRelationPenalty;
                float currentTownStealCrimeRating = KleptomaniaSubModule.settings.TownStealCrimeRating;
                float currentVillageStealCrimeRating = KleptomaniaSubModule.settings.VillageStealCrimeRating;

                if (Hero.MainHero.MapFaction != null && settlement.MapFaction != null)
                {
                    if (Hero.MainHero.MapFaction == settlement.MapFaction)     //if from player faction: realtion penalty x2, decreases relation with players faction leader
                    {
                        stealRelationPenalty *= 2;
                        ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, settlement.MapFaction.Leader, stealRelationPenalty, true);
                        KleptomaniaSubModule.Log.Info("Stealing | Faction Leader Hero " + settlement.MapFaction.Leader.Name + "decreases relation with player by " + stealRelationPenalty.ToString());
                    }
                }

                if (settlement.OwnerClan.Leader != null)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, settlement.OwnerClan.Leader, stealRelationPenalty, true);        //decreases relation with settlement owner
                    KleptomaniaSubModule.Log.Info("Stealing | Settlement owner Hero " + settlement.OwnerClan.Leader.Name + "decreases relation with player by " + stealRelationPenalty.ToString());
                }

                if (settlement.Notables != null)
                {
                    foreach (Hero notableHero in settlement.Notables)     //decreases relation with notables
                    {
                        if (notableHero != null && !notableHero.IsGangLeader)
                        {
                            ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, notableHero, stealRelationPenalty, true);
                            KleptomaniaSubModule.Log.Info("Stealing | Notable Hero " + notableHero.Name + "decreases relation with player by " + stealRelationPenalty.ToString());
                        }
                    }
                }

                if (settlement.IsTown)
                {
                    ChangeCrimeRatingAction.Apply(settlement.MapFaction, currentTownStealCrimeRating, true);
                    KleptomaniaSubModule.Log.Info("Stealing | Faction " + settlement.MapFaction.Name + " crime rating increases with player by " + currentTownStealCrimeRating.ToString());
                }
                else if (settlement.IsVillage)
                {
                    ChangeCrimeRatingAction.Apply(settlement.MapFaction, currentVillageStealCrimeRating, true);
                    KleptomaniaSubModule.Log.Info("Stealing | Faction " + settlement.Name + " crime rating increases with player by " + currentVillageStealCrimeRating.ToString());
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("Kleptomania: An error occured adding consequences for detection. Check the Log file.", Colors.Red));
                KleptomaniaSubModule.Log.Info("Stealing | Exception in DetectionConsequence: " + ex.Message);
            }
        }

        public void IncreaseRecentStealPenalty(IFaction faction)
        {
            try     //AddRecentStealPenalty(IFaction faction)
            {
                int updatedValue = 10;
                if (this._recentFactionStealAttemptPenalty.ContainsKey(faction))
                {
                    this._recentFactionStealAttemptPenalty.TryGetValue(faction, out updatedValue);
                    updatedValue += 10;
                    this._recentFactionStealAttemptPenalty.Remove(faction);
                    this._recentFactionStealAttemptPenalty.Add(faction, updatedValue);

                    if (KleptomaniaSubModule.settings.DebugInfo)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Increased " + faction.Name + " recent steal penalty to " + updatedValue, Colors.Yellow));
                    }
                    KleptomaniaSubModule.Log.Info("Stealing | Increased " + faction.Name + " recent steal penalty to " + updatedValue);
                }
                else
                {
                    this._recentFactionStealAttemptPenalty.Add(faction, updatedValue);

                    if (KleptomaniaSubModule.settings.DebugInfo)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Added " + faction.Name + " to recent steal penalty dictionary and increased by " + updatedValue, Colors.Yellow));
                    }
                    KleptomaniaSubModule.Log.Info("Stealing | Added " + faction.Name + " to recent steal penalty dictionary and increased by " + updatedValue);
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("Kleptomania: An error occured adding settlement to recent steal attempts. Check the Log file.", Colors.Red));
                KleptomaniaSubModule.Log.Info("Stealing | Exception in AddRecentStealPenalty: " + ex.Message);
            }
        }

        public void LootStolenGoods(Settlement settlement, int ammountFromProsperity, int goodsQuantity)
        {
            try
            {
                ItemRoster itemRoster = new ItemRoster();

                int lootQuantity = MathF.Ceiling(((float)(ammountFromProsperity * goodsQuantity / 100)));
                int cheapestAnimalValue = 50;       //used to pick the cheapest animal availiable first

                while (lootQuantity > 0 && settlement.ItemRoster.Count > 0)
                {
                    int itemSeed = MBRandom.RandomInt();
                    for (int j = 0; j < settlement.ItemRoster.Count; j++)
                    {
                        ItemRosterElement itemRosterElement = settlement.ItemRoster[(j + itemSeed) % settlement.ItemRoster.Count];
                        ItemObject item = itemRosterElement.EquipmentElement.Item;
                        if (!itemRosterElement.IsEmpty && lootQuantity > 0)
                        {
                            if (item.IsTradeGood)
                            {
                                int randomAmmount = MBRandom.RandomInt(Math.Min(lootQuantity, itemRosterElement.Amount) - 1) + 1;
                                settlement.ItemRoster.AddToCounts(item, -randomAmmount, true);
                                itemRoster.AddToCounts(item, randomAmmount, true);
                                lootQuantity -= randomAmmount;
                            }
                            else if (item.IsAnimal || item.IsMountable)
                            {
                                if (item.Value <= cheapestAnimalValue)
                                {
                                    int randomAmmount = MBRandom.RandomInt(Math.Min(lootQuantity, itemRosterElement.Amount) - 1) + 1;
                                    settlement.ItemRoster.AddToCounts(item, -randomAmmount, true);
                                    itemRoster.AddToCounts(item, randomAmmount, true);
                                    lootQuantity -= randomAmmount;
                                }
                                else
                                {
                                    cheapestAnimalValue += 100;
                                }
                            }
                        }
                    }
                }

                KleptomaniaSubModule.Log.Info("Stealing | Total number of stolen goods: " + itemRoster.Count.ToString());
                if (KleptomaniaSubModule.settings.DebugInfo)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Total number of stolen goods: " + itemRoster.Count.ToString(), Colors.Yellow));
                }
                InventoryManager.OpenScreenAsLoot(new Dictionary<PartyBase, ItemRoster>
                {
                    {
                        PartyBase.MainParty,
                        itemRoster
                    }
                });
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("Kleptomania: An error occured on LootStolenGoods. Check the Log file.", Colors.Red));
                KleptomaniaSubModule.Log.Info("Stealing | Exception in LootStolenGoods: " + ex.Message);
            }
        }

        private void AddTownStealMenu(CampaignGameStarter campaignGameStarter)     
        {
            try
            {
                campaignGameStarter.AddGameMenuOption("town", "town_steal", "Steal supplies from the traders.", this.game_menu_steal_on_condition, this.game_menu_steal_on_consequence, false, 10, false);

                campaignGameStarter.AddGameMenu("town_steal", "{TOWN_STEAL_INTRO}  \n{SETTLEMENT_STEAL_INTRO_CONSEQUENCE}", this.game_menu_steal_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("town_steal", "town_steal_atempt", "Look for opportunities to steal supplies.", this.game_menu_steal_atempt_on_condition, this.game_menu_steal_atempt_on_consequence, false, 1, false);
                campaignGameStarter.AddGameMenuOption("town_steal", "town_steal_back", "Leave.", this.game_menu_steal_leave_on_condition, this.game_menu_steal_leave_on_consequence, false, -1, false);

                campaignGameStarter.AddWaitGameMenu("town_steal_wait", "{SETTLEMENT_STEAL_WAIT}  \n  {SETTLEMENT_STEAL_WAIT_DETECTION}  \n  {SETTLEMENT_STEAL_WAIT_MIN_GOODS}", this.game_menu_steal_wait_on_init, this.game_menu_steal_wait_on_condition, this.game_menu_steal_wait_on_consequence, this.game_menu_steal_wait_on_tick, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameOverlays.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("town_steal_wait", "town_steal_wait_back", "Forget it.", this.game_menu_steal_leave_on_condition, this.game_menu_steal_leave_on_consequence, false, -1, false);

                campaignGameStarter.AddGameMenu("town_steal_receive", "{SETTLEMENT_STEAL_RECEIVE}  \n{SETTLEMENT_STEAL_RECEIVE_DETECT}  \n  {SETTLEMENT_STEAL_RECEIVE_LOOT}", this.game_menu_steal_receive_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("town_steal_receive", "town_steal_loot", "Take away the supplies.", this.game_menu_steal_receive_on_condition, this.game_menu_steal_receive_on_consequence, false, -1, false);
                campaignGameStarter.AddGameMenuOption("town_steal_receive", "town_steal_encounter_persuade", "Here, some denars will shut you up. "+KleptomaniaSubModule.settings.EncounterBribeCost+"{GOLD_ICON}", this.game_menu_steal_encounter_persuade_on_condition, this.game_menu_steal_encounter_persuade_on_consequence, false, 0, false);
                campaignGameStarter.AddGameMenuOption("town_steal_receive", "town_steal_encounter_influence", "Do you even know who I am? "+KleptomaniaSubModule.settings.EncounterInfluenceCost + " Influence", this.game_menu_steal_encounter_influence_on_condition, this.game_menu_steal_encounter_influence_on_consequence, false, 1, false);
                campaignGameStarter.AddGameMenuOption("town_steal_receive", "town_steal_encounter_threat", "Don't mess with gang business, or else...", this.game_menu_steal_encounter_threat_on_condition, this.game_menu_steal_threat_on_consequence, false, 2, false);
                campaignGameStarter.AddGameMenuOption("town_steal_receive", "town_steal_encounter_giveback", "Fine, you can have all the supplies back.", this.game_menu_steal_encounter_giveback_on_condition, this.game_menu_steal_giveback_on_consequence, false, 3, false);

                KleptomaniaSubModule.Log.Info("Behaviour intialization | Sucessfully added Town Steal Menus");
                if (KleptomaniaSubModule.settings.DebugInfo)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Sucessfully added Town Steal Menus", Colors.Yellow));
                }
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
                campaignGameStarter.AddGameMenuOption("village", "village_steal", "Steal supplies from the peasants.", this.game_menu_steal_on_condition, this.game_menu_steal_on_consequence, false, 6, false);

                campaignGameStarter.AddGameMenu("village_steal", "{VILLAGE_STEAL_INTRO}  \n{SETTLEMENT_STEAL_INTRO_CONSEQUENCE}", this.game_menu_steal_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("village_steal", "village_steal_atempt", "Look for opportunities to steal supplies.", this.game_menu_steal_atempt_on_condition, this.game_menu_steal_atempt_on_consequence, false, 1, false);
                campaignGameStarter.AddGameMenuOption("village_steal", "village_steal_back", "Leave.", this.game_menu_steal_leave_on_condition, this.game_menu_steal_leave_on_consequence, false, -1, false);

                campaignGameStarter.AddWaitGameMenu("village_steal_wait", "{SETTLEMENT_STEAL_WAIT}  \n  {SETTLEMENT_STEAL_WAIT_DETECTION}  \n  {SETTLEMENT_STEAL_WAIT_MIN_GOODS}", this.game_menu_steal_wait_on_init, this.game_menu_steal_wait_on_condition, this.game_menu_steal_wait_on_consequence, this.game_menu_steal_wait_on_tick, GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameOverlays.MenuOverlayType.SettlementWithBoth, 0f, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("village_steal_wait", "village_steal_wait_back", "Forget it.", this.game_menu_steal_leave_on_condition, this.game_menu_steal_leave_on_consequence, false, -1, false);

                campaignGameStarter.AddGameMenu("village_steal_receive", "{SETTLEMENT_STEAL_RECEIVE}  \n{SETTLEMENT_STEAL_RECEIVE_DETECT}  \n  {SETTLEMENT_STEAL_RECEIVE_LOOT}", this.game_menu_steal_receive_on_init, GameOverlays.MenuOverlayType.SettlementWithCharacters, GameMenu.MenuFlags.none, null);
                campaignGameStarter.AddGameMenuOption("village_steal_receive", "village_steal_loot", "Take away the supplies.", this.game_menu_steal_receive_on_condition, this.game_menu_steal_receive_on_consequence, false, -1, false);
                campaignGameStarter.AddGameMenuOption("village_steal_receive", "village_steal_encounter_persuade", "Here, some denars will shut you up. " + KleptomaniaSubModule.settings.EncounterBribeCost + "{GOLD_ICON}", this.game_menu_steal_encounter_persuade_on_condition, this.game_menu_steal_encounter_persuade_on_consequence, false, 0, false);
                campaignGameStarter.AddGameMenuOption("village_steal_receive", "village_steal_encounter_influence", "Do you even know who I am? " + KleptomaniaSubModule.settings.EncounterInfluenceCost + " Influence", this.game_menu_steal_encounter_influence_on_condition, this.game_menu_steal_encounter_influence_on_consequence, false, 1, false);
                campaignGameStarter.AddGameMenuOption("village_steal_receive", "village_steal_encounter_threat", "Don't mess with gang business, or else...", this.game_menu_steal_encounter_threat_on_condition, this.game_menu_steal_threat_on_consequence, false, 2, false);
                campaignGameStarter.AddGameMenuOption("village_steal_receive", "village_steal_encounter_giveback", "Fine, you can have all the supplies back.", this.game_menu_steal_encounter_giveback_on_condition, this.game_menu_steal_giveback_on_consequence, false, 3, false);

                KleptomaniaSubModule.Log.Info("Behaviour intialization | Sucessfully added Village Steal Menus");
                if (KleptomaniaSubModule.settings.DebugInfo)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Sucessfully added Village Steal Menus", Colors.Yellow));
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("Something went wrong with Iron Will - Wound Experience: " + ex.Message, Colors.Red));
                KleptomaniaSubModule.Log.Error("Error on adding StealSuppliesBehaviour for Villages | " + ex.Message);
            }
        }

        //init
        private void game_menu_steal_on_init(MenuCallbackArgs args)
        {
            string settlementIntroMsg;
            string settlementIntroConsequenceMsg;

            if (Settlement.CurrentSettlement.IsTown)
            {
                settlementIntroMsg = "The Town is full of rich traders trying to sell goods in the street and bargaining for the most profitable deal. By looking long enough, there might appear a way to 'relieve' them from their supplies.  \n";
                prosperityGoodsAmmount = MathF.Ceiling(((float)(Settlement.CurrentSettlement.Town.Prosperity / 200)));
                settlementIntroMsg += "\nSeems like the storage you are planning to steal from is able to hold around " + prosperityGoodsAmmount.ToString() + " goods inside.\n";
                MBTextManager.SetTextVariable("TOWN_STEAL_INTRO", settlementIntroMsg, false);
            }
            else if (Settlement.CurrentSettlement.IsVillage)
            {
                settlementIntroMsg = "In this Village, you see peasants working day and night in the fields to feed the community. Perhaps there is a considerable amount of supplies hidden somewhere, waiting to be yours.  \n";
                prosperityGoodsAmmount = MathF.Ceiling(((float)(Settlement.CurrentSettlement.Village.Hearth / 20)));
                settlementIntroMsg += "\nSeems like the storage you are planning to steal from is able to hold around " + prosperityGoodsAmmount.ToString() + " goods inside.\n";
                MBTextManager.SetTextVariable("VILLAGE_STEAL_INTRO", settlementIntroMsg, false);
            }

            if (KleptomaniaSubModule.settings.DebugInfo)
            {
                InformationManager.DisplayMessage(new InformationMessage("Calculated items in storage = "+ prosperityGoodsAmmount, Colors.Yellow));
            }


            settlementIntroConsequenceMsg = "\nIf you get caught stealing you will not be able to steal from it for some time, your criminal rating with the faction will increase and ";

            if(Hero.MainHero.MapFaction != Settlement.CurrentSettlement.MapFaction)
            {
                settlementIntroConsequenceMsg += "your relation will decrease with the settlement owner and influent people living here.";
            }
            else
            {
                settlementIntroConsequenceMsg += "as this is owned by your faction your relation will decrease a lot with the faction leader, settlement owner and influent people living here.";
            }
            
            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_INTRO_CONSEQUENCE", settlementIntroConsequenceMsg, false);
        }

        private void game_menu_steal_wait_on_init(MenuCallbackArgs args)
        {
            string detectionMsg;
            string minimunGoodsMsg;
            bool isNight = CampaignTime.Now.IsNightTime;
            int detectionSkillBonus = MathF.Ceiling(Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) / 5);
            int minimunGoodsSkillBonus = MathF.Ceiling(Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) / 10);

            int recentStealPenalty = 0;
            if (this._recentFactionStealAttemptPenalty.ContainsKey(Settlement.CurrentSettlement.MapFaction))
            {
                this._recentFactionStealAttemptPenalty.TryGetValue(Settlement.CurrentSettlement.MapFaction, out recentStealPenalty);
            }

            currentDetectionChance = stealUtils.CalculateDetectionBonus(detectionSkillBonus, isNight, recentStealPenalty);
            currentMinimunGoods = stealUtils.CalculateLootBonus(minimunGoodsSkillBonus);            

            detectionMsg = "\n - " + stealUtils.TextPrefixFromValue(currentDetectionChance) + " chance of detection during the steal attempt (" + currentDetectionChance.ToString() + "% probability of detection).";
            detectionMsg = detectionMsg + "\n  * Roguery skill bonus (-" + detectionSkillBonus.ToString() + "%)  ";

            if (isNight)
            {
                detectionMsg += "\n  * Night time bonus (-10%)  ";
            }
            if (this._recentFactionStealAttemptPenalty.ContainsKey(Settlement.CurrentSettlement.MapFaction))
            {
                detectionMsg += "\n  * Recent steal from Faction penalty (+" + recentStealPenalty  + "%)";
            }

            minimunGoodsMsg = "\n - " + stealUtils.TextPrefixFromValue(currentMinimunGoods) + " ammount of garanteed minimun goods (at least " + currentMinimunGoods.ToString() + "% of the storage).";
            minimunGoodsMsg = minimunGoodsMsg + "\n  * Roguery skill bonus (+" + minimunGoodsSkillBonus.ToString() + "%)  ";

            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_WAIT", "Wait for some opportunity to steal the supplies to appear at " + Settlement.CurrentSettlement.Name + "...", false);
            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_WAIT_DETECTION", detectionMsg, false);
            MBTextManager.SetTextVariable("SETTLEMENT_STEAL_WAIT_MIN_GOODS", minimunGoodsMsg, false);

            if (KleptomaniaSubModule.settings.DebugInfo)
            {
                InformationManager.DisplayMessage(new InformationMessage("Calculated Detection Chance = " + currentDetectionChance + ". Calculated Minimun Goods Bonus = " + minimunGoodsSkillBonus, Colors.Yellow));
            }
            KleptomaniaSubModule.Log.Info("Stealing | Bonus calculated sucessfully. Detetion Chance = " + currentDetectionChance.ToString() + "%,  Minimun ammount of goods = " + currentMinimunGoods.ToString());
        }

        private void game_menu_steal_receive_on_init(MenuCallbackArgs args)
        {
            string detectMsg;
            string lootMsg;

            isDetected = stealUtils.CalculateDetectionResult(currentDetectionChance);
            stealQuantity = stealUtils.CalculateLootResult(currentMinimunGoods);

            if (isDetected)
            {
                detectMsg = "\nAs you leave you hear someone yell at you: 'Thief!'. The peasant who caught you comes to to confront you. He says that will call the guards on you for stealling from the settlement storage.";
            }
            else
            {
                detectMsg = "\nYou sneak out of the settlement without being seen by anyone. ";
            }

            lootMsg = "\nIn your bag, there is a " + stealUtils.TextPrefixFromValue(stealQuantity) + " ammount of stolen supplies (" + stealQuantity.ToString() + "% of the storage).";

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
            args.optionLeaveType = GameMenuOption.LeaveType.Trade;      //Option icon
            if (isDetected)
            {
                return false;
            }
            return true;
        }

        private bool game_menu_steal_encounter_persuade_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.RansomAndBribe ;      //Option icon

            if (!isDetected)
            {
                return false;
            }

            int bribeCost = KleptomaniaSubModule.settings.EncounterBribeCost;

            if (Hero.MainHero.Gold < bribeCost)     //change value
            {
                args.IsEnabled = false;
                args.Tooltip = new TextObject("You dont have enough denars to bribe. ("+ bribeCost+")", null);
            }

            return true;
        }

        private bool game_menu_steal_encounter_influence_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Conversation;      //Option icon

            if (!isDetected)
            {
                return false;
            }

            float influenceCost = KleptomaniaSubModule.settings.EncounterInfluenceCost; 

            if(Hero.MainHero.Clan.Influence < influenceCost)
            {
                args.IsEnabled = false;
                args.Tooltip = new TextObject("You dont have enough influence to spend. ("+ influenceCost + ")", null);
            }

            return true;
        }

        private bool game_menu_steal_encounter_threat_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods;      //Option icon

            if (!isDetected)
            {
                return false;
            }

            int relationCost = 50;

            if (Settlement.CurrentSettlement.Notables == null)
            {
                args.IsEnabled = false;
                args.Tooltip = new TextObject("There is no Gang Leader notables in this settlement.", null);
            }
            else
            {
                foreach (Hero notableHero in Settlement.CurrentSettlement.Notables)
                {
                    if (notableHero.IsGangLeader && CharacterRelationManager.GetHeroRelation(Hero.MainHero, notableHero) > relationCost)
                    {
                        args.IsEnabled = true;
                        return true;
                    }
                }
                args.IsEnabled = false;
                args.Tooltip = new TextObject("No Gang Leader notables in this settlmenet have a relation higher than " + relationCost + " with you.", null);
            }
            return true;
        }

        private bool game_menu_steal_encounter_giveback_on_condition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Escape;      //Option icon

            if (!isDetected)
            {
                return false;
            }

            return true;
        }

        //private bool game_menu_steal_encounter_callguards_on_condition(MenuCallbackArgs args)
        //{
        //    args.optionLeaveType = GameMenuOption.LeaveType.Surrender;      //Option icon
        //    return true;
        //}


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
            OnStealAttempt(Settlement.CurrentSettlement,isDetected,prosperityGoodsAmmount,stealQuantity);
        }

        private void game_menu_steal_encounter_persuade_on_consequence(MenuCallbackArgs args)
        {
            GiveGoldAction.ApplyForCharacterToSettlement(Hero.MainHero, Settlement.CurrentSettlement, KleptomaniaSubModule.settings.EncounterBribeCost);
            OnStealAttempt(Settlement.CurrentSettlement,isDetected, prosperityGoodsAmmount, stealQuantity);
        }

        private void game_menu_steal_encounter_influence_on_consequence(MenuCallbackArgs args)
        {
            Hero.MainHero.AddInfluenceWithKingdom(-KleptomaniaSubModule.settings.EncounterInfluenceCost);
            OnStealAttempt(Settlement.CurrentSettlement,isDetected, prosperityGoodsAmmount, stealQuantity);
        }

        private void game_menu_steal_threat_on_consequence(MenuCallbackArgs args)
        {
            ChangeCrimeRatingAction.Apply(Settlement.CurrentSettlement.MapFaction, 5, true);
            OnStealAttempt(Settlement.CurrentSettlement,isDetected, prosperityGoodsAmmount, stealQuantity);
        }

        private void game_menu_steal_giveback_on_consequence(MenuCallbackArgs args)
        {
            stealQuantity = 0;
            OnStealAttempt(Settlement.CurrentSettlement,isDetected, prosperityGoodsAmmount, stealQuantity);
        }

        //private void game_menu_steal_encounter_callguards_on_consequence(MenuCallbackArgs args)
        //{
        //    PartyBase.MainParty.AddElementToMemberRoster(CharacterObject.PlayerCharacter, -1, true);
        //    TakePrisonerAction.Apply(Settlement.CurrentSettlement.Party, Hero.MainHero);        //doesnt work. crashes and its a dumb solution
        //}


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

        public Dictionary<string, CampaignTime> _settlementLastStealDetectionTimeDictionary = new Dictionary<string, CampaignTime>();
        public Dictionary<IFaction, int> _recentFactionStealAttemptPenalty = new Dictionary<IFaction, int>();
        private StealSuppliesUtils stealUtils = new StealSuppliesUtils();
 

        int prosperityGoodsAmmount=0;
        CampaignTime GoalStealTime;

        int currentDetectionChance;
        int currentMinimunGoods;

        public bool isDetected;
        public int stealQuantity;
    }
}