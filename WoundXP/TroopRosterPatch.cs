using System;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
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
                    int xpValue = WoundXpSubModule.settings.HeroWoundXpValue;

                    Hero heroTroop = troop.HeroObject;
                    heroTroop.AddSkillXp(DefaultSkills.Athletics, xpValue);

                    if (troop.IsPlayerCharacter || heroTroop.IsPlayerCompanion || WoundXpSubModule.settings.DebugInfo)
                    {
                        InformationManager.DisplayMessage(new InformationMessage(heroTroop.Name + " received " + xpValue + " Athletics XP for surviving after being wounded.", Colors.Yellow));
                        WoundXpSubModule.Log.Info("Hero Troop: " + troopSeed.ToString() + " | " + heroTroop.Name + " received Athletics XP value of " + xpValue);
                    }
                }
                else
                {
                    int xpValue = WoundXpSubModule.settings.TroopWoundXpValue;

                    __instance.AddXpToTroop(xpValue, troop);

                    if (WoundXpSubModule.settings.DebugInfo || OwnerParty.Owner != null && OwnerParty.Owner.IsHumanPlayerCharacter)
                    {
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

