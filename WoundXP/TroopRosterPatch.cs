using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using xxWoundXP;

namespace xWoundXP
{
    [HarmonyPatch(typeof(TroopRoster), "WoundTroop")]
    public class TroopRosterPatch
    {
        static void Postfix(TroopRoster __instance, CharacterObject troop, int numberToWound, UniqueTroopDescriptor troopSeed)
        {
            try
            {
                if (troop.IsHero)
                {
                    int xpValue = WoundXpSubModule.settings.heroWoundXpValue;

                    Hero heroTroop = troop.HeroObject;
                    heroTroop.AddSkillXp(DefaultSkills.Athletics, xpValue);

                    if (troop.IsPlayerCharacter || heroTroop.IsPlayerCompanion || WoundXpSubModule.settings.debugInfo)
                    {
                        InformationManager.DisplayMessage(new InformationMessage(heroTroop.Name + " received " + xpValue + " Athletics XP for surviving after being wounded.", Colors.Yellow));
                        WoundXpSubModule.Log.Info("Hero Troop: " + troopSeed.ToString() + " | " + heroTroop.Name + " received Athletics XP value of " + xpValue);
                    }
                }
                else
                {
                    int xpValue = WoundXpSubModule.settings.troopWoundXpValue;

                    __instance.AddXpToTroop(xpValue, troop);

                    if (WoundXpSubModule.settings.debugInfo)
                    {
                        InformationManager.DisplayMessage(new InformationMessage(troop.Name + " received " + xpValue + " XP for surviving after being wounded.", Colors.Yellow));
                        WoundXpSubModule.Log.Info("Generic Troop: " + troopSeed.ToString() + " | " + troop.Name + " received XP value of " + xpValue);
                    }
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage("Something went wrong with Iron Will - Wound Experience. Please check the log file at \\bin\\Win64_Shipping_Client\\: " + WoundXpSubModule.settings.LogFilePath, Colors.Red)); 
                WoundXpSubModule.Log.Error(ex, "Error on Harmony Patch for WoundTroop.");
            }
        }
    }
}

