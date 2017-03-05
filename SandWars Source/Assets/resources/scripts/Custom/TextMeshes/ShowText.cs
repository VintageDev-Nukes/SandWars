using UnityEngine;
using System.Collections;

public class ShowText : MonoBehaviour {

	public Transform target;
	public TextMesh tx;
	public float distance;
	public string text;

	// Use this for initialization
	void Start() {
		if(tx == null) {
			tx = gameObject.GetComponent<TextMesh>();
		}
		/*if(target == null) {
			target = PlayerManager.player.transform;
		}*/
	}
	
	// Update is called once per frame
	void Update() {
		if(!Rooms.isLobbyReady) {
			return;
		}
		if(target == null) {
			target = PlayerManager.player.transform;
			return;
		}
		if(Vector3.Distance(target.position, tx.transform.position) <= distance) {
			tx.text = text;
		} else {
			tx.text = "";
		}
	}

}
