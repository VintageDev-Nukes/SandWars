using UnityEngine;
using System.Collections;

public class Rooms {
	public static bool canCreateRooms;
	public static bool isLobby;
	public static bool isLobbyReady;
}

public class Flags {
	public static bool isPvpEnabled = true;
	public static bool canShoot = true;
	public static bool canMove;
	public static float minHeight;
}

public class MPLoader : Photon.MonoBehaviour {

	private bool connectFailed;

	void Awake() {
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.automaticallySyncScene = true;
		
		// the following line checks if this client was just created (and not yet online). if so, we connect
		if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
		{
			// Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
			PhotonNetwork.ConnectUsingSettings("1.0");
		}
		
		// generate a name for this player, if none is assigned yet
		if (System.String.IsNullOrEmpty(PhotonNetwork.playerName))
		{
			string selected = PlayerPrefs.GetInt("scNum").ToString();
			string nickname = PlayerPrefs.GetString("c"+selected+"_Name");
			if(!System.String.IsNullOrEmpty(nickname)) {
				PhotonNetwork.playerName = nickname;
			} else {
				PhotonNetwork.playerName = "Guest"+Random.Range(0, 9999);
			}
		}
		
		// if you wanted more debug out, turn this on:
		// PhotonNetwork.logLevel = NetworkLogLevel.Full;
	}

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {

		/*if(Rooms.canCreateRooms) {
			Application.LoadLevel("Lobby");
		}*/
	
	}

	void OnGUI() {
		if (!PhotonNetwork.connected)
		{
			if (PhotonNetwork.connecting)
			{
				GUILayout.Label("Connecting to: " + PhotonNetwork.ServerAddress);
			}
			else
			{
				GUILayout.Label("Not connected. Check console output. Detailed connection state: " + PhotonNetwork.connectionStateDetailed + " Server: " + PhotonNetwork.ServerAddress);
			}
			
			if (this.connectFailed)
			{
				GUILayout.Label("Connection failed. Check setup and use Setup Wizard to fix configuration.");
				GUILayout.Label(System.String.Format("Server: {0}", new object[] {PhotonNetwork.ServerAddress}));
				GUILayout.Label("AppId: " + PhotonNetwork.PhotonServerSettings.AppID);
				
				if (GUILayout.Button("Try Again", GUILayout.Width(100)))
				{
					this.connectFailed = false;
					PhotonNetwork.ConnectUsingSettings("1.0");
				}
			}
			
			GUI.Box(new Rect(Screen.width/2-150, Screen.height/2-15, 300, 30), "Conectando. Espere un momento, por favor...");
			
			return;
		}
		
		if(!Rooms.canCreateRooms) {
			GUI.Box(new Rect(Screen.width/2-75, Screen.height/2-15, 150, 30), "Entrando a la Lobby...");
		}
	}

	void OnJoinedLobby() {
		Rooms.canCreateRooms = true;
		//Rooms.connected = true;
		Debug.Log("OnJoinedLobby");
	} 

}
