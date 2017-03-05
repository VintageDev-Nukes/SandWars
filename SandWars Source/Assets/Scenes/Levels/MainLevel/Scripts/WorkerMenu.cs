// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkerMenu.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CreateOrList {Create, ListServers}

public class WorkerMenu : Photon.MonoBehaviour
{
    private string roomName = "SandWars Room";

    private Vector2 scrollPos = Vector2.zero;

    private bool connectFailed = false;

    public static readonly string SceneNameMenu = "MainMenu";

    public static readonly string SceneNameGame = "MainLevel_Game";

	public CreateOrList CreateGameOrListAvServers;

    public void Awake()
    {
        /*// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;

        // the following line checks if this client was just created (and not yet online). if so, we connect
        if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
        {
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings("1.0");
        }

        // generate a name for this player, if none is assigned yet
        if (String.IsNullOrEmpty(PhotonNetwork.playerName))
        {
			PhotonNetwork.playerName = PlayerPrefs.GetString("c"+PlayerPrefs.GetInt("scNum").ToString()+"_Name");
        }

        // if you wanted more debug out, turn this on:
        // PhotonNetwork.logLevel = NetworkLogLevel.Full;*/

		if(!System.String.IsNullOrEmpty(PlayerPrefs.GetString("selectedOption"))) {
			CreateGameOrListAvServers = (CreateOrList)Enum.Parse(typeof(CreateOrList), PlayerPrefs.GetString("selectedOption"));
			PlayerPrefs.DeleteKey("selectedOption");
		}

    }

    public void OnGUI()
    {
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
                GUILayout.Label(String.Format("Server: {0}", new object[] {PhotonNetwork.ServerAddress}));
                GUILayout.Label("AppId: " + PhotonNetwork.PhotonServerSettings.AppID);
                
                if (GUILayout.Button("Try Again", GUILayout.Width(100)))
                {
                    this.connectFailed = false;
                    PhotonNetwork.ConnectUsingSettings("1.0");
                }
            }

            return;
        }

		GUI.skin.box.fontStyle = FontStyle.Bold;

		if(CreateGameOrListAvServers == CreateOrList.Create) {

			GUI.Box(new Rect((Screen.width - 400) / 2, (Screen.height - 350) / 2, 400, 300), "Crear partida");
			GUILayout.BeginArea(new Rect((Screen.width - 400) / 2 + 10, (Screen.height - 350) / 2, 380, 300));

	        GUILayout.Space(40);

	        // Join room by title
	        GUILayout.BeginHorizontal();
			GUILayout.Label("Nombre de la partida", GUILayout.Width(130));
			this.roomName = GUILayout.TextField(this.roomName); //new GUIStyle(GUI.skin.textField) {margin = new RectOffset(-5, 15, 0, 0)}

			GUILayout.EndHorizontal();

			GUILayout.Space(10);

			GUILayout.BeginArea(new Rect(0, 230, 400, 50));
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			if (GUILayout.Button("Crear partida", GUILayout.Width(100), GUILayout.Height(50)) && Rooms.canCreateRooms)
			{
				PhotonNetwork.CreateRoom(this.roomName, new RoomOptions() { maxPlayers = 10 }, null);
			}

			//Opciones que faltan a poner, gamemode (segun el mostrar o no mostrar el maximo de jugadores)
			
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndArea();

			GUILayout.EndArea();

			// Create a room (fails if exist!)

		} else if(CreateGameOrListAvServers == CreateOrList.ListServers) {

			GUI.Box(new Rect((Screen.width - 400) / 2, (Screen.height - 350) / 2, 400, 300), "Encontrar partida");
			GUILayout.BeginArea(new Rect((Screen.width - 400) / 2 + 10, (Screen.height - 350) / 2, 380, 300));

			GUILayout.Space(40);

	        if (PhotonNetwork.GetRoomList().Length == 0)
	        {
	            GUILayout.Label("No hay partidas disponibles.");
	            GUILayout.Label("Las partidas se mostrarán aquí cuando haya alguna disponible.");
	        }
	        else
	        {
				GUILayout.Label(PhotonNetwork.countOfPlayers + " usuarios están jugando en " + PhotonNetwork.GetRoomList().Length + " partidas.");

	            // Room listing: simply call GetRoomList: no need to fetch/poll whatever!
	            this.scrollPos = GUILayout.BeginScrollView(this.scrollPos);
	            foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
	            {
	                GUILayout.BeginHorizontal();
	                GUILayout.Label(roomInfo.name + " " + roomInfo.playerCount + "/" + roomInfo.maxPlayers);
					if (GUILayout.Button("Join") && Rooms.canCreateRooms)
	                {
	                    PhotonNetwork.JoinRoom(roomInfo.name);
	                }

	                GUILayout.EndHorizontal();
	            }

	            GUILayout.EndScrollView();
	        }

			GUILayout.EndArea();

		}
       
    }

    // We have two options here: we either joined(by title, list or random) or created a room.
    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        PhotonNetwork.LoadLevel(SceneNameGame);
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Photon.");
    }

    public void OnFailedToConnectToPhoton(object parameters)
    {
        this.connectFailed = true;
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
    }

	void OnJoinedLobby() {
		Rooms.canCreateRooms = true;
		//Rooms.connected = true;
		Debug.Log("OnJoinedLobby");
	} 

}
