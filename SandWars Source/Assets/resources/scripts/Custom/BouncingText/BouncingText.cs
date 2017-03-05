using UnityEngine;
using System.Collections;

public class BouncingText : MonoBehaviour {

	public float heightToMove = 1, timeToMove = 5;
	float y;
	bool moved;
	Vector3 velocity, lastPosition, curPosition;

	void Start() {
		y = transform.position.y;
	}
	
	void FixedUpdate() {
		Vector3 curPos = transform.position;
		if(transform.position.y == y && !moved) {
			StartCoroutine(Vector.MoveFromTo(transform, transform.position, new Vector3(curPos.x, y+heightToMove/2, curPos.z), timeToMove/2));
		} else if(transform.position.y == y+heightToMove/2 && !moved) {
			StartCoroutine(Vector.MoveFromTo(transform, transform.position, new Vector3(curPos.x, y-heightToMove/2, curPos.z), timeToMove/2));
		} else if(transform.position.y == y-heightToMove/2 && !moved) {
			StartCoroutine(Vector.MoveFromTo(transform, transform.position, new Vector3(curPos.x, y+heightToMove/2, curPos.z), timeToMove/2));
		}
		curPosition = transform.position;
		velocity = (lastPosition - curPosition) / Time.deltaTime;
		lastPosition = transform.position;
		moved = velocity != Vector3.zero;
	}

}
