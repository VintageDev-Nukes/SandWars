using UnityEngine;
using System.Collections;

public class LvlSys {
	
	private static int _lvl;
	
	private static int _hts;
	private static int _accsx;
	private static int _accsy;
	private static int _iix;
	private static int _iiy;
	private static int _ammo;
	private static int _exp;
	
	public static int Level {
		get {return _lvl;}
		set {_lvl = value;}
	}
	
	public static int HTS {
		get {return _hts;}
		set {_hts = value;}
	}
	
	public static int AccsX {
		get {return _accsx;}
		set {_accsx = value;}
	}
	
	public static int AccsY {
		get {return _accsy;}
		set {_accsy = value;}
	}
	
	public static int IIX {
		get {return _iix;}
		set {_iix = value;}
	}
	
	public static int IIY {
		get {return _iiy;}
		set {_iiy = value;}
	}
	
	public static int Ammo {
		get {return _ammo;}
		set {_ammo = value;}
	}
	
	public static int Exp {
		get {return _exp;}
		set {_exp = value;}
	}

	public static void SetSlotsByLvl() {
		int lvl = Level;
		if (lvl == 0) {
			
			HTS = 10;
			AccsX = 5;
			AccsY = 2;
			IIX = 16;
			IIY = 4;
			Ammo = 5;
			
		}
	}

	public static ulong GetExpfromLvl(int lvl, bool accumulative = false) 
	{

		ulong number = 0;
		
		if(accumulative) 
		{
			for(int i = 0; i<=lvl; ++i) 
			{
				if(i < 100) {
					number += (ulong)Mathf.RoundToInt(50 + i * Mathf.Pow((i+1), (1+i/100)) * 50);
				} else {
					number += 51005050;
				}
			}
		} else 
		{
			if(lvl < 100) {
				number = (ulong)Mathf.RoundToInt(50 + lvl * Mathf.Pow((lvl+1), (1+lvl/100)) * 50);
			} else {
				number = 51005050;
			}
		}
		
		return number;

	}

	public static ulong BonusCashfromLvlUp(int lvl, bool accumulative = false) {

		ulong number = 0;
		
		if(accumulative) 
		{
			for(int i = 0; i<=lvl; ++i) 
			{
				number += (ulong)Mathf.RoundToInt(25 + Mathf.Pow(i, (2+i/100)) * 5);
			}
		} else 
		{
			number = (ulong)Mathf.RoundToInt(25 + Mathf.Pow(lvl, (2+lvl/100)) * 5);
		}
		
		return number;

	} 

	public static void RecalculateLvl() {

		MoneySystem money = GameObject.Find("GameScripts").GetComponent<MoneySystem>();

		int actualLvl = PlayerStats.Lvl;
		int newlvl = PlayerStats.Lvl;
		ulong exp = PlayerStats.Exp;
		
		bool reachednewlvl = false;
		
		if(GetExpfromLvl(actualLvl, true) < exp) 
		{

			newlvl = actualLvl + 1;
			//PlayerStats.Money += 
			money.ChangeMoney(BonusCashfromLvlUp(actualLvl));
			AudioManager.Play(Resources.Load<AudioClip>("sounds/lvlup"), Player.PlayerObj.transform.position);

		}

		PlayerStats.Lvl = newlvl;

	}

	public static ulong tempExp;
	public static string newExp;

	public static IEnumerator FadeExp(ulong newExpSum, float duration) {
		
		float t = 0;
		
		newExp = "+"+newExpSum.ToString();
		
		while(true) {
			
			yield return null;

			Color tmpColor = GUIStyles.ExpStyle.normal.textColor;
			
			if(t < duration) {
				tmpColor.a = Mathf.Lerp(0, 1, t/duration);
				GUIStyles.NewExpStyle.normal.textColor = tmpColor;
			} else if(t > duration && t < duration*2) {
				tmpColor.a = Mathf.Lerp(1, 0, t/duration/2);
				GUIStyles.NewExpStyle.normal.textColor = tmpColor;
			}
			
			t += Time.deltaTime;
			
			if(t > duration*2) {
				break;
			}
			
		}
		
	}
	
}

public class LevelSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LvlSys.Level = 0;
		LvlSys.SetSlotsByLvl();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(PlayerStats.Exp != LvlSys.tempExp) {
			StartCoroutine(LvlSys.FadeExp(PlayerStats.Exp - LvlSys.tempExp, 2));
			LvlSys.RecalculateLvl();
		}

		LvlSys.tempExp = PlayerStats.Exp;

	}
}
