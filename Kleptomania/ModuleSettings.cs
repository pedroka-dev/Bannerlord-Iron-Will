using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using HarmonyLib;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Providers;
using NLog;
using NLog.Config;
using NLog.Targets;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;


namespace xxKleptomania
{
    [Serializable()]
    public class ModuleSettings : AttributeGlobalSettings<ModuleSettings>
    {
        public override string Id { get; } = "xxKleptomania";
        public override string DisplayName { get; } = "Iron Will - Kleptomania";
        public override string FolderName { get; } = "xxKleptomania";
        public string LogFileTarget { get; } = "KleptomaniaLogFile";


        private bool _debugInfo = false;
        private int _baseDetectionChance = 75;
        private int _baseMinimunGoods = 30;
        private float _hoursWaitingToSteal = 4;
        private float _townStealCrimeRating = 35f;
        private float _villageStealCrimeRating = 30f;
        private int _stealRelationPenalty = -15;
        private int _encounterBribeCost = 150;
        private int _encounterInfluenceCost = 2;
        private bool _receivedXpInConsole = true;


        public string SettingsFilePath
        {
            get {
                return BasePath.Name + "Modules/" + this.Id + "/" + "xxKleptomaniaSettings.xml";
            }
        }

        public string LogFilePath
        {
            get {
                return BasePath.Name + "Modules/" + this.Id + "/" + "KleptomaniaLog.txt";
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

        [SettingProperty("Base Detection Chance", minValue: 20, maxValue: 100, RequireRestart = false, HintText = "Determines the base chance of getting detected stealing. If the RNG detection is smaller than this modifier you get detected while stealling and will suffer consequences.")]
        [SettingPropertyGroup("Modifiers")]
        [XmlElement(DataType = "int", ElementName = "HeroWoundXpValue")]
        [XmlElement(DataType = "int", ElementName = "BaseDetectionChance")]
        public int BaseDetectionChance
        {
            get => _baseDetectionChance;
            set {
                if (_baseDetectionChance != value)
                {
                    _baseDetectionChance = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Base Minimun Goods", minValue: 0, maxValue: 70, RequireRestart = false, HintText = "Represent a porcentage of the items inside the settlement storage. Actual storage scales with settlement hearth / prosperity. The  ammount of goods will never go bellow this modifier. ")]
        [SettingPropertyGroup("Modifiers")]
        [XmlElement(DataType = "int", ElementName = "BaseMinimunGoods")]
        public int BaseMinimunGoods
        {
            get => _baseMinimunGoods;
            set {
                if (_baseMinimunGoods != value)
                {
                    _baseMinimunGoods = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Hours Waiting To Steal", minValue: 1f, maxValue: 24f, RequireRestart = false, HintText = "Hours wanting to find a opportunity to steal in any settlement.")]
        [SettingPropertyGroup("Modifiers")]
        [XmlElement(DataType = "float", ElementName = "HoursWaitingToSteal")]
        public float HoursWaitingToSteal
        {
            get => _hoursWaitingToSteal;
            set {
                if (_hoursWaitingToSteal != value)
                {
                    _hoursWaitingToSteal = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Town Steal Crime Rating", minValue: 1f, maxValue: 100f, RequireRestart = false, HintText = "Crime rate that increases with Town's Faction when caught stealing.")]
        [SettingPropertyGroup("Consequences")]
        [XmlElement(DataType = "float", ElementName = "TownStealCrimeRating")]
        public float TownStealCrimeRating
        {
            get => _townStealCrimeRating;
            set {
                if (_townStealCrimeRating != value)
                {
                    _townStealCrimeRating = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Village Steal Crime Rating", minValue: 1f, maxValue: 100f, RequireRestart = false, HintText = "Crime rate that increases with Village's Faction when caught stealing.")]
        [SettingPropertyGroup("Consequences")]
        [XmlElement(DataType = "float", ElementName = "VillageStealCrimeRating")]
        public float VillageStealCrimeRating
        {
            get => _villageStealCrimeRating;
            set {
                if (_villageStealCrimeRating != value)
                {
                    _villageStealCrimeRating = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Steal Relation Penalty", minValue: -100, maxValue: -1, RequireRestart = false, HintText = "How many points relation will decrease with settlement notables on detection (does not apply to Gang Leaders). If stealing from own faction, doubles and also applies to Leader.")]
        [SettingPropertyGroup("Consequences")]
        [XmlElement(DataType = "int", ElementName = "StealRelationPenalty")]
        public int StealRelationPenalty
        {
            get => _stealRelationPenalty;
            set {
                if (_stealRelationPenalty != value)
                {
                    _stealRelationPenalty = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Encounter Bribe Cost", minValue: 1, maxValue: 1500, RequireRestart = true, HintText = "The ammount of Dennars spent on Bribe after getting caught. Bribing is one of the choices to get away after being detected.")]
        [SettingPropertyGroup("Consequences")]
        [XmlElement(DataType = "int", ElementName = "EncounterBribeCost")]
        public int EncounterBribeCost
        {
            get => _encounterBribeCost;
            set {
                if (_encounterBribeCost != value)
                {
                    _encounterBribeCost = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Encounter Influence Cost", minValue: 1, maxValue: 10, RequireRestart = true, HintText = "The ammount of Influence spent to Persuade after getting caught. Persuading is one of the choices to get away after being detected.")]
        [SettingPropertyGroup("Consequences")]
        [XmlElement(DataType = "int", ElementName = "EncounterInfluenceCost")]
        public int EncounterInfluenceCost
        {
            get => _encounterInfluenceCost;
            set {
                if (_encounterInfluenceCost != value)
                {
                    _encounterInfluenceCost = value;
                    OnPropertyChanged();
                }
            }
        }

        [SettingProperty("Received Experience In Console", RequireRestart = false, HintText = "Shows informational messages in the console when the player receives experience.")]
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
    }
}
