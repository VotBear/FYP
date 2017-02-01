using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class C : MonoBehaviour {

	public static float P_START_HEIGHT = -12.5f;
	public static float P_DISTANCE = 22f;

	public static string EVENT_LABEL = "Event";
 
	public static string SINGLE_TOOLTIP =
		"{0} must fulfill the rule:";

	public static string CHANCE_TEXT = 
		"Probability: {0}/{1} ({2}%)";

	public static string SAMPLESPACE_TEXT = 
		"{0} Possible Combinations";

	//--------------------------------------------------------------------------------------------------

	public static string TYPE_LABEL = "Type";
	public static List<string> TYPE_OPTIONS = new List<string>{
		"Select one",
		"Event",
		"Value"
	};
	public static int TYPE_UNDEF = 0;
	public static int TYPE_EVENT = 1;
	public static int TYPE_VALUE = 2;

	//--------------------------------------------------------------------------------------------------

	public static string VALUE_LABEL = "Value Type";
	public static List<string> VALUE_OPTIONS = new List<string>{
		"Dice",
		"Coin",
		"Card",
		"Event",
		"Number"
	};
	public static List<string> VALUE_TOOLTIP = new List<string>{
		"For values >6, use Number instead",
		"Heads = 1, Tails = 0. For multiple Heads, use Number instead",
		"",
		"True = 1, False = 0. For multiple Trues, use Number instead",
		"",
	};
	public static int VAL_DICE   = 0; 
	public static int VAL_COIN 	 = 1;
	public static int VAL_CARD 	 = 2;
	public static int VAL_EVENT  = 3;
	public static int VAL_NUMBER = 4;

	//--------------------------------------------------------------------------------------------------
 
	public static string VAL_DEFAULT_LABEL = "Value";
	public static string VAL_NUMBER_LABEL = "Number";
	public static string VAL_SUIT_LABEL = "Suit";

	public static List<string> VAL_OPTIONS_DICE = new List<string>{"1", "2", "3", "4", "5", "6"};

	public static List<string> VAL_OPTIONS_COIN = new List<string>{"Tails","Heads"};

	public static List<string> VAL_CARD_SUIT = new List<string>{
		'\u2660'.ToString(), //spade
		'\u2661'.ToString(), //heart
		'\u2663'.ToString(), //club
		'\u2662'.ToString()  //diamond
	};

	public static List<string> VAL_CARD_NUM = new List<string>{
		"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"
	};

	public static List<string> VAL_OPTIONS_CARD_SUIT = new List<string>{
		"Any suit",
		string.Format("Spades ({0})"	,VAL_CARD_SUIT[0]),
		string.Format("Hearts ({0})"	,VAL_CARD_SUIT[1]),
		string.Format("Clubs ({0})" 	,VAL_CARD_SUIT[2]),
		string.Format("Diamonds ({0})"	,VAL_CARD_SUIT[3]),
	};	
	
	public static List<string> VAL_OPTIONS_CARD_NUM = new List<string>{
		"Any", "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King"
	};


	public static List<string> VAL_OPTIONS_EVENT = new List<string>{"Fail", "Success"};

	//--------------------------------------------------------------------------------------------------

	public static string LOGIC_LABEL = "Logic";
	public static List<string> LOGIC_OPTIONS = new List<string>{
		"Equals",
		"Not equal to",
		"Larger than",
		"Smaller than",
		"At least",
		"At most",
	};
	public static int LOG_EQUALS	= 0;
	public static int LOG_NEQUAL 	= 1; 
	public static int LOG_LARGER 	= 2;
	public static int LOG_SMALLER 	= 3;
	public static int LOG_ATLEAST 	= 4;
	public static int LOG_ATMOST 	= 5;

	//--------------------------------------------------------------------------------------------------

	public static string SELECTION_LABEL = "Select";
	public static List<string> SELECTION_OPTIONS = new List<string>{
		"All",
		"Any",
		"None",
		"Specific instance",
		"At least [X]",
		"At most [X]",
		"Exactly [X]"
	};  
	public static List<string> SELECTION_TOOLTIP = new List<string>{
		"",
		"Any of {0}'s instances fulfills the rule.",
		"None of {0}'s instances may fulfill the rule.",
		"Only instance {0} of {1} needs to fulfill the rule.",
		"There must be {0} or more of {1}'s instances that fulfill the rule.",
		"There must be {0} or less of {1}'s instances that fulfill the rule.",
		"There must be exactly {0} of {1}'s instances that fulfill the rule."
	};
	public static int SEL_ALL 		= 0;
	public static int SEL_ANY 		= 1;
	public static int SEL_NONE 		= 2;
	public static int SEL_SPECIFIC 	= 3;
	public static int SEL_ATLEAST 	= 4;
	public static int SEL_ATMOST	= 5;
	public static int SEL_EXACTLY	= 6;

	//--------------------------------------------------------------------------------------------------

	public static string X_LABEL = "[X]";
	public static string INSTANCE_LABEL = "Instance";
	public static string AGGREGATE_LABEL = "Aggregate";
	public static List<string> AGGREGATE_OPTIONS = new List<string>{
		"Apply to each",
		"Sum value",
		"Max value",
		"Min value"
	}; 
	public static List<string> AGGREGATE_TOOLTIP = new List<string>{
		"Every single instance of {0} must fulfill the rule.",
		"The sum of all {0}'s instances' values must fulfill the rule.",
		"The largest value of all {0}'s instances must fulfill the rule.",
		"The smallest value of all {0}'s instances must fulfill the rule.",
	};
	public static int AGG_ALL	= 0;
	public static int AGG_SUM	= 1;
	public static int AGG_MAX	= 2;
	public static int AGG_MIN	= 3;

	//--------------------------------------------------------------------------------------------------
 
	public static int VARIABLE_ALL	= 0;
	public static int VARIABLE_ATLEAST	= 1;
	public static int VARIABLE_ATMOST	= 2; 

	//--------------------------------------------------------------------------------------------------
}
