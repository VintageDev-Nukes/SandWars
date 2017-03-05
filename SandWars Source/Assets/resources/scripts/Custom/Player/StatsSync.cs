using UnityEngine;
using System.Collections;

public class StatsSync : Photon.MonoBehaviour {

	public PlayerController player;

	[HideInInspector]
	public float HP, MP, SP, MaxHP, MaxMP, MaxSP;

	[HideInInspector]
	public int SkillPoints, Lvl;

	[HideInInspector]
	public long Exp, Money;

	//public bool isLoaded = false;

	void Start() {
		//this.enabled = photonView.isMine;
		if(!photonView.isMine) {
			player = GetComponent<PlayerController>();
		}
	}

	// Update is called once per frame
	void Update() {
		if(!photonView.isMine) {
			player.HP = HP;
			player.MP = MP;
			player.SP = SP;
			player.MaxHP = MaxHP;
			player.MaxMP = MaxMP;
			player.MaxSP = MaxSP;
			player.SkillPoints = SkillPoints;
			player.Lvl = Lvl;
			player.Exp = Exp;
			player.Money = Money;
		}
	}
}
