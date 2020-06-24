using System;
using System.Xml.Serialization;
using TaleWorlds.Library;

namespace xxWoundXP
{
    [Serializable()]
    public class ModuleSettings
    {
        [XmlElement("ID")]
        public string ID { get; set; } = "xxWoundXP";

        [XmlElement(DataType = "boolean", ElementName = "DebugInfo")]
        public bool DebugInfo { get; set; } = false;


        [XmlElement(DataType = "boolean", ElementName = "ScalableSkillXp")]
        public bool ScalableSkillXp { get; set; } = true;


        [XmlElement(DataType = "int", ElementName = "TroopWoundXpValue")]
        public int TroopWoundXpValue { get; set; } = 80;


        [XmlElement(DataType = "int", ElementName = "HeroWoundXpValue")]
        public int HeroWoundXpValue { get; set; } = 80;


        public string ModName
        {
            get {
                return "Iron Will - Wound Experience";
            }
        }

        public string ModuleFolderName
        {
            get {
                return "xxWoundXp";
            }
        }

        public string SettingsFilePath
        {
            get {
                return BasePath.Name + "Modules/" + this.ID + "/" + "xxWoundXPSettings.xml";
            }
        }

        public string LogFileTarget
        {
            get {
                return "WoundExperienceLogFile";
            }
        }

        public string LogFilePath
        {
            get {
                return BasePath.Name + "Modules/" + this.ID + "/" + "WoundExperienceLog.txt";
            }
        }
    }
}
