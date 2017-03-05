using UnityEngine;
using System.Collections;

public class ParallaxScript : MonoBehaviour {

	public Transform target;
	public float parallaxFactor = 5;
	public float deepPosition = 10;
	public float heightMargin = 0;

	// Update is called once per frame
	void Update() {

		if(!Rooms.isLobbyReady) {
			return;
		}

		Vector3 tgpos = target.position;
		transform.position = new Vector3(tgpos.x/parallaxFactor, tgpos.y/parallaxFactor+heightMargin, deepPosition);

	}

}
