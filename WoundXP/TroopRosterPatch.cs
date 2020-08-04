using System;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace xxWoundXP
{
    [HarmonyPatch(typeof(TroopRoster), "WoundTroop")]
    public class TroopRosterPatch
    {
        static void Postfix(TroopRoster __instance, CharacterObject troop, int numberToWound, UniqueTroopDescriptor troopSeed)
        {
            try
            {
                //Reflection gimmicks to get "internal PartyBase OwnerParty" from TroopRoster 
                PropertyInfo prop = __instance.GetType().GetProperty("OwnerParty", BindingFlags.NonPublic | BindingFlags.Instance);
                PartyBase OwnerParty = (PartyBase)prop.GetValue(__instance);

                if (troop.IsHero)
                {
                    float xpValue = WoundXpSubModule.settings.HeroWoundXpValue;

                    Hero heroTroop = troop.HeroObject;

                    DefaultCharacterDevelopmentModel characterDevelopmentModel = new DefaultCharacterDevelopmentModel();
                    float learningRateBonus = characterDevelopmentModel.CalculateLearningRate(heroTroop, DefaultSkills.Athletics);

                    xpValue = xpValue * learningRateBonus;
                    heroTroop.AddSkillXp(DefaultSkills.Athletics, xpValue);

                    if (WoundXpSubModule.settings.DebugInfo || troop.IsPlayerCharacter || heroTroop.IsPlayerCompanion)
                    {
                        WoundXpSubModule.Log.Info("Hero Troop: " + troopSeed.ToString() + " | Calculated learning rate bonus = " + learningRateBonus.ToString());
                        if (WoundXpSubModule.settings.DebugInfo)
                        {
                            InformationManager.DisplayMessage(new InformationMessage(heroTroop.Name + " calculated learning rate bonus = " + learningRateBonus.ToString(), Colors.Yellow));
                        }

                        InformationManager.DisplayMessage(new InformationMessage(heroTroop.Name + " received " + xpValue + " Athletics XP for surviving after being wounded.", Colors.Yellow));
                        WoundXpSubModule.Log.Info("Hero Troop: " + troopSeed.ToString() + " | " + heroTroop.Name + " received Athletics XP value of " + xpValue);
                    }
                }
                else
                {
                    int xpValue = WoundXpSubModule.settings.TroopWoundXpValue;

                    int troopTierBonus = troop.Tier+1;

                    xpValue = xpValue * troopTierBonus;
                    __instance.AddXpToTroop(xpValue, troop);
                    
                    if (WoundXpSubModule.settings.DebugInfo || OwnerParty.Owner != null && OwnerParty.Owner.IsHumanPlayerCharacter)
                    {
                        WoundXpSubModule.Log.Info("Generic Troop: " + troopSeed.ToString() + " | Calculated learning troop tier bonus = " + troopTierBonus.ToString());
                        if (WoundXpSubModule.settings.DebugInfo)
                        {
                            InformationManager.DisplayMessage(new InformationMessage(troop.Name + " calculated troop tier bonus = " + troopTierBonus.ToString(), Colors.Yellow));
                        }

                        InformationManager.DisplayMessage(new InformationMessage(troop.Name + " received " + xpValue + " XP for surviving after being wounded.", Colors.Yellow));
                        WoundXpSubModule.Log.Info("Generic Troop: " + troopSeed.ToString() + " | " + troop.Name + " received XP value of " + xpValue);
                    }
                }
            }
            catch (Exception ex)
            {
                WoundXpSubModule.Log.Error("Error on Harmony Patch for WoundTroop. | " + ex.Message);
                InformationManager.DisplayMessage(new InformationMessage("Something went wrong with Iron Will - Wound Experience: " + ex.Message, Colors.Red)); 
            }
        }
    }
}

