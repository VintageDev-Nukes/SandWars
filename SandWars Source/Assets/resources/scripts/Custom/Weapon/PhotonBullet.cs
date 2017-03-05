using UnityEngine;
using System.Collections;

public class PhotonBullet : Photon.MonoBehaviour {

	public float Duration = 3, speed = 10, Damage = 10, criticProb = 20;
	public string playername;
	//public bool destroyed;
	
	private Vector3 newPos = Vector3.zero, oldPos = Vector3.zero, direction = Vector3.zero;
	private bool hasHit, stopped;
	private float timer = 0;
	private Vector3 velocity, lastPosition, curPosition;

	PlayerManager plmn;
	
	// Use this for initialization
	void Start () {
		plmn = GameObject.Find("MainScripts").GetComponent<PlayerManager>();
		if(photonView.isMine) {
			newPos = transform.position; 
			oldPos = newPos;
		}
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit2D bulletHit = Physics2D.Raycast(transform.position, transform.right, 0.1f);
		
		if(hasHit){
			Destroy(this.gameObject);
			return;
		}	

		newPos += (transform.right*speed + direction) * Time.deltaTime;
		
		Vector3 dir = newPos - oldPos;
		float dist = dir.magnitude;
		
		dir /= dist;
		
		oldPos = transform.position; 

		if(photonView.isMine || (!photonView.isMine && stopped)) {
		
			transform.position = newPos;
		
		}

		if(bulletHit) {
			if(bulletHit.transform.GetComponent<PhotonView>() != null) {
				if(bulletHit.transform.GetComponent<PlayerController>() != null && Flags.isPvpEnabled) {
					PlayerController playerHitted = bulletHit.transform.GetComponent<PlayerController>();
					bool critic = Probability.Prob(criticProb);
					if(plmn.Damage(Damage, true, critic, false, bulletHit.transform.position, playerHitted, playername)) {
						hasHit = true;
					}
					//Detect there the PVP? Or maybe make disappear but not make damage?
				} else {
					hasHit = true;
				}
			} else {
				hasHit = true;
			}
		}

		if(!photonView.isMine) {
			if(velocity == Vector3.zero) {
				stopped = true;
			}
		}
		
		if(timer > Duration) {
			Destroy(this.gameObject);
		}
		
		timer += Time.deltaTime;
		
	}

	void FixedUpdate() {
		curPosition = transform.position;
		velocity = (lastPosition - curPosition) / Time.deltaTime;
		lastPosition = transform.position;
	}

	void ApplyDamage(float dmg) {
		Damage += dmg;
	}

	void ApplyVelocity(float Velocity) {
		speed += Velocity;
	}

	void ApplyCritic(float cProb) {
		criticProb = (criticProb+cProb)/2;
	}

	void ApplyName(string name) {
		playername = name;
	}

}
