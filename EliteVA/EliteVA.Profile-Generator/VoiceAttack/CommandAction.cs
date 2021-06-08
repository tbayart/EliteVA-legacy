using System;

namespace EliteVA.ProfileGenerator.VoiceAttack
{
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public class CommandAction
    {
	    public CommandAction()
	    {
		    Id = Guid.NewGuid().ToString();
		    ActionType = "InternalProcess_Ignore";
	    }
	    
	    /// <remarks/>
	    public string PairingSet { get; set; }

	    /// <remarks/>
		public string PairingSetElse { get; set; }
	    
	    /// <remarks/>
		public string Ordinal { get; set; }

	    /// <remarks/>
		public string ConditionMet { get; set; }
	
		public string IndentLevel { get; set; }

		public string ConditionSkip { get; set; }
	
		public string IsSuffixAction { get; set; }

		public string DecimalTransient1 { get; set; }
	
		public string Id { get; set; }

		public string ActionType { get; set; }
	
		public string Duration { get; set; }

		public string Delay { get; set; }
	
		public string KeyCodes { get; set; }

		public string X { get; set; }
	
		public string Y { get; set; }

		public string Z { get; set; }
	
		public string InputMode { get; set; }
	
		public string ConditionPairing { get; set; }
		
		public string ConditionGroup { get; set; }

		public string ConditionStartOperator { get; set; }
	
		public string ConditionStartValue { get; set; }

		public string ConditionStartValueType { get; set; }
		
		public string ConditionStartType { get; set; }
	
		public string DecimalContext1 { get; set; }
	
		public string DecimalContext2 { get; set; }
		
		public string DateContext1 { get; set; }

		public string DateContext2 { get; set; }
		
		public string Disabled { get; set; }
		
		public string RandomSounds { get; set; }

		public string ConditionExpressions { get; set; }
    }
}