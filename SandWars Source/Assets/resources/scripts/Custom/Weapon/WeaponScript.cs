using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;

public enum WeaponType {Manual, Automatic}
public enum WeaponKind {Melee, Handgun, Shotgun, Rifle, Sniper, RPG, Thrown}

public class WeaponScript : Photon.MonoBehaviour {

	GameObject bullet;

	public WeaponType GunType;
	public WeaponKind GunKind;
	public float Damage = 10, useTime = 1, bulletVelocity = 15, criticProb;
	public int bulletsToShot = 1;
	public bool canShoot;

	private float timer;
	private bool firstTime = true, isResting;
	private string Level; //This level will set the gun's properties

	PlayerController player;

	//Var player, que detectara si el jugador esta muerto para que no dispare
	//A su vez hacer que player tenga otro gameobject que se llame SelectedItem que buscara esta variable dentro de GetComponent y le dira si esta muerto

	// Use this for initialization
	void Start() {
		//if(photonView.isMine) {
			bullet = Resources.Load<GameObject>("prefabs/Bullet");
			player = transform.root.GetComponent<PlayerController>();
			player.attachedWeapon = this;
		//}
		foreach(Match match in Regex.Matches(transform.name, "(?<Kind>\\D+) (?<Level>\\d+)", RegexOptions.IgnoreCase)) {
			GunKind = (WeaponKind)Enum.Parse(typeof(WeaponKind), match.Groups["Kind"].Value);
			Level = match.Groups["Level"].Value;
		}
	}
	
	// Update is called once per frame
	void Update() {

		if(!Rooms.isLobbyReady) {
			return;
		}

		if(photonView.isMine && !GameObject.Find(player.name).GetComponent<PlayerController>().isFalling && !player.isKilled && canShoot) {
			UpdateRotation();
		}
		if(canShoot) {
			if(isResting) {
				transform.parent = transform.FindChild("ArmB");
				isResting = false;
			}
			if(GunType == WeaponType.Manual) {
				if(Input.GetMouseButtonDown(0)) {
					if(timer > useTime || firstTime) {
						ShotBullets(bulletsToShot);
						firstTime = false;
						timer = 0;
					}
				}
				if(timer < useTime) {
					timer += Time.deltaTime;
				}
			} else if(GunType == WeaponType.Automatic) {
				if(Input.GetMouseButton(0)) {
					if(timer > useTime || firstTime) {
						ShotBullets(bulletsToShot);
						firstTime = false;
						timer = 0;
					}
				}
				if(timer < useTime) {
					timer += Time.deltaTime;
				}
			}
		} else {
			//Put the weapon in the back... (Clasify it and put it in diferent parts...)
			if(GunKind == WeaponKind.Melee) {

			} else if(GunKind == WeaponKind.Handgun) {
				
			} else if(GunKind == WeaponKind.Rifle) {
				
			} else if(GunKind == WeaponKind.RPG) {
				
			} else if(GunKind == WeaponKind.Shotgun) {
				transform.parent = transform.root;
				transform.eulerAngles = new Vector3(0, 0, 270);
				if(player.isLookingRight) {
					transform.localPosition = new Vector3(-0.25f, 0.3f, 0);
					Vector3 curScale = transform.localScale;
					transform.localScale = new Vector3(curScale.x, Mathf.Abs(curScale.y), curScale.z);
				} else {
					transform.localPosition = new Vector3(0.3f, 0.1f, 0);
					Vector3 curScale = transform.localScale;
					transform.localScale = new Vector3(curScale.x, -Mathf.Abs(curScale.y), curScale.z);
				}
			} else if(GunKind == WeaponKind.Sniper) {
				
			} else if(GunKind == WeaponKind.Thrown) {
				
			}
			isResting = true;
		}

	}

	/*float test;

	void FixedUpdate() {
		transform.eulerAngles = new Vector3(0, 0, test);
		test += 1;
	}*/
	
	void UpdateRotation() {
		if(photonView.isMine) {
			Vector3 mouseScreenPosition = Input.mousePosition;
			mouseScreenPosition.z = transform.position.z;
			Vector3 mouseWorldSpace = Camera.mainCamera.ScreenToWorldPoint(mouseScreenPosition);
			transform.LookAt(mouseWorldSpace, Vector3.back);
			float z = -transform.eulerAngles.z-90;
			Vector3 sc = transform.localScale;
			float y = Mathf.Abs(sc.y);
			float pz = 360+z;
			if(pz > 90 && pz < 270) {
				y = -Mathf.Abs(sc.y);
				if(player.isLookingRight) {
					player.isLookingRight = false;
				}
			} else {
				if(!player.isLookingRight) {
					player.isLookingRight = true;
				}
			}
			transform.localScale = new Vector3(sc.x, y, 1);
			transform.eulerAngles = new Vector3(0,0,z);
		}
	}

	void ShotBullets(int bulletNum, float marginAngle = 2.5f) {
		//Creo que la parte de photonView.mine sobra
		/*if(photonView.isMine) { //For avoid external uses
			if(bulletNum == 1) {
				Instantiate(bullet, transform.FindChild("muzzle").position, transform.rotation);
			} else {
				float angle = transform.eulerAngles.z;
				for(int i = 0; i < bulletNum; i++) {
					Instantiate(bullet, transform.FindChild("muzzle").position, Quaternion.Euler(new Vector3(0, 0, angle)));
					angle += marginAngle;
				}
			}
		} else {
			RPCProjectiles(bulletNum, marginAngle);
		}*/
		RPCProjectiles(bulletNum, marginAngle);
		//Test();
	}
	
	void RPCProjectiles(int bulletNum, float marginAngle = 2.5f) {
		if(bulletNum == 1) {
			GameObject newBullet = PhotonNetwork.Instantiate("Bullet", transform.FindChild("muzzle").position, transform.rotation, 0);
			newBullet.BroadcastMessage("ApplyDamage", Damage);
			newBullet.BroadcastMessage("ApplyVelocity", bulletVelocity);
			newBullet.BroadcastMessage("ApplyCritic", criticProb);
			newBullet.BroadcastMessage("ApplyName", player.name);
		} else {
			float angle = transform.eulerAngles.z;
			for(int i = 0; i < bulletNum; i++) {
				GameObject newBullet = PhotonNetwork.Instantiate("Bullet", transform.FindChild("muzzle").position, Quaternion.Euler(new Vector3(0, 0, angle)), 0);
				newBullet.BroadcastMessage("ApplyDamage", Damage);
				newBullet.BroadcastMessage("ApplyVelocity", bulletVelocity);
				newBullet.BroadcastMessage("ApplyCritic", criticProb);
				newBullet.BroadcastMessage("ApplyName", player.name);
				angle += marginAngle;
			}
		}
	}

}
