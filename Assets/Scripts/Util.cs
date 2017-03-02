
public class Util {
	public static int GetGCD(int a, int b){
		return b == 0? a : GetGCD(b, a%b); 
	}

	public static long GetGCD(long a, long b){
		return b == 0? a : GetGCD(b, a%b); 
	}

	public static int Card_GetSuit(int a){
		return a%4;
	}

	public static int Card_GetNum(int a){
		return a/4;
	}

	public static int Card_GetID(int suit, int num){
		return ((num*4) + suit);
	}
}

