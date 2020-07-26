
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;

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

        public int CalculateDetectionBonus(int skillBonus, bool isNight)      //current bonuses: Night = -10%, From Skill = Roguery LvL /5. current penalty: Moderate crime rating = +10%, Severe crime rating = +20%
        {
            int detectionChanceBonus = KleptomaniaSubModule.settings.BaseDetectionChance;

            //DefaultCrimeModel crimeModel = new DefaultCrimeModel();
            //if (crimeModel.IsPlayerCrimeRatingModerate(Settlement.CurrentSettlement.MapFaction))
            //{
            //    detectionChanceBonus = detectionChanceBonus + 10;
            //}
            //else if (crimeModel.IsPlayerCrimeRatingSevere(Settlement.CurrentSettlement.MapFaction))
            //{
            //    detectionChanceBonus = detectionChanceBonus + 20;
            //}

            if (skillBonus > 50)        //Max detection bonus from roguery skill = -50%
            {
                skillBonus = 50;
            }
            detectionChanceBonus = detectionChanceBonus - skillBonus;

            if (isNight)
            {
                detectionChanceBonus = detectionChanceBonus - 10;
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
            minimunGoodsBonus = minimunGoodsBonus + skillBonus;

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
    }
}
