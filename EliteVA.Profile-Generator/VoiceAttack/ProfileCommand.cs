using System;

namespace EliteVA.ProfileGenerator.VoiceAttack
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ProfileCommand
    {
        public ProfileCommand()
        {
            Init();
        }

        public ProfileCommand(string command, string category = "")
        {
            Init();
            CommandString = command;
            Category = category;
        }

        private void Init()
        {
            ExecType = 3;
            BaseId = Guid.NewGuid().ToString();
            SessionEnabled = true;
            Id = Guid.NewGuid().ToString();
            Async = true;
            Enabled = true;
            keyPassthru = true;
            UseSpokenPhrase = true;
            RepeatNumber = 2;
            SourceProfile = Guid.NewGuid().ToString();
            ProcessOverrideActiveWindow = true;
            LostFocusBackCompat = true;
            MousePassThru = true;
        }

        private object referrerField;

        private byte execTypeField;

        private byte confidenceField;

        private byte prefixActionCountField;

        private bool isDynamicallyCreatedField;

        private bool targetProcessSetField;

        private byte targetProcessTypeField;

        private byte targetProcessLevelField;

        private byte compareTypeField;

        private bool execFromWildcardField;

        private bool isSubCommandField;

        private bool isOverrideField;

        private string baseIdField;

        private string originIdField;

        private bool sessionEnabledField;

        private bool doubleTapInvokedField;

        private bool singleTapDelayedInvokedField;

        private bool longTapInvokedField;

        private bool shortTapDelayedInvokedField;

        private byte sleepFlagField;

        private string idField;

        private string commandStringField;

        private object actionSequenceField;

        private bool asyncField;

        private bool enabledField;

        private string categoryField;

        private bool useShortcutField;

        private byte keyValueField;

        private byte keyShiftField;

        private byte keyAltField;

        private byte keyCtrlField;

        private byte keyWinField;

        private bool keyPassthruField;

        private bool useSpokenPhraseField;

        private bool onlyKeyUpField;

        private byte repeatNumberField;

        private byte repeatTypeField;

        private byte commandTypeField;

        private string sourceProfileField;

        private bool useConfidenceField;

        private byte minimumConfidenceLevelField;

        private bool useJoystickField;

        private byte joystickNumberField;

        private byte joystickButtonField;

        private byte joystickNumber2Field;

        private byte joystickButton2Field;

        private bool joystickUpField;

        private bool keepRepeatingField;

        private bool useProcessOverrideField;

        private bool processOverrideActiveWindowField;

        private bool lostFocusStopField;

        private bool pauseLostFocusField;

        private bool lostFocusBackCompatField;

        private bool useMouseField;

        private bool mouse1Field;

        private bool mouse2Field;

        private bool mouse3Field;

        private bool mouse4Field;

        private bool mouse5Field;

        private bool mouse6Field;

        private bool mouse7Field;

        private bool mouse8Field;

        private bool mouse9Field;

        private bool mouseUpOnlyField;

        private bool mousePassThruField;

        private bool joystickExclusiveField;

        private string lastEditedActionField;

        private bool useProfileProcessOverrideField;

        private bool profileProcessOverrideActiveWindowField;

        private bool repeatIfKeysDownField;

        private bool repeatIfMouseDownField;

        private bool repeatIfJoystickDownField;

        private byte ahField;

        private byte clField;

        private bool hasMBField;

        private bool useVariableHotkeyField;

        private byte cLEField;

        private bool eX1Field;

        private bool eX2Field;

        private object internalIdField;

        private bool hasInputField;

        private byte hotkeyDoubleTapLevelField;

        private byte mouseDoubleTapLevelField;

        private byte joystickDoubleTapLevelField;

        private byte hotkeyLongTapLevelField;

        private byte mouseLongTapLevelField;

        private byte joystickLongTapLevelField;

        private bool alwaysExecField;

        private byte resourceBalanceField;

        private bool preventExecField;

        private bool externalEventsEnabledField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object Referrer
        {
            get => referrerField;
            set => referrerField = value;
        }

        /// <remarks/>
        public byte ExecType
        {
            get => execTypeField;
            set => execTypeField = value;
        }

        /// <remarks/>
        public byte Confidence
        {
            get => confidenceField;
            set => confidenceField = value;
        }

        /// <remarks/>
        public byte PrefixActionCount
        {
            get => prefixActionCountField;
            set => prefixActionCountField = value;
        }

        /// <remarks/>
        public bool IsDynamicallyCreated
        {
            get => isDynamicallyCreatedField;
            set => isDynamicallyCreatedField = value;
        }

        /// <remarks/>
        public bool TargetProcessSet
        {
            get => targetProcessSetField;
            set => targetProcessSetField = value;
        }

        /// <remarks/>
        public byte TargetProcessType
        {
            get => targetProcessTypeField;
            set => targetProcessTypeField = value;
        }

        /// <remarks/>
        public byte TargetProcessLevel
        {
            get => targetProcessLevelField;
            set => targetProcessLevelField = value;
        }

        /// <remarks/>
        public byte CompareType
        {
            get => compareTypeField;
            set => compareTypeField = value;
        }

        /// <remarks/>
        public bool ExecFromWildcard
        {
            get => execFromWildcardField;
            set => execFromWildcardField = value;
        }

        /// <remarks/>
        public bool IsSubCommand
        {
            get => isSubCommandField;
            set => isSubCommandField = value;
        }

        /// <remarks/>
        public bool IsOverride
        {
            get => isOverrideField;
            set => isOverrideField = value;
        }

        /// <remarks/>
        public string BaseId
        {
            get => baseIdField;
            set => baseIdField = value;
        }

        /// <remarks/>
        public string OriginId
        {
            get => originIdField;
            set => originIdField = value;
        }

        /// <remarks/>
        public bool SessionEnabled
        {
            get => sessionEnabledField;
            set => sessionEnabledField = value;
        }

        /// <remarks/>
        public bool DoubleTapInvoked
        {
            get => doubleTapInvokedField;
            set => doubleTapInvokedField = value;
        }

        /// <remarks/>
        public bool SingleTapDelayedInvoked
        {
            get => singleTapDelayedInvokedField;
            set => singleTapDelayedInvokedField = value;
        }

        /// <remarks/>
        public bool LongTapInvoked
        {
            get => longTapInvokedField;
            set => longTapInvokedField = value;
        }

        /// <remarks/>
        public bool ShortTapDelayedInvoked
        {
            get => shortTapDelayedInvokedField;
            set => shortTapDelayedInvokedField = value;
        }

        /// <remarks/>
        public byte SleepFlag
        {
            get => sleepFlagField;
            set => sleepFlagField = value;
        }

        /// <remarks/>
        public string Id
        {
            get => idField;
            set => idField = value;
        }

        /// <remarks/>
        public string CommandString
        {
            get => commandStringField;
            set => commandStringField = value;
        }

        /// <remarks/>
        public object ActionSequence
        {
            get => actionSequenceField;
            set => actionSequenceField = value;
        }

        /// <remarks/>
        public bool Async
        {
            get => asyncField;
            set => asyncField = value;
        }

        /// <remarks/>
        public bool Enabled
        {
            get => enabledField;
            set => enabledField = value;
        }

        /// <remarks/>
        public string Category
        {
            get => categoryField;
            set => categoryField = value;
        }

        /// <remarks/>
        public bool UseShortcut
        {
            get => useShortcutField;
            set => useShortcutField = value;
        }

        /// <remarks/>
        public byte keyValue
        {
            get => keyValueField;
            set => keyValueField = value;
        }

        /// <remarks/>
        public byte keyShift
        {
            get => keyShiftField;
            set => keyShiftField = value;
        }

        /// <remarks/>
        public byte keyAlt
        {
            get => keyAltField;
            set => keyAltField = value;
        }

        /// <remarks/>
        public byte keyCtrl
        {
            get => keyCtrlField;
            set => keyCtrlField = value;
        }

        /// <remarks/>
        public byte keyWin
        {
            get => keyWinField;
            set => keyWinField = value;
        }

        /// <remarks/>
        public bool keyPassthru
        {
            get => keyPassthruField;
            set => keyPassthruField = value;
        }

        /// <remarks/>
        public bool UseSpokenPhrase
        {
            get => useSpokenPhraseField;
            set => useSpokenPhraseField = value;
        }

        /// <remarks/>
        public bool onlyKeyUp
        {
            get => onlyKeyUpField;
            set => onlyKeyUpField = value;
        }

        /// <remarks/>
        public byte RepeatNumber
        {
            get => repeatNumberField;
            set => repeatNumberField = value;
        }

        /// <remarks/>
        public byte RepeatType
        {
            get => repeatTypeField;
            set => repeatTypeField = value;
        }

        /// <remarks/>
        public byte CommandType
        {
            get => commandTypeField;
            set => commandTypeField = value;
        }

        /// <remarks/>
        public string SourceProfile
        {
            get => sourceProfileField;
            set => sourceProfileField = value;
        }

        /// <remarks/>
        public bool UseConfidence
        {
            get => useConfidenceField;
            set => useConfidenceField = value;
        }

        /// <remarks/>
        public byte minimumConfidenceLevel
        {
            get => minimumConfidenceLevelField;
            set => minimumConfidenceLevelField = value;
        }

        /// <remarks/>
        public bool UseJoystick
        {
            get => useJoystickField;
            set => useJoystickField = value;
        }

        /// <remarks/>
        public byte joystickNumber
        {
            get => joystickNumberField;
            set => joystickNumberField = value;
        }

        /// <remarks/>
        public byte joystickButton
        {
            get => joystickButtonField;
            set => joystickButtonField = value;
        }

        /// <remarks/>
        public byte joystickNumber2
        {
            get => joystickNumber2Field;
            set => joystickNumber2Field = value;
        }

        /// <remarks/>
        public byte joystickButton2
        {
            get => joystickButton2Field;
            set => joystickButton2Field = value;
        }

        /// <remarks/>
        public bool joystickUp
        {
            get => joystickUpField;
            set => joystickUpField = value;
        }

        /// <remarks/>
        public bool KeepRepeating
        {
            get => keepRepeatingField;
            set => keepRepeatingField = value;
        }

        /// <remarks/>
        public bool UseProcessOverride
        {
            get => useProcessOverrideField;
            set => useProcessOverrideField = value;
        }

        /// <remarks/>
        public bool ProcessOverrideActiveWindow
        {
            get => processOverrideActiveWindowField;
            set => processOverrideActiveWindowField = value;
        }

        /// <remarks/>
        public bool LostFocusStop
        {
            get => lostFocusStopField;
            set => lostFocusStopField = value;
        }

        /// <remarks/>
        public bool PauseLostFocus
        {
            get => pauseLostFocusField;
            set => pauseLostFocusField = value;
        }

        /// <remarks/>
        public bool LostFocusBackCompat
        {
            get => lostFocusBackCompatField;
            set => lostFocusBackCompatField = value;
        }

        /// <remarks/>
        public bool UseMouse
        {
            get => useMouseField;
            set => useMouseField = value;
        }

        /// <remarks/>
        public bool Mouse1
        {
            get => mouse1Field;
            set => mouse1Field = value;
        }

        /// <remarks/>
        public bool Mouse2
        {
            get => mouse2Field;
            set => mouse2Field = value;
        }

        /// <remarks/>
        public bool Mouse3
        {
            get => mouse3Field;
            set => mouse3Field = value;
        }

        /// <remarks/>
        public bool Mouse4
        {
            get => mouse4Field;
            set => mouse4Field = value;
        }

        /// <remarks/>
        public bool Mouse5
        {
            get => mouse5Field;
            set => mouse5Field = value;
        }

        /// <remarks/>
        public bool Mouse6
        {
            get => mouse6Field;
            set => mouse6Field = value;
        }

        /// <remarks/>
        public bool Mouse7
        {
            get => mouse7Field;
            set => mouse7Field = value;
        }

        /// <remarks/>
        public bool Mouse8
        {
            get => mouse8Field;
            set => mouse8Field = value;
        }

        /// <remarks/>
        public bool Mouse9
        {
            get => mouse9Field;
            set => mouse9Field = value;
        }

        /// <remarks/>
        public bool MouseUpOnly
        {
            get => mouseUpOnlyField;
            set => mouseUpOnlyField = value;
        }

        /// <remarks/>
        public bool MousePassThru
        {
            get => mousePassThruField;
            set => mousePassThruField = value;
        }

        /// <remarks/>
        public bool joystickExclusive
        {
            get => joystickExclusiveField;
            set => joystickExclusiveField = value;
        }

        /// <remarks/>
        public string lastEditedAction
        {
            get => lastEditedActionField;
            set => lastEditedActionField = value;
        }

        /// <remarks/>
        public bool UseProfileProcessOverride
        {
            get => useProfileProcessOverrideField;
            set => useProfileProcessOverrideField = value;
        }

        /// <remarks/>
        public bool ProfileProcessOverrideActiveWindow
        {
            get => profileProcessOverrideActiveWindowField;
            set => profileProcessOverrideActiveWindowField = value;
        }

        /// <remarks/>
        public bool RepeatIfKeysDown
        {
            get => repeatIfKeysDownField;
            set => repeatIfKeysDownField = value;
        }

        /// <remarks/>
        public bool RepeatIfMouseDown
        {
            get => repeatIfMouseDownField;
            set => repeatIfMouseDownField = value;
        }

        /// <remarks/>
        public bool RepeatIfJoystickDown
        {
            get => repeatIfJoystickDownField;
            set => repeatIfJoystickDownField = value;
        }

        /// <remarks/>
        public byte AH
        {
            get => ahField;
            set => ahField = value;
        }

        /// <remarks/>
        public byte CL
        {
            get => clField;
            set => clField = value;
        }

        /// <remarks/>
        public bool HasMB
        {
            get => hasMBField;
            set => hasMBField = value;
        }

        /// <remarks/>
        public bool UseVariableHotkey
        {
            get => useVariableHotkeyField;
            set => useVariableHotkeyField = value;
        }

        /// <remarks/>
        public byte CLE
        {
            get => cLEField;
            set => cLEField = value;
        }

        /// <remarks/>
        public bool EX1
        {
            get => eX1Field;
            set => eX1Field = value;
        }

        /// <remarks/>
        public bool EX2
        {
            get => eX2Field;
            set => eX2Field = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object InternalId
        {
            get => internalIdField;
            set => internalIdField = value;
        }

        /// <remarks/>
        public bool HasInput
        {
            get => hasInputField;
            set => hasInputField = value;
        }

        /// <remarks/>
        public byte HotkeyDoubleTapLevel
        {
            get => hotkeyDoubleTapLevelField;
            set => hotkeyDoubleTapLevelField = value;
        }

        /// <remarks/>
        public byte MouseDoubleTapLevel
        {
            get => mouseDoubleTapLevelField;
            set => mouseDoubleTapLevelField = value;
        }

        /// <remarks/>
        public byte JoystickDoubleTapLevel
        {
            get => joystickDoubleTapLevelField;
            set => joystickDoubleTapLevelField = value;
        }

        /// <remarks/>
        public byte HotkeyLongTapLevel
        {
            get => hotkeyLongTapLevelField;
            set => hotkeyLongTapLevelField = value;
        }

        /// <remarks/>
        public byte MouseLongTapLevel
        {
            get => mouseLongTapLevelField;
            set => mouseLongTapLevelField = value;
        }

        /// <remarks/>
        public byte JoystickLongTapLevel
        {
            get => joystickLongTapLevelField;
            set => joystickLongTapLevelField = value;
        }

        /// <remarks/>
        public bool AlwaysExec
        {
            get => alwaysExecField;
            set => alwaysExecField = value;
        }

        /// <remarks/>
        public byte ResourceBalance
        {
            get => resourceBalanceField;
            set => resourceBalanceField = value;
        }

        /// <remarks/>
        public bool PreventExec
        {
            get => preventExecField;
            set => preventExecField = value;
        }

        /// <remarks/>
        public bool ExternalEventsEnabled
        {
            get => externalEventsEnabledField;
            set => externalEventsEnabledField = value;
        }
    }
}