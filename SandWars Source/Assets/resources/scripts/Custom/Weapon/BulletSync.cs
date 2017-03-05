using UnityEngine;
using System.Collections;

public class BulletSync : Photon.MonoBehaviour {

	public PhotonBullet bullet;

	[HideInInspector]
	public float Damage, speed, criticProb;

	//public bool isLoaded;

	// Use this for initialization
	void Start () {
		if(!photonView.isMine) {
			bullet = GetComponent<PhotonBullet>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!photonView.isMine) {
			bullet.Damage = Damage;
			bullet.speed = speed;
			bullet.criticProb = criticProb;
		}
	}
}
