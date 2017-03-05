using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public static PlayerController player;
	
	public void FillStats(Object playernul = null) {

		PlayerController pl = player;

		if(playernul != null) {
			pl = (PlayerController)playernul;
		}

		pl.MaxHP = 100;
		pl.MaxMP = 100;
		pl.MaxSP = 100;
		pl.HP = pl.MaxHP;
		pl.MP = pl.MaxMP;
		pl.SP = pl.MaxSP;

	}

	public bool Damage(float dmg, bool random = false, bool critic = false, bool damagemyself = true, Vector3? nulpos = null, Object playernul = null, string attacker = "") {
		
		PlayerController pl = player;
		
		if(playernul != null) {
			pl = (PlayerController)playernul;
		}

		Vector3 pos = pl.transform.position;

		if(nulpos != null) {
			pos = (Vector3)nulpos;
		}

		bool next = false;

		string victim = pl.transform.name;
		string myname = player.transform.name;

		if(damagemyself) {
			next = true;
		} else {
			if(victim != myname || attacker != victim) {
				next = true;
			}	
		}

		if(next) {

			float Damage = dmg;

			if(random) {
				Damage += Random.Range(-5, 5);
			}

			if(critic) {
				Damage *= (Random.Range(15, 25)/10);
			}
			pl.HP -= Damage;

			ShowDamage(pos, Damage, critic);

		}

		return next;
		
	}

	public void KillMe(Object playernul = null) {
		
		PlayerController pl = player;
		
		if(playernul != null) {
			pl = (PlayerController)playernul;
		}
		
		pl.isKilled = true;
		pl.canMove = false;
		pl.canShoot = false;
		
	}

	public void Respawn() {
		player.HP = player.MaxHP;
		player.isKilled = false;
		player.canShoot = true;
		GameObject.Find("MainScripts").GetComponent<GameGUI>().deadScreen = false;
	}

	public void ShowDamage(Vector3 pos, float damage, bool critic = false) {
		GameObject dmgText = (GameObject)Instantiate(Resources.Load("prefabs/DmgText"), pos, Quaternion.identity);
		object[] obj = new object[2] {damage, critic};
		dmgText.BroadcastMessage("ApplyText", obj);
	}

}
