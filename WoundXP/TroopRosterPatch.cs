using System;
using HarmonyLib;
using Helpers;
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
            if (troop.IsHero)
            {
                int xpValue = WoundXpSubModule.heroWoundXpValue;

                Hero heroTroop = troop.HeroObject;
                heroTroop.AddSkillXp(DefaultSkills.Athletics, xpValue);

                if (troop.IsPlayerCharacter || heroTroop.IsPlayerCompanion || WoundXpSubModule.debugInfo)
                {
                    InformationManager.DisplayMessage(new InformationMessage(heroTroop.Name + "received " + xpValue + " Athletics XP for survinving after being wounded."));
                }
            }
            else
            {
                int xpValue = WoundXpSubModule.troopWoundXpValue;

                __instance.AddXpToTroop(xpValue, troop);

                if(WoundXpSubModule.debugInfo)
                {
                    InformationManager.DisplayMessage(new InformationMessage(troop.Name + " received " + xpValue + " XP for survinving after being wounded."));
                }
            }
        }
    }
}

