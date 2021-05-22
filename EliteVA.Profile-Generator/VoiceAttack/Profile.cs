using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteVA.ProfileGenerator.VoiceAttack
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Profile
    {
        public Profile()
        {
            Id = Guid.NewGuid().ToString();
            Name = "EliteVA commands";
            Commands = new ProfileCommand[1];
            ExportVAVersion = "1.8.7";
            ExportOSVersionMajor = 10;
            ExportOSVersionMinor = 0;
            ProcessOverrideAciveWindow = true;
            InternalID = Guid.NewGuid().ToString();
        }

        public void AddCommand(ProfileCommand command)
        {
            command.AddAction(new CommandAction());
            commandsField.Add(command);
        }

        private bool hasMBField;

        private string idField;

        private string nameField;

        private IList<ProfileCommand> commandsField;

        private bool overrideGlobalField;

        private byte globalHotkeyIndexField;

        private bool globalHotkeyEnabledField;

        private byte globalHotkeyValueField;

        private byte globalHotkeyShiftField;

        private byte globalHotkeyAltField;

        private byte globalHotkeyCtrlField;

        private byte globalHotkeyWinField;

        private bool globalHotkeyPassThruField;

        private bool overrideMouseField;

        private byte mouseIndexField;

        private bool overrideStopField;

        private bool stopCommandHotkeyEnabledField;

        private byte stopCommandHotkeyValueField;

        private byte stopCommandHotkeyShiftField;

        private byte stopCommandHotkeyAltField;

        private byte stopCommandHotkeyCtrlField;

        private byte stopCommandHotkeyWinField;

        private bool stopCommandHotkeyPassThruField;

        private bool disableShortcutsField;

        private bool useOverrideListeningField;

        private bool overrideJoystickGlobalField;

        private byte globalJoystickIndexField;

        private byte globalJoystickButtonField;

        private byte globalJoystickNumberField;

        private byte globalJoystickButton2Field;

        private byte globalJoystickNumber2Field;

        private object referencedProfileField;

        private string exportVAVersionField;

        private byte exportOSVersionMajorField;

        private byte exportOSVersionMinorField;

        private bool overrideConfidenceField;

        private byte confidenceField;

        private bool catchAllEnabledField;

        private object catchAllIdField;

        private bool initializeCommandEnabledField;

        private object initializeCommandIdField;

        private bool useProcessOverrideField;

        private bool processOverrideAciveWindowField;

        private bool dictationCommandEnabledField;

        private object dictationCommandIdField;

        private bool enableProfileSwitchField;

        private object categoryGroupsField;

        private bool groupCategoryField;

        private string lastEditedCommandField;

        private byte isField;

        private byte ioField;

        private byte ipField;

        private byte beField;

        private bool unloadCommandEnabledField;

        private object unloadCommandIdField;

        private bool blockExternalField;

        private object authorIDField;

        private object productIDField;

        private byte crField;

        private string internalIDField;

        private byte prField;

        private byte coField;

        private byte opField;

        private byte cvField;

        private byte pdField;

        private byte peField;

        /// <remarks/>
        public bool HasMB
        {
            get => hasMBField;
            set => hasMBField = value;
        }

        /// <remarks/>
        public string Id
        {
            get => idField;
            set => idField = value;
        }

        /// <remarks/>
        public string Name
        {
            get => nameField;
            set => nameField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Command", IsNullable = false)]
        public ProfileCommand[] Commands
        {
            get => commandsField.ToArray();
            set => commandsField = value.ToList();
        }

        /// <remarks/>
        public bool OverrideGlobal
        {
            get => overrideGlobalField;
            set => overrideGlobalField = value;
        }

        /// <remarks/>
        public byte GlobalHotkeyIndex
        {
            get => globalHotkeyIndexField;
            set => globalHotkeyIndexField = value;
        }

        /// <remarks/>
        public bool GlobalHotkeyEnabled
        {
            get => globalHotkeyEnabledField;
            set => globalHotkeyEnabledField = value;
        }

        /// <remarks/>
        public byte GlobalHotkeyValue
        {
            get => globalHotkeyValueField;
            set => globalHotkeyValueField = value;
        }

        /// <remarks/>
        public byte GlobalHotkeyShift
        {
            get => globalHotkeyShiftField;
            set => globalHotkeyShiftField = value;
        }

        /// <remarks/>
        public byte GlobalHotkeyAlt
        {
            get => globalHotkeyAltField;
            set => globalHotkeyAltField = value;
        }

        /// <remarks/>
        public byte GlobalHotkeyCtrl
        {
            get => globalHotkeyCtrlField;
            set => globalHotkeyCtrlField = value;
        }

        /// <remarks/>
        public byte GlobalHotkeyWin
        {
            get => globalHotkeyWinField;
            set => globalHotkeyWinField = value;
        }

        /// <remarks/>
        public bool GlobalHotkeyPassThru
        {
            get => globalHotkeyPassThruField;
            set => globalHotkeyPassThruField = value;
        }

        /// <remarks/>
        public bool OverrideMouse
        {
            get => overrideMouseField;
            set => overrideMouseField = value;
        }

        /// <remarks/>
        public byte MouseIndex
        {
            get => mouseIndexField;
            set => mouseIndexField = value;
        }

        /// <remarks/>
        public bool OverrideStop
        {
            get => overrideStopField;
            set => overrideStopField = value;
        }

        /// <remarks/>
        public bool StopCommandHotkeyEnabled
        {
            get => stopCommandHotkeyEnabledField;
            set => stopCommandHotkeyEnabledField = value;
        }

        /// <remarks/>
        public byte StopCommandHotkeyValue
        {
            get => stopCommandHotkeyValueField;
            set => stopCommandHotkeyValueField = value;
        }

        /// <remarks/>
        public byte StopCommandHotkeyShift
        {
            get => stopCommandHotkeyShiftField;
            set => stopCommandHotkeyShiftField = value;
        }

        /// <remarks/>
        public byte StopCommandHotkeyAlt
        {
            get => stopCommandHotkeyAltField;
            set => stopCommandHotkeyAltField = value;
        }

        /// <remarks/>
        public byte StopCommandHotkeyCtrl
        {
            get => stopCommandHotkeyCtrlField;
            set => stopCommandHotkeyCtrlField = value;
        }

        /// <remarks/>
        public byte StopCommandHotkeyWin
        {
            get => stopCommandHotkeyWinField;
            set => stopCommandHotkeyWinField = value;
        }

        /// <remarks/>
        public bool StopCommandHotkeyPassThru
        {
            get => stopCommandHotkeyPassThruField;
            set => stopCommandHotkeyPassThruField = value;
        }

        /// <remarks/>
        public bool DisableShortcuts
        {
            get => disableShortcutsField;
            set => disableShortcutsField = value;
        }

        /// <remarks/>
        public bool UseOverrideListening
        {
            get => useOverrideListeningField;
            set => useOverrideListeningField = value;
        }

        /// <remarks/>
        public bool OverrideJoystickGlobal
        {
            get => overrideJoystickGlobalField;
            set => overrideJoystickGlobalField = value;
        }

        /// <remarks/>
        public byte GlobalJoystickIndex
        {
            get => globalJoystickIndexField;
            set => globalJoystickIndexField = value;
        }

        /// <remarks/>
        public byte GlobalJoystickButton
        {
            get => globalJoystickButtonField;
            set => globalJoystickButtonField = value;
        }

        /// <remarks/>
        public byte GlobalJoystickNumber
        {
            get => globalJoystickNumberField;
            set => globalJoystickNumberField = value;
        }

        /// <remarks/>
        public byte GlobalJoystickButton2
        {
            get => globalJoystickButton2Field;
            set => globalJoystickButton2Field = value;
        }

        /// <remarks/>
        public byte GlobalJoystickNumber2
        {
            get => globalJoystickNumber2Field;
            set => globalJoystickNumber2Field = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object ReferencedProfile
        {
            get => referencedProfileField;
            set => referencedProfileField = value;
        }

        /// <remarks/>
        public string ExportVAVersion
        {
            get => exportVAVersionField;
            set => exportVAVersionField = value;
        }

        /// <remarks/>
        public byte ExportOSVersionMajor
        {
            get => exportOSVersionMajorField;
            set => exportOSVersionMajorField = value;
        }

        /// <remarks/>
        public byte ExportOSVersionMinor
        {
            get => exportOSVersionMinorField;
            set => exportOSVersionMinorField = value;
        }

        /// <remarks/>
        public bool OverrideConfidence
        {
            get => overrideConfidenceField;
            set => overrideConfidenceField = value;
        }

        /// <remarks/>
        public byte Confidence
        {
            get => confidenceField;
            set => confidenceField = value;
        }

        /// <remarks/>
        public bool CatchAllEnabled
        {
            get => catchAllEnabledField;
            set => catchAllEnabledField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object CatchAllId
        {
            get => catchAllIdField;
            set => catchAllIdField = value;
        }

        /// <remarks/>
        public bool InitializeCommandEnabled
        {
            get => initializeCommandEnabledField;
            set => initializeCommandEnabledField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object InitializeCommandId
        {
            get => initializeCommandIdField;
            set => initializeCommandIdField = value;
        }

        /// <remarks/>
        public bool UseProcessOverride
        {
            get => useProcessOverrideField;
            set => useProcessOverrideField = value;
        }

        /// <remarks/>
        public bool ProcessOverrideAciveWindow
        {
            get => processOverrideAciveWindowField;
            set => processOverrideAciveWindowField = value;
        }

        /// <remarks/>
        public bool DictationCommandEnabled
        {
            get => dictationCommandEnabledField;
            set => dictationCommandEnabledField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object DictationCommandId
        {
            get => dictationCommandIdField;
            set => dictationCommandIdField = value;
        }

        /// <remarks/>
        public bool EnableProfileSwitch
        {
            get => enableProfileSwitchField;
            set => enableProfileSwitchField = value;
        }

        /// <remarks/>
        public object CategoryGroups
        {
            get => categoryGroupsField;
            set => categoryGroupsField = value;
        }

        /// <remarks/>
        public bool GroupCategory
        {
            get => groupCategoryField;
            set => groupCategoryField = value;
        }

        /// <remarks/>
        public string LastEditedCommand
        {
            get => lastEditedCommandField;
            set => lastEditedCommandField = value;
        }

        /// <remarks/>
        public byte IS
        {
            get => isField;
            set => isField = value;
        }

        /// <remarks/>
        public byte IO
        {
            get => ioField;
            set => ioField = value;
        }

        /// <remarks/>
        public byte IP
        {
            get => ipField;
            set => ipField = value;
        }

        /// <remarks/>
        public byte BE
        {
            get => beField;
            set => beField = value;
        }

        /// <remarks/>
        public bool UnloadCommandEnabled
        {
            get => unloadCommandEnabledField;
            set => unloadCommandEnabledField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object UnloadCommandId
        {
            get => unloadCommandIdField;
            set => unloadCommandIdField = value;
        }

        /// <remarks/>
        public bool BlockExternal
        {
            get => blockExternalField;
            set => blockExternalField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object AuthorID
        {
            get => authorIDField;
            set => authorIDField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object ProductID
        {
            get => productIDField;
            set => productIDField = value;
        }

        /// <remarks/>
        public byte CR
        {
            get => crField;
            set => crField = value;
        }

        /// <remarks/>
        public string InternalID
        {
            get => internalIDField;
            set => internalIDField = value;
        }

        /// <remarks/>
        public byte PR
        {
            get => prField;
            set => prField = value;
        }

        /// <remarks/>
        public byte CO
        {
            get => coField;
            set => coField = value;
        }

        /// <remarks/>
        public byte OP
        {
            get => opField;
            set => opField = value;
        }

        /// <remarks/>
        public byte CV
        {
            get => cvField;
            set => cvField = value;
        }

        /// <remarks/>
        public byte PD
        {
            get => pdField;
            set => pdField = value;
        }

        /// <remarks/>
        public byte PE
        {
            get => peField;
            set => peField = value;
        }
    }
}