using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	CreateOrList selectedOpt;

	bool connectFailed;

	/*void Awake() {
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
	}*/

	// Use this for initialization
	void Start () {

		if(PlayerPrefs.GetInt("cNum") == 0) {
			Application.LoadLevel("CharacterCreator");
		}

		if(PlayerPrefs.GetInt("scNum") == 0) {
			Application.LoadLevel("CharacterSelector");
		}

	}

	void OnGUI() {

		/*if (!PhotonNetwork.connected)
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
		}*/

		//Botones del menu

		if(GUI.Button(new Rect(50, Screen.height-220, 200, 50), "Buscar partida aleatoria") && Rooms.canCreateRooms) {
			PhotonNetwork.JoinRandomRoom();
		}

		if(GUI.Button(new Rect(50, Screen.height-160, 200, 50), "Ver partidas disponibles") && Rooms.canCreateRooms) {
			selectedOpt = CreateOrList.ListServers;
			PlayerPrefs.SetString("selectedOption", selectedOpt.ToString());
			Application.LoadLevel("MainLevel_Scene");
		}

		if(GUI.Button(new Rect(50, Screen.height-100, 200, 50), "Crear partida") && Rooms.canCreateRooms) {
			selectedOpt = CreateOrList.Create;
			PlayerPrefs.SetString("selectedOption", selectedOpt.ToString());
			Application.LoadLevel("MainLevel_Scene");
		}

		if(GUI.Button(new Rect(Screen.width-170, 20, 150, 50), "Borrar todos mis datos")) {
			PlayerPrefs.DeleteAll();
		}

		/*if(GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), "¡Jugar la versión en desarrollo!")) {
			Application.LoadLevel("MainLevel_Scene");
		}*/

	}

	/*void OnJoinedLobby() {
		Rooms.canCreateRooms = true;
		//Rooms.connected = true;
		Debug.Log("OnJoinedLobby");
	}*/

	/*void OnConnectedToMaster() {
		Debug.Log("OnConnectedToMaster");
	}*/

}
