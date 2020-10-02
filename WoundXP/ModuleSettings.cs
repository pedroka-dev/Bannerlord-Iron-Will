using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Providers;
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


        private bool _debugInfo = false;
        private bool _scalableSkillXp = true;
        private bool _receivedXpInConsole = true;
        private int _troopWoundXpValue = 40;
        private int _heroWoundXpValue = 40;


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
        public bool DebugInfo
        {
            get => _debugInfo;
            set {
                if (_debugInfo != value)
                {
                    _debugInfo = value;
                    OnPropertyChanged();
                }
            }
        }
        

        [SettingProperty("Scalable Experience", RequireRestart = false, HintText = "The earned Experience will scale with Hero Learning Rate and Troop Tier.")]
        [SettingPropertyGroup("General")]
        [XmlElement(DataType = "boolean", ElementName = "ScalableSkillXp")]
        public bool ScalableSkillXp
        {
            get => _scalableSkillXp;
            set {
                if (_scalableSkillXp != value)
                {
                    _scalableSkillXp = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Received Experience In Console", RequireRestart = false, HintText = "Shows informational messages in the console when a Hero or Troop receives experience.")]
        [SettingPropertyGroup("General")]
        [XmlElement(DataType = "boolean", ElementName = "ReceivedXpInConsole")]
        public bool ReceivedXpInConsole
        {
            get => _receivedXpInConsole;
            set {
                if (_receivedXpInConsole != value)
                {
                    _receivedXpInConsole = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Troop Wound Experience", minValue: 0, maxValue: 500, RequireRestart = false, HintText = "Generic Experience value received by troops after surviving getting wounded. Scales by xpValue * learningRateBonus if 'Scalable Experience Values' is true..")]
        [SettingPropertyGroup("Experience Values")]
        [XmlElement(DataType = "int", ElementName = "TroopWoundXpValue")]
        public int TroopWoundXpValue
        {
            get => _troopWoundXpValue;
            set {
                if (_troopWoundXpValue != value)
                {
                    _troopWoundXpValue = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Hero Wound Experience", minValue: 0, maxValue: 500, RequireRestart = false, HintText = "Athletics Experience value received by troops after surviving getting wounded. Scales by xpValue * (troop.Tier +1) if 'Scalable Experience Values' is true.")]
        [SettingPropertyGroup("Experience Values")]
        [XmlElement(DataType = "int", ElementName = "HeroWoundXpValue")]
        public int HeroWoundXpValue
        {
            get => _heroWoundXpValue;
            set {
                if (_heroWoundXpValue != value)
                {
                    _heroWoundXpValue = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
