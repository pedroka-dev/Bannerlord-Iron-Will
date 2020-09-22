using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Settings.Base.Global;
using System;
using System.Xml.Serialization;
using TaleWorlds.Library;

namespace xxWoundXP
{
    [Serializable()]
    public class ModuleSettings : AttributeGlobalSettings<ModuleSettings>
    {
        public override string Id { get; } = "xxWoundXp";
        public override string DisplayName { get; } = "Iron Will - Wound Experience";
        public override string FolderName { get; } = "xxWoundXp";
        public string LogFileTarget { get; } = "WoundExperienceLogFile";
        public string SettingsFilePath
        {
            get {
                return BasePath.Name + "Modules/" + this.FolderName + "/" + "xxWoundXPSettings.xml";
            }
        }
        public string LogFilePath
        {
            get {
                return BasePath.Name + "Modules/" + this.FolderName + "/" + "WoundExperienceLog.txt";
            }
        }

        [SettingProperty("DebugInfo", RequireRestart = false, HintText = "Shows information many of the operations of the mod. Used to debugging.")]
        [SettingPropertyGroup("General")]
        [XmlElement(DataType = "boolean", ElementName = "DebugInfo")]
        public bool DebugInfo { get; set; } = false;

        [SettingProperty("Scalable Experience", RequireRestart = false, HintText = "The earned Experience will scale with Hero Learning Rate and Troop Tier.")]
        [SettingPropertyGroup("General")]
        [XmlElement(DataType = "boolean", ElementName = "ScalableSkillXp")]
        public bool ScalableSkillXp { get; set; } = true;

        [SettingProperty("Received Experience In Console", RequireRestart = false, HintText = "Shows informational messages in the console when a Hero or Troop receives experience.")]
        [SettingPropertyGroup("General")]
        [XmlElement(DataType = "boolean", ElementName = "ReceivedXpInConsole")]
        public bool ReceivedXpInConsole { get; set; } = true;

        [SettingProperty("Troop Wound Experience", minValue: 0, maxValue: 1000, RequireRestart = true, HintText = "Generic Experience value received by troops after surviving getting wounded. Scales by xpValue * learningRateBonus if 'Scalable Experience Values' is true..")]
        [SettingPropertyGroup("Experience Values")]
        [XmlElement(DataType = "int", ElementName = "TroopWoundXpValue")]
        public int TroopWoundXpValue { get; set; } = 40;

        [SettingProperty("Hero Wound Experience", minValue: 0, maxValue: 1000, RequireRestart = true, HintText = "Athletics Experience value received by troops after surviving getting wounded. Scales by xpValue * (troop.Tier +1) if 'Scalable Experience Values' is true.")]
        [SettingPropertyGroup("Experience Values")]
        [XmlElement(DataType = "int", ElementName = "HeroWoundXpValue")]
        public int HeroWoundXpValue { get; set; } = 40;
    }
}
