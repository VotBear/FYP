using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class C {

	public static float P_START_HEIGHT = -10f;
	public static float P_DISTANCE = 22f;
	public static float P_INDENT = 8f;
	public static float P_OPERATOR_IMG_BASEHEIGHT = -3f;

	public static float P_HEIGHT_LIMIT = 15;

	public static int MODE_RULE = 0;
	public static int MODE_COND = 1;

	public static string RULE_LABEL = "Rule List:";
	public static string COND_LABEL = "Given That:";
	//--------------------------------------------------------------------------------------------------

	public static int TAB_PROBLEMS	 = 0;
	public static int TAB_EVENTS	 = 1;
	public static int TAB_RULES		 = 2; 
	public static int TAB_CONDITIONS = 3; 

	//--------------------------------------------------------------------------------------------------

	public static float P_PROBDISTANCE = 27f;

	public static int PROBLEM_PAGE_LIST = 0;
	public static int PROBLEM_PAGE_DETAIL = 1;

	public static int PROBLEM_ANS_MODE_SSPACE 	= 0;
	public static int PROBLEM_ANS_MODE_CHANCE 	= 1;
	public static int PROBLEM_ANS_MODE_INPUT 	= 2;
	public static int PROBLEM_ANS_MODE_CHANCEINP= 3;

	public static string PROBLEM_ANS_TEXT_CHANCE = 
		"Current Answer: {0}/{1} ({2}%)";

	public static string PROBLEM_ANS_TEXT_SSPACE = 
		"Current Answer: {0} possible outcomes";

	public static string PROBLEM_ANS_TEXT_COND_SSPACE = 
		"Current Answer: {0} valid outcomes";

	public static string PROBLEM_ANS_TEXT_INVALID = 
		"Current Answer: N/A";

	public static string PROBLEM_RES_TEXT_RIGHT = 
		"Correct!";

	public static string PROBLEM_RES_TEXT_WRONG = 
		"Not quite right. Keep trying!";

	public static string PROBLEM_RES_TEXT_REQ_FALSE = 
		"Requirements not fulfilled.";
	
	//--------------------------------------------------------------------------------------------------

	public static string SINGLE_TOOLTIP =
		"{0} must fulfill the rule:";

	public static string CHANCE_TEXT = 
		"Probability: {0}/{1} ({2}%)";

	public static string CONDITION_TEXT = 
		"{0}/{1} outcomes fulfill the conditions";

	public static string SAMPLESPACE_TEXT = 
		"{0} possible outcomes";

	public static string INVALID_TEXT = 
		"N/A";

	//--------------------------------------------------------------------------------------------------

	public static string TYPE_LABEL = "Type";
	public static List<string> TYPE_OPTIONS = new List<string>{
		"Select variable type",
		"Event",
		"Value"
	};
	public static int TYPE_UNDEF = 0;
	public static int TYPE_EVENT = 1;
	public static int TYPE_VALUE = 2;

	//--------------------------------------------------------------------------------------------------

	public static string EVENT_LABEL = "Event";
	public static string EVENT_DEFAULT_OPTION = "Select an event";
	public static int EVENT_SELECT = 0;

	//--------------------------------------------------------------------------------------------------
	public static string VALUE_LABEL = "Value Type";
	public static List<string> VALUE_OPTIONS = new List<string>{
		"Select value type",
		"Dice",
		"Coin",
		"Card",
		"Event",
		"Number"
	};

	public static List<string> VALUE_TOOLTIP = new List<string>{
		"Select a value type",
		"For values >6, use Number instead",
		"Tails = 0, Heads = 1.\nFor multiple Heads, use Number instead",
		"Diamonds < Clubs < Hearts < Spades",
		"Fail = 0, Success = 1.\nFor multiple Successes, use Number instead",
		"",
	};
	public static int VAL_SELECT = 0;
	public static int VAL_DICE   = 1; 
	public static int VAL_COIN 	 = 2;
	public static int VAL_CARD 	 = 3;
	public static int VAL_EVENT  = 4;
	public static int VAL_NUMBER = 5;

	//--------------------------------------------------------------------------------------------------
 
	public static string VAL_DEFAULT_LABEL = "Value";
	public static string VAL_NUMBER_LABEL = "Number";
	public static string VAL_SUIT_LABEL = "Suit";

	public static List<string> VAL_OPTIONS_DICE = new List<string>{"1", "2", "3", "4", "5", "6"};

	public static List<string> VAL_OPTIONS_COIN = new List<string>{"Tails","Heads"};

	public static List<string> VAL_CARD_SUIT = new List<string>{
		'\u2666'.ToString(), //diamond
		'\u2663'.ToString(), //club
		'\u2665'.ToString(), //heart
		'\u2660'.ToString()  //spade
	};

	public static List<string> VAL_CARD_NUM = new List<string>{
		"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"
	};

	public static List<string> VAL_OPTIONS_CARD_SUIT = new List<string>{
		"Any suit",
		string.Format("Diamonds ({0})"	,VAL_CARD_SUIT[0]),
		string.Format("Clubs ({0})"		,VAL_CARD_SUIT[1]),
		string.Format("Hearts ({0})" 	,VAL_CARD_SUIT[2]),
		string.Format("Spades ({0})"	,VAL_CARD_SUIT[3]),
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
		"Is multiple of",
	};
	public static int LOG_EQUALS	= 0;
	public static int LOG_NEQUAL 	= 1; 
	public static int LOG_LARGER 	= 2;
	public static int LOG_SMALLER 	= 3;
	public static int LOG_ATLEAST 	= 4;
	public static int LOG_ATMOST 	= 5;
	public static int LOG_MULTIPLE 	= 6;

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

}
