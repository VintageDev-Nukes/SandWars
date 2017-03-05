using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour {

	public bool isPvpEnabled;
	public bool canShoot;
	public float minHeight = -1000;

	// Use this for initialization
	void Start() {
		Rooms.isLobby = true;
		Flags.isPvpEnabled = isPvpEnabled;
		Flags.canShoot = canShoot;
		Flags.minHeight = minHeight;
	}
	
	// Update is called once per frame
	void Update() {
	
	}

}
