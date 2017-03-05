using UnityEngine;

public class Synchronizer : Photon.MonoBehaviour {
	//受信データ
	private Vector3 receivePosition = Vector3.zero, receiveScale = Vector3.zero;
	private Quaternion receiveRotation = Quaternion.identity;
	private Vector2 receiveVelocity = Vector2.zero;
	private bool hasRigidBody, hasCollider, isPlayer, isAnim, isBullet;
	public PlayerController player;
	public PhotonBullet bullet;
	
	//Player
	string name;
	float HP;
	float MP;
	float SP;
	float MaxHP;
	float MaxMP;
	float MaxSP;
	int Lvl;
	int SkillPoints;
	long Exp;
	long Money;
	bool playersended;
	bool isLookingRight;

	//Bullet
	float speed, Damage, criticProb;
	string playername;
	

	void Start() {
		hasRigidBody = rigidbody != null;
		hasCollider = gameObject.gameObject.GetComponent<BoxCollider2D>() != null;
		isPlayer = player != null || gameObject.GetComponent<PlayerController>() != null;
		isAnim = isPlayer && gameObject.GetComponent<AnimSync>() != null && gameObject.GetComponent<AnimSync>().anim != null;
		isBullet = bullet != null || gameObject.GetComponent<PhotonBullet>() != null;
		if(isPlayer) {
			if(player == null) {
				player = gameObject.GetComponent<PlayerController>();
			}
		}
		if(isBullet) {
			if(bullet == null) {
				bullet = gameObject.GetComponent<PhotonBullet>();
			}
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			//データの送信
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(transform.localScale);
			if(hasRigidBody) {
				stream.SendNext(rigidbody2D.velocity);
			}
			if(hasCollider) {
				stream.SendNext(gameObject.GetComponent<BoxCollider2D>().size);
				stream.SendNext(gameObject.GetComponent<BoxCollider2D>().center);
			}
			if(isPlayer) {
				stream.SendNext(player.name);
				stream.SendNext(player.HP);
				stream.SendNext(player.isLookingRight);
				/*stream.SendNext(true);
				stream.SendNext(player.name);
				stream.SendNext(player.HP);
				stream.SendNext(player.MP);
				stream.SendNext(player.SP);
				stream.SendNext(player.MaxHP);
				stream.SendNext(player.MaxMP);
				stream.SendNext(player.MaxSP);
				stream.SendNext(player.Lvl);
				stream.SendNext(player.SkillPoints);
				stream.SendNext(player.Exp);
				stream.SendNext(player.Money);*/
			}
			if(isAnim) {
				if(photonView.isMine) {
					stream.SendNext(player.player_anims.GetBool("isMoving"));
					stream.SendNext(player.player_anims.GetBool("isGrounded"));
					stream.SendNext(player.player_anims.GetBool("isLookingRight"));
					stream.SendNext(player.player_anims.GetBool("isJumping"));
					stream.SendNext(player.player_anims.GetBool("isWalking"));
					stream.SendNext(player.player_anims.GetBool("isCrouching"));
					stream.SendNext(player.player_anims.GetBool("isCrawling"));
					stream.SendNext(player.player_anims.speed);
				}
			}
			if(isBullet) {
				if(photonView.isMine) {
					stream.SendNext(bullet.Damage);
					stream.SendNext(bullet.speed);
					stream.SendNext(bullet.criticProb);
					stream.SendNext(bullet.playername);
				}
			}
		} else {
			//データの受信（変数へ格納）
			receivePosition = (Vector3)stream.ReceiveNext();
			receiveRotation = (Quaternion)stream.ReceiveNext();
			receiveScale = (Vector3)stream.ReceiveNext();
			if(hasRigidBody) {
				receiveVelocity = (Vector2)stream.ReceiveNext();
			}
			if(hasCollider) {

				Vector2 size = (Vector2)stream.ReceiveNext();
				Vector2 center = (Vector2)stream.ReceiveNext();

				if(size != gameObject.GetComponent<BoxCollider2D>().size && center != gameObject.GetComponent<BoxCollider2D>().center) {
					DestroyImmediate(gameObject.GetComponent<BoxCollider2D>());
					BoxCollider2D box2D = gameObject.AddComponent<BoxCollider2D>();
					box2D.size = size;
					box2D.center = center;
				}

			}
			if(isPlayer) {
				name = (string)stream.ReceiveNext();
				HP = (float)stream.ReceiveNext();
				isLookingRight = (bool)stream.ReceiveNext();
				/*playersended = (bool)stream.ReceiveNext();
				name = (string)stream.ReceiveNext();
				HP = (float)stream.ReceiveNext();
				MP = (float)stream.ReceiveNext();
				SP = (float)stream.ReceiveNext();
				MaxHP = (float)stream.ReceiveNext();
				MaxMP = (float)stream.ReceiveNext();
				MaxSP = (float)stream.ReceiveNext();
				Lvl = (int)stream.ReceiveNext();
				SkillPoints = (int)stream.ReceiveNext();
				Exp = (long)stream.ReceiveNext();
				Money = (long)stream.ReceiveNext();*/
			}
			if(isAnim) {
				if(!photonView.isMine) {
					AnimSync ansync = gameObject.GetComponent<AnimSync>();
					ansync.isMoving = (bool)stream.ReceiveNext();
					ansync.isGrounded = (bool)stream.ReceiveNext();
					ansync.isLookingRight = (bool)stream.ReceiveNext();
					ansync.isJumping = (bool)stream.ReceiveNext();
					ansync.isWalking = (bool)stream.ReceiveNext();
					ansync.isCrouching = (bool)stream.ReceiveNext();
					ansync.isCrawling = (bool)stream.ReceiveNext();
					ansync.aSpeed = (float)stream.ReceiveNext();
				}
			}
			if(isBullet) {
				if(!photonView.isMine) {
					Damage = (float)stream.ReceiveNext();
					speed = (float)stream.ReceiveNext();
					criticProb = (float)stream.ReceiveNext();
					playername = (string)stream.ReceiveNext();
				}
			}
		}
	}
	
	void Update() {
		//自分以外のプレイヤーの補正
		if(!photonView.isMine) {
			transform.position = Vector3.Lerp(transform.position, receivePosition, Time.deltaTime * 10);
			transform.rotation = Quaternion.Lerp(transform.rotation, receiveRotation, Time.deltaTime * 10);
			transform.localScale = receiveScale;
			if(hasRigidBody) {
				rigidbody2D.velocity = Vector2.Lerp(rigidbody2D.velocity, receiveVelocity, Time.deltaTime * 10);
			}
			if(isBullet) {
				bullet.Damage = Damage;
				bullet.speed = speed;
				bullet.criticProb = criticProb;
				bullet.playername = playername;
			}
			if(isPlayer) {
				player.HP = HP;
				player.name = name;
				player.isLookingRight = isLookingRight;
			}
		} /*else {
			if(playersended) {
				player.HP = HP;
				player.MP = MP;
				player.SP = SP;
				player.MaxHP = MaxHP;
				player.MaxMP = MaxMP;
				player.MaxSP = MaxSP;
				player.Lvl = Lvl;
				player.SkillPoints = SkillPoints;
				player.Exp = Exp;
				player.Money = Money;
			}
		}*/
	}

}
