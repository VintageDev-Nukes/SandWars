using UnityEngine;
using System.Collections;

public class LobbyGUI : MonoBehaviour {

	private bool started;
	private Texture2D black;

	// Use this for initialization
	void Start () {
		black = new Texture2D(1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		black.SetPixel(1,1,Color.black);
		black.Apply();
	}

	void OnGUI() {
		if(Rooms.isLobbyReady) {
			if(!started) {
				GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
				if(GUI.Button(new Rect(Screen.width/2-75, Screen.height/2-25, 150, 50), "Comenzar")) {
					started = true;
					Flags.canMove = true;
				}
			}
		} else {
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
			GUI.Label(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), PhotonNetwork.connectionStateDetailed.ToString(), new GUIStyle(GUI.skin.label) {alignment  = TextAnchor.MiddleCenter});
		}
	}

}
