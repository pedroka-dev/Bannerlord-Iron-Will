using System;
using System.Xml.Serialization;
using TaleWorlds.Library;

namespace xxKleptomania
{
    [Serializable()]
    public class ModuleSettings
    {
        [XmlElement("ID")]
        public string ID { get; set; } = "xxKleptomania";

        [XmlElement(DataType = "boolean", ElementName = "DebugInfo")]
        public bool DebugInfo { get; set; } = false;

        [XmlElement(DataType = "int", ElementName = "BaseDetectionChance")]
        public int BaseDetectionChance { get; set; } = 75;

        [XmlElement(DataType = "int", ElementName = "BaseMinimunGoods")]
        public int BaseMinimunGoods { get; set; } = 30;

        [XmlElement(DataType = "float", ElementName = "HoursWaitingToSteal")]
        public float HoursWaitingToSteal { get; set; } = 4;

        [XmlElement(DataType = "float", ElementName = "TownStealCrimeRating")]
        public float TownStealCrimeRating { get; set; } = 35f;

        [XmlElement(DataType = "float", ElementName = "VillageStealCrimeRating")]
        public float VillageStealCrimeRating { get; set; } = 30f;

        [XmlElement(DataType = "int", ElementName = "StealRelationPenalty")]
        public int StealRelationPenalty { get; set; } = -15;

        public string ModName
        {
            get {
                return "Iron Will - Kleptomania";
            }
        }

        public string ModuleFolderName
        {
            get {
                return "xxKleptomania";
            }
        }

        public string SettingsFilePath
        {
            get {
                return BasePath.Name + "Modules/" + this.ID + "/" + "xxKleptomaniaSettings.xml";
            }
        }

        public string LogFileTarget
        {
            get {
                return "KleptomaniaLogFile";
            }
        }

        public string LogFilePath
        {
            get {
                return BasePath.Name + "Modules/" + this.ID + "/" + "KleptomaniaLog.txt";
            }
        }
    }
}
