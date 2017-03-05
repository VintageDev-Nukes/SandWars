using UnityEngine;
using System;
using System.Collections;

//Character Selector Menu GUI

public class CSMenuGUI : MonoBehaviour {

	GUIStyle csCaption = new GUIStyle();

	// Use this for initialization
	void Start() {

		if(PlayerPrefs.GetInt("cNum") == 0) {
			Application.LoadLevel("CharacterCreator");
		}

		//Set GUIStyles
		csCaption.fontSize = 16;
		csCaption.alignment = TextAnchor.MiddleCenter;
		csCaption.normal.textColor = Color.white;

	}

	void OnGUI() {
		GUI.Window(0, new Rect(Screen.width*0.1f, 170, Screen.width*0.8f, Screen.height-230), cSelectionWin, "Selecciona un personaje");
	}

	//Falta el boton para borrar

	void cSelectionWin(int id) {

		GUI.Box(new Rect(25, 35, Screen.width*(0.8f/3)-34, (Screen.height-230)-60), "");

		if(String.IsNullOrEmpty(PlayerPrefs.GetString("c1_Name"))) {
			GUI.Box(new Rect((Screen.width*(0.8f/3)-34+25)/2-50+12, (Screen.height-230)/2-12.5f, 100, 25), "Vacío");
			if(GUI.Button(new Rect((Screen.width*(0.8f/3)-34+25)/2-100+12, (Screen.height-230)-100, 200, 50), "Crear un nuevo personaje")) {
				Application.LoadLevel("CharacterCreator");
			}
		} else {
			GUI.Label(new Rect((Screen.width*(0.8f/3)-34+25)/2-75+12, (Screen.height-230)-135, 150, 30), PlayerPrefs.GetString("c1_Name"), csCaption);
			if(GUI.Button(new Rect((Screen.width*(0.8f/3)-34+25)/2-75+12, (Screen.height-230)-100, 150, 50), "Seleccionar")) {
				PlayerPrefs.SetInt("scNum", 1);
				Application.LoadLevel("Lobby");
			}
			if(GUI.Button(new Rect(Screen.width*(0.8f/3)-34+25-10-24, 45, 24, 24), "X")) {
				PlayerPrefs.SetInt("dlID", 1);
				Application.LoadLevel("CharacterDeletion");
			}
		}

		GUI.Box(new Rect(Screen.width*(0.8f/3)+16, 35, Screen.width*(0.8f/3)-34, (Screen.height-230)-60), "");

		if(String.IsNullOrEmpty(PlayerPrefs.GetString("c2_Name"))) {
			GUI.Box(new Rect((Screen.width*(0.8f/3)-34+(Screen.width*(0.8f/3)*2)+7)/2-50+12, (Screen.height-230)/2-12.5f, 100, 25), "Vacío");
			if(GUI.Button(new Rect((Screen.width*(0.8f/3)-34+(Screen.width*(0.8f/3)*2)+7)/2-100+12, (Screen.height-230)-100, 200, 50), "Crear un nuevo personaje")) {
				Application.LoadLevel("CharacterCreator");
			}
		} else {
			GUI.Label(new Rect((Screen.width*(0.8f/3)-34+(Screen.width*(0.8f/3)*2)+7)/2-75+12, (Screen.height-230)-135, 150, 30), PlayerPrefs.GetString("c2_Name"), csCaption);
			if(GUI.Button(new Rect((Screen.width*(0.8f/3)-34+(Screen.width*(0.8f/3)*2)+7)/2-75+12, (Screen.height-230)-100, 150, 50), "Seleccionar")) {
				PlayerPrefs.SetInt("scNum", 2);
				Application.LoadLevel("Lobby");
			}
			if(GUI.Button(new Rect((Screen.width*(0.8f/3)*2)-52, 45, 24, 24), "X")) {
				PlayerPrefs.SetInt("dlID", 2);
				Application.LoadLevel("CharacterDeletion");	
			}
		}

		GUI.Box(new Rect((Screen.width*(0.8f/3)*2)+7, 35, Screen.width*(0.8f/3)-34, (Screen.height-230)-60), "");

		if(String.IsNullOrEmpty(PlayerPrefs.GetString("c3_Name"))) {
			GUI.Box(new Rect((Screen.width*(0.8f/3)*2)+7+(Screen.width*(0.8f/3)-34)/2-50, (Screen.height-230)/2-12.5f, 100, 25), "Vacío");
			if(GUI.Button(new Rect((Screen.width*(0.8f/3)*2)+7+(Screen.width*(0.8f/3)-34)/2-100, (Screen.height-230)-100, 200, 50), "Crear un nuevo personaje")) {
				Application.LoadLevel("CharacterCreator");
			}
		} else {
			GUI.Label(new Rect((Screen.width*(0.8f/3)*2)+7+(Screen.width*(0.8f/3)-34)/2-75, (Screen.height-230)-135, 150, 30), PlayerPrefs.GetString("c3_Name"), csCaption);
			if(GUI.Button(new Rect((Screen.width*(0.8f/3)*2)+7+(Screen.width*(0.8f/3)-34)/2-75, (Screen.height-230)-100, 150, 50), "Seleccionar")) {
				PlayerPrefs.SetInt("scNum", 3);
				Application.LoadLevel("Lobby");
			}
			if(GUI.Button(new Rect((Screen.width*(0.8f/3)*2)+7+(Screen.width*(0.8f/3)-34)-10-24, 45, 24, 24), "X")) {
				PlayerPrefs.SetInt("dlID", 3);
				Application.LoadLevel("CharacterDeletion");	
			}
		}

	}

}