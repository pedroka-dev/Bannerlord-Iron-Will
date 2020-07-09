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


        //[XmlElement(DataType = "boolean", ElementName = "ScalableSkillXp")]
        //public bool ScalableSkillXp { get; set; } = true;


        //[XmlElement(DataType = "int", ElementName = "TroopWoundXpValue")]
        //public int TroopWoundXpValue { get; set; } = 80;


        //[XmlElement(DataType = "int", ElementName = "HeroWoundXpValue")]
        //public int HeroWoundXpValue { get; set; } = 80;


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
