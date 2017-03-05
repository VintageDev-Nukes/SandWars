using UnityEngine;
using System.Collections;

public class AnimSync : Photon.MonoBehaviour {

	//[HideInInspector]
	public Animator anim;

	[HideInInspector]
	public bool isMoving, isGrounded, isLookingRight, isJumping, isWalking, isCrouching, isCrawling;
	public float aSpeed;

	// Update is called once per frame
	void Update() {
		if(!photonView.isMine) {
			anim.SetBool("isMoving", isMoving);
			anim.SetBool("isGrounded", isGrounded);
			anim.SetBool("isLookingRight", isLookingRight);
			anim.SetBool("isJumping", isJumping);
			anim.SetBool("isWalking", isWalking);
			anim.SetBool("isCrouching", isCrouching);
			anim.SetBool("isCrawling", isCrawling);
			anim.speed = aSpeed;
		}
	}

}
