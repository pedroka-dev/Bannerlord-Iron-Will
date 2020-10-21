
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using System;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem;

namespace xxKleptomania
{
    public class StealSuppliesUtils
    {
        public string TextPrefixFromValue(int value)       //to add string prefixes to a given value
        {
            string message;
            if (value > 80)
            {
                message = "Very High";
            }
            else if (value > 60)
            {
                message = "High";
            }
            else if (value > 40)
            {
                message = "Medium";
            }
            else if (value > 20)
            {
                message = "Low";
            }
            else
            {
                message = "Very Low";
            }
            return message;
        }

        public int CalculateDetectionBonus(int skillBonus, bool isNight, int recentAtemptPenalty)      //current bonuses: Night = -10%, From Skill = Roguery LvL /5. current penalty: High crime rating = 15%
        {
            int detectionChanceBonus = KleptomaniaSubModule.settings.BaseDetectionChance;

            if (skillBonus > 50)        //Max detection bonus from roguery skill = -50%
            {
                skillBonus = 50;
            }
            detectionChanceBonus -= skillBonus;

            if (isNight)
            {
                detectionChanceBonus -= 10;
            }

            if(recentAtemptPenalty > 0)
            {
                detectionChanceBonus += recentAtemptPenalty;
            }

            if(detectionChanceBonus > 100)
            {
                detectionChanceBonus = 100;
            }

            return detectionChanceBonus;
        }

        public int CalculateLootBonus(int skillBonus)       //current bonuses: From Skill = Roguery LvL /10
        {
            int minimunGoodsBonus = KleptomaniaSubModule.settings.BaseMinimunGoods;

            if (skillBonus > 30)        //Max minimun goods bonus from roguery skill = +30%
            {
                skillBonus = 30;
            }
            minimunGoodsBonus += skillBonus;

            return minimunGoodsBonus;
        }

        public bool CalculateDetectionResult(int detectionChance)     //to be undetected, random number stealDetectionResult should be bigger than the currentDetectionChance
        {
            bool isDetectedResult;
            int stealDetectionResult = MBRandom.RandomInt(1, 100);

            if (stealDetectionResult >= detectionChance)
            {
                isDetectedResult = false;
            }
            else
            {
                isDetectedResult = true;
            }

            return isDetectedResult;
        }

        public int CalculateLootResult(int minimunGoods)      //get a random ammount of goods. but if its smaller than the currentMinimunGoods, gets currentMinimunGoods instead
        {
            int stealLootResult = MBRandom.RandomInt(1, 100);
            int lootQuantityResult;

            if (stealLootResult >= minimunGoods)
            {
                lootQuantityResult = stealLootResult;
            }
            else
            {
                lootQuantityResult = minimunGoods;
            }

            return lootQuantityResult;
        }

        public void GiveRogueryXp(Hero heroTroop)
        {
            float xpValue = KleptomaniaSubModule.settings.StealXpValue;
            DefaultCharacterDevelopmentModel characterDevelopmentModel = new DefaultCharacterDevelopmentModel();
            float learningRateBonus = characterDevelopmentModel.CalculateLearningRate(heroTroop, DefaultSkills.Roguery);

            heroTroop.AddSkillXp(DefaultSkills.Roguery, xpValue);

            if (KleptomaniaSubModule.settings.ReceivedXpInConsole)
            {
                InformationManager.DisplayMessage(new InformationMessage(heroTroop.Name + " received " + Math.Round(xpValue * learningRateBonus, 1) + " Roguery XP for steal attempt.", Colors.Yellow));
            }
            KleptomaniaSubModule.Log.Info("Main Hero | " + heroTroop.Name + " received Roguery XP value of " + Math.Round(xpValue * learningRateBonus, 1));
        }
    }
}
