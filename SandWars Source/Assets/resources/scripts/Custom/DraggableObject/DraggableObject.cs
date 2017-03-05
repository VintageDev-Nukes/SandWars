using UnityEngine;
using System.Collections;

public class DraggableObject : MonoBehaviour {

	float x;
	float y;
	float z;

	void Start() {
		z = transform.position.z;
	}
	
	// Update is called once per frame
	void Update() {
		x = Input.mousePosition.x;
		y = Input.mousePosition.y;
	}
	
	void OnMouseDrag() {
		Vector3 newpos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, z));
		transform.position = new Vector3(newpos.x, newpos.y, z);
	}

}
