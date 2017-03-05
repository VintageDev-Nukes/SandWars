using UnityEngine;
using System;
using System.Collections;

public class GameGUI : MonoBehaviour {

	Texture2D blackSquare,HPSquare, MPSquare, SPSquare;
	float HP, MP, SP, MaxHP, MaxMP, MaxSP, tempHP, tempMP, tempSP, wHP, wMP, wSP;

	public PlayerController player;

	[HideInInspector]
	public bool deadScreen, playersTab;

	private float playerlistScroll, timer, subTimer, uptPingRate = 0.5f;
	private string ping, lastPing, myPing, myLastPing;

	// Use this for initialization
	void Start () {
		blackSquare = new Texture2D(1,1);
		blackSquare.SetPixel(0,0,Color.black);
		blackSquare.Apply();
		HPSquare = new Texture2D(1,1);
		HPSquare.SetPixel(0,0,Color.red);
		HPSquare.Apply();
		MPSquare = new Texture2D(1,1);
		MPSquare.SetPixel(0,0,Color.blue);
		MPSquare.Apply();
		SPSquare = new Texture2D(1,1);
		SPSquare.SetPixel(0,0,Color.green);
		SPSquare.Apply();
	}

	void OnGUI() {

		if(!Rooms.isLobbyReady) {
			return;
		}

		if(wHP < 0) {
			wHP = 0;
		}

		if(wMP < 0) {
			wMP = 0;
		}

		if(wSP < 0) {
			wSP = 0;
		}

		GUI.DrawTexture(new Rect(10, 10, 110, 20), blackSquare);
		GUI.DrawTexture(new Rect(10, 35, 110, 20), blackSquare);
		GUI.DrawTexture(new Rect(10, 60, 110, 20), blackSquare);
		GUI.DrawTexture(new Rect(15, 15, wHP, 10), HPSquare);
		GUI.DrawTexture(new Rect(15, 40, wMP, 10), MPSquare);
		GUI.DrawTexture(new Rect(15, 65, wSP, 10), SPSquare);

		if(String.IsNullOrEmpty(myLastPing)) {
			myPing = PhotonNetwork.GetPing()+" ms";
		} else {
			myPing = myLastPing;
		}

		GUI.Label(new Rect(Screen.width-110, Screen.height-40, 100, 30), myPing, new GUIStyle(GUI.skin.label) {alignment = TextAnchor.LowerRight});

		if(subTimer > uptPingRate) {
			myLastPing = PhotonNetwork.GetPing()+" ms";
			subTimer = 0;
		}

		if(deadScreen) {

			GUI.depth = 2;
			
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Resources.Load<Texture2D>("imgs/RedScreen"));
			
			GUI.Label(new Rect(Screen.width/2-250, 25, 500, 50), "Has muerto.", new GUIStyle(GUI.skin.label) {fontSize = 24, alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
			
			if(GUI.Button(new Rect(Screen.width/2-50, 100, 100, 30), "Reespawnear")) {
				GameObject.Find("MainScripts").GetComponent<PlayerManager>().Respawn();
			}
			
			GUI.depth = 0;

		}

		if(playersTab) {
			float margin = 0;
			float iElements = PhotonNetwork.playerList.Length;
			foreach (PhotonPlayer player in PhotonNetwork.playerList){

				if(String.IsNullOrEmpty(lastPing)) {
					ping = player.customProperties["Ping"].ToString()+" ms";
				} else {
					ping = lastPing;
				}

				/*GUI.Box(new Rect(Screen.width/2-125, Screen.height/2-100, 250, 200), "");
				GUI.BeginGroup(new Rect(Screen.width/2-125, Screen.height/2-100, 250, 200));
				float pad = 20;
				if(25*iElements>=200) {
					playerlistScroll = GUI.VerticalSlider(new Rect(240, 0, 10, 200), playerlistScroll, 0, 25*iElements);
					pad = 0;
				}
				GUI.BeginGroup(new Rect(0, playerlistScroll, 250, 25*iElements));*/
				GUI.BeginGroup(new Rect(Screen.width/2-125, Screen.height/2-(25*iElements), 250, 25*iElements));
				GUI.Box(new Rect(0, margin, 200, 25), player.name); //+pad
				GUI.Box(new Rect(200, margin, 50, 25), ping);
				GUI.EndGroup();
				margin += 25;
				/*GUI.EndGroup();
				GUI.EndGroup();*/

				if(timer > uptPingRate) {
					lastPing = player.customProperties["Ping"].ToString()+" ms";
					timer = 0;
				}

			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
		if(!Rooms.isLobbyReady) {
			return;
		}
	
		HP = player.HP;
		MP = player.MP;
		SP = player.SP;

		MaxHP = player.MaxHP;
		MaxMP = player.MaxMP;
		MaxSP = player.MaxSP;

		if(tempHP != HP) {
			wHP = HP*100/MaxHP;
		}

		if(tempMP != MP) {
			wMP = MP*100/MaxMP;
		}

		if(tempSP != SP) {
			wSP = SP*100/MaxSP;
		}

		tempHP = HP;
		tempMP = MP;
		tempSP = SP;

		if(Input.GetKeyDown(KeyCode.Tab)) {
			playersTab = true;
		} else if(Input.GetKeyUp(KeyCode.Tab)) {
			playersTab = false;
		}

		/*string summonize = "";

		foreach (PhotonPlayer pl in PhotonNetwork.playerList){

			summonize += "[Player: "+pl.name+"]"+System.Environment.NewLine+"============="+System.Environment.NewLine;
			
			foreach (DictionaryEntry entry in pl.allProperties)
			{
				summonize += System.String.Format("KeyName: {0}, Value: {1}"+System.Environment.NewLine, entry.Key, entry.Value);
			}
			
		}

		summonize += System.Environment.NewLine;

		Debug.Log(summonize);*/

	}

	void FixedUpdate() {
		timer += Time.deltaTime;
		subTimer += Time.deltaTime;
	}

	public void InvokeDeadScreen() {
		deadScreen = true;
	}

}
