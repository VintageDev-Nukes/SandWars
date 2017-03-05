using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public float Duration = 3, speed = 10, Damage = 10;

	private Vector3 newPos = Vector3.zero, oldPos = Vector3.zero, direction = Vector3.zero;
	private bool hasHit = false;

	// Use this for initialization
	void Start () {
		newPos = transform.position; 
		oldPos = newPos;
		Destroy(this.gameObject, Duration);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (hasHit){
			Destroy(this.gameObject);
			return;
		}	
		
		newPos += (transform.right*speed + direction) * Time.deltaTime;
		
		Vector3 dir = newPos - oldPos;
		float dist = dir.magnitude;
		
		dir /= dist;

		oldPos = transform.position; 
		transform.position = newPos;

		if(Physics2D.Raycast(transform.position, transform.right, 0.1f)) {
			hasHit = true;
		}

	}

}
