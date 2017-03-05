using UnityEngine;
using System.Collections;

public enum PlayerStates {Standing, Crouching, Crawling}

public class PlayerController : Photon.MonoBehaviour {

	//Internal errors debugger (borra algo de aqui: akdfklfkd) para que el compilador recompile

	Transform player;

	//Body parts
	Transform ArmA, ArmA_000, ArmB, ArmB_000, Head, Body, LegA, LegA_000, LegB, LegB_000;

	//Animator controller bools + other
	[HideInInspector]
	public bool canMove, isMoving, isJumping, isGrounded, isFalling, isLookingRight, isWalking, isCrouching, isCrawling, isPlayerRagdoll, canRun, isRunning, isAlive, isKilled, canShoot, showHP = true, fullHP, isMasterKilled;

	//Player coordinates
	private Vector3 playerpos, velocity, lastPosition, curPosition;

	//Movement & jumping speed related variables
	public float walkSpeed = 1, runSpeed = 2.3f, crouchSpeed = 0.5f, crawlSpeed = 0.2f, JumpHeight, JumpDelay;
	private float movementSpeed = 1, speedFactor = 1, currentJumpHeight;
	public float SPRec = 0.5f; //Recovery factors
	public float SPConsInRun = 1; //Waste variables

	[HideInInspector]
	public PlayerStates currentState = PlayerStates.Standing;

	//In games statics
	public float HP, MP, SP, MaxHP, MaxMP, MaxSP;

	public int SkillPoints, Lvl;

	public long Exp, Money;

	PlayerManager plmn;
	GameGUI ggui;

	[HideInInspector]
	public WeaponScript attachedWeapon;

	//Profile statics...
	//Stregth, hability, agility (multiplican HP, MP, SP)

	//Animator
	public Animator player_anims;
	public AnimSync ansync;

	//public bool passtosub;

	public string name;

	void Awake() {
		if(photonView.isMine) {
			CameraController.target = transform;
		}
		transform.name = photonView.owner.ToString();
		name = transform.name;
		transform.FindChild("PlayerName").GetComponent<TextMesh>().text = transform.name;
		plmn = GameObject.Find("MainScripts").GetComponent<PlayerManager>();
		ggui = GameObject.Find("MainScripts").GetComponent<GameGUI>();
	}

	void Start() {

		if(photonView.isMine) {

			PlayerManager.player = this;

			ggui.player = this;

			player = transform;

			foreach(GameObject background in GameObject.FindGameObjectsWithTag("Background")) {
				if(background.GetComponent<ParallaxScript>() != null) {
					background.GetComponent<ParallaxScript>().target = this.transform;
				}
			}

			ArmA = player.FindChild("ArmA");
			ArmA_000 = player.FindChild("ArmA_000");
			ArmB = player.FindChild("ArmB");
			ArmB_000 = player.FindChild("ArmB_000");
			Head = player.FindChild("Head");
			Body = player.FindChild("Body");
			LegA = player.FindChild("LegA");
			LegA_000 = player.FindChild("LegA_000");
			LegB = player.FindChild("LegB");
			LegB_000 = player.FindChild("LegB_000");

			isGrounded = true;
			isWalking = true;
			canShoot = true;

		}

		plmn.FillStats(this);
		ansync.anim = player_anims;

		Physics2D.IgnoreLayerCollision(8, 8);
		
		//Debug.Log("Name: "+name+", maxh: "+MaxHP);

	}

	void Update() {

		isAlive = HP > 0;
		canRun = SP > 0;
		fullHP = HP == MaxHP;

		if(HP>100) {
			HP = 100;
		}

		if(!photonView.isMine && Rooms.isLobby && System.String.IsNullOrEmpty(transform.name) && !System.String.IsNullOrEmpty(name)) {
			transform.name = name;
		}

		canMove = Flags.canMove;
		canShoot = Flags.canShoot;

		//HP -= 0.01f;

		if(!isAlive && !isKilled) {
			plmn.KillMe(GetComponent<PlayerController>());
		}

		if(photonView.isMine) {

			playerpos = Vector3.zero;

			if(isPlayerRagdoll) {
				playerpos = Body.position;
				//CameraController.target = Body;
			} else {
				playerpos = player.position;
				//CameraController.target = transform;
			}

			if(!isPlayerRagdoll) {

				UpdateMovements();

				isMoving = velocity != Vector3.zero;

				isGrounded = Physics2D.Raycast(new Vector2(playerpos.x, playerpos.y-1.31f), -Vector2.up, 0.1f).transform != null;

				isFalling = !isGrounded && !isJumping;

				UpdateAnimations();
			
			}

			SwitchRagdoll();

		}

		if(!isAlive && isKilled) {
			if(photonView.isMine) {
				ggui.InvokeDeadScreen();
			} else {
				//Anim the player
			}
		}

		string leftHP = "";

		if(!fullHP) {
			float tempHP = HP;
			if(HP < 0) {
				tempHP = 0;
			}
			leftHP = " ["+tempHP+"/"+MaxHP+"]";
			if(HP < 0) {
				leftHP = " (Dead)";
			}
		}

		if(showHP) {
			transform.FindChild("PlayerName").GetComponent<TextMesh>().text = name+leftHP;
		}

		if(photonView.isMine && !isKilled && transform.position.y < Flags.minHeight) {
			plmn.Damage(Random.Range(5, 10), true);
		}

	}

	void FixedUpdate() {
		if(photonView.isMine) {
			curPosition = transform.position;
			velocity = (lastPosition - curPosition) / Time.deltaTime;
			lastPosition = transform.position;
			attachedWeapon.canShoot = canShoot;
			if(isRunning) {
				SP -= SPConsInRun;
				movementSpeed = runSpeed*speedFactor;
				player_anims.speed = 2*speedFactor;
			} else {
				movementSpeed = walkSpeed*speedFactor;
				player_anims.speed = 1*speedFactor;
				if(SP < MaxSP) {
					if(isMoving) {
						SP += SPRec/1.5f;
					} else {
						SP += SPRec;
					}
				}
			}
		}
	}

	void UpdateAnimations() {

		if(photonView.isMine) {
			player_anims.SetBool("isMoving", isMoving);
			player_anims.SetBool("isGrounded", isGrounded);
			player_anims.SetBool("isLookingRight", isLookingRight);
			player_anims.SetBool("isJumping", isJumping);
			player_anims.SetBool("isWalking", isWalking);
			player_anims.SetBool("isCrouching", isCrouching);
			player_anims.SetBool("isCrawling", isCrawling);
		}
		
	}

	void UpdateMovements() {
		if(photonView.isMine) {
			if(canMove && !isKilled) {
				if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
					player.position = playerpos + new Vector3(movementSpeed/10, 0, 0);
					isLookingRight = true;
				} else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
					player.position = playerpos - new Vector3(movementSpeed/10, 0, 0);
					isLookingRight = false;
				} 
				if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
				{
					isJumping = true;
					currentJumpHeight = JumpHeight;
				}
				if(isMoving) {
					if(Input.GetKeyDown(KeyCode.LeftShift) && canRun) {
						isRunning = true;
					} else if(Input.GetKeyUp(KeyCode.LeftShift)) {
						isRunning = false;
					}
				}
				if(isRunning && (!canRun || !canMove)) {
					isRunning = false;
				}
				if(Input.GetKeyDown(KeyCode.DownArrow)) {
					if(currentState == PlayerStates.Standing) {
						SetCrouch();
					} else if(currentState == PlayerStates.Crouching) {
						SetCrawl();
					}
				} else if(Input.GetKeyDown(KeyCode.UpArrow)) {
					if(currentState == PlayerStates.Crawling) {
						SetCrouch();
					} else if(currentState == PlayerStates.Crouching) {
						SetWalk();
					}
				}
				if(Input.GetKeyDown(KeyCode.C)) {
					if(currentState == PlayerStates.Standing || currentState == PlayerStates.Crouching) {
						SetCrawl();
					} else if(currentState == PlayerStates.Crawling) {
						SetWalk();
					}
				} else if(Input.GetKeyDown(KeyCode.Space)) {
					SetWalk();
				}
				if (isJumping)
				{
					isGrounded = false;
					if (currentJumpHeight > 0)
					{
						rigidbody2D.AddForce(Vector2.up * currentJumpHeight * 100);
						currentJumpHeight -= JumpDelay;
					}
					else
					{
						currentJumpHeight = 0;
						isJumping = false;
					}
				}
			}
		}
	}

	void SetWalk() {
		if(photonView.isMine) {
			currentState = PlayerStates.Standing;
			isWalking = true;
			isCrouching = false;
			isCrawling = false;
			player_anims.speed = 1;
			movementSpeed = walkSpeed;
			speedFactor = walkSpeed;
			GetComponent<BoxCollider2D>().size = new Vector2(1.2f, 2.5f);
			GetComponent<BoxCollider2D>().center = new Vector2(0, -0.05f);
		}
	}

	void SetCrouch() {
		if(photonView.isMine) {
			currentState = PlayerStates.Crouching;
			isWalking = false;
			isCrouching = true;
			isCrawling = false;
			player_anims.speed = 0.5f;
			movementSpeed = crouchSpeed;
			speedFactor = crouchSpeed;
			if(isLookingRight) {
				GetComponent<BoxCollider2D>().size = new Vector2(1.1f, 2.32f);
				GetComponent<BoxCollider2D>().center = new Vector2(0.18f, -0.36f);
			} else {
				//GetComponent<BoxCollider2D>().size = new Vector2(1, 2.25f);
				//GetComponent<BoxCollider2D>().center = new Vector2(0.08f, -0.17f);
				GetComponent<BoxCollider2D>().size = new Vector2(1, 2.25f);
				GetComponent<BoxCollider2D>().center = new Vector2(0, -0.1f);
			}
		}
	}

	void SetCrawl() {
		if(photonView.isMine) {
			currentState = PlayerStates.Crawling;
			isWalking = false;
			isCrouching = false;
			isCrawling = true;
			player_anims.speed = 0.1f;
			movementSpeed = crawlSpeed;
			speedFactor = crawlSpeed;
			GetComponent<BoxCollider2D>().size = new Vector2(2.5f, 1);
			GetComponent<BoxCollider2D>().center = new Vector2(0.55f, -0.05f);
		}
	}

	void SwitchRagdoll() {

		if(photonView.isMine) {

			//Convert into a ragdoll

			if(isPlayerRagdoll && player.GetComponent<BoxCollider2D>().enabled) {

				CameraController.target = Body;

				if(player.rigidbody2D != null) {
					Destroy(player.rigidbody2D);
				}

				player.GetComponent<BoxCollider2D>().enabled = false;

				if(Body.rigidbody2D == null) {
					Body.gameObject.AddComponent<Rigidbody2D>();
				}

				if(Body.GetComponent<BoxCollider2D>() != null) {
					BoxCollider2D pbox2D = Body.gameObject.AddComponent<BoxCollider2D>();
					pbox2D.size = new Vector2(1.2f, 2.5f);
					pbox2D.center = new Vector2(0, -0.05f);
				}

				if(ArmA.rigidbody2D == null) {
					ArmA.gameObject.AddComponent<Rigidbody2D>();
				}

				if(ArmA_000.rigidbody2D == null) {
					ArmA_000.gameObject.AddComponent<Rigidbody2D>();
				}

				if(ArmB.rigidbody2D == null) {
					ArmB.gameObject.AddComponent<Rigidbody2D>();
				}

				if(ArmB_000.rigidbody2D == null) {
					ArmB_000.gameObject.AddComponent<Rigidbody2D>();
				}

				if(Head.rigidbody2D == null) {
					Head.gameObject.AddComponent<Rigidbody2D>();
				}

				if(Body.rigidbody2D == null) {
					Body.gameObject.AddComponent<Rigidbody2D>();
				}

				if(LegA.rigidbody2D == null) {
					LegA.gameObject.AddComponent<Rigidbody2D>();
				}

				if(LegA_000.rigidbody2D == null) {
					LegA_000.gameObject.AddComponent<Rigidbody2D>();
				}

				if(LegB.rigidbody2D == null) {
					LegB.gameObject.AddComponent<Rigidbody2D>();
				}

				if(LegB_000.rigidbody2D == null) {
					LegB_000.gameObject.AddComponent<Rigidbody2D>();
				}

			} else if(!isPlayerRagdoll && !player.GetComponent<BoxCollider2D>().enabled) {

			//Convert ragdoll into player

				CameraController.target = transform;

				player.GetComponent<BoxCollider2D>().enabled = true;

				if(Body.GetComponent<BoxCollider2D>() != null) {
					Destroy(Body.GetComponent<BoxCollider2D>());
				}

				if(player.rigidbody2D == null) {
					player.gameObject.AddComponent<Rigidbody2D>();
				}

				if(Body.rigidbody2D != null) {
					Destroy(Body.rigidbody2D);
				}
				
				if(ArmA.rigidbody2D != null) {
					Destroy(ArmA.rigidbody2D);
				}
				
				if(ArmA_000.rigidbody2D != null) {
					Destroy(ArmA_000.rigidbody2D);
				}
				
				if(ArmB.rigidbody2D != null) {
					Destroy(ArmB.rigidbody2D);
				}
				
				if(ArmB_000.rigidbody2D != null) {
					Destroy(ArmB_000.rigidbody2D);
				}
				
				if(Head.rigidbody2D != null) {
					Destroy(Head.rigidbody2D);
				}
				
				if(Body.rigidbody2D != null) {
					Destroy(Body.rigidbody2D);
				}
				
				if(LegA.rigidbody2D != null) {
					Destroy(LegA.rigidbody2D);
				}
				
				if(LegA_000.rigidbody2D != null) {
					Destroy(LegA_000.rigidbody2D);
				}
				
				if(LegB.rigidbody2D != null) {
					Destroy(LegB.rigidbody2D);
				}
				
				if(LegB_000.rigidbody2D != null) {
					Destroy(LegB_000.rigidbody2D);
				}

			}

		}

	}

}