using UnityEngine;
using System.Collections;

public class CDMenuGUI : MonoBehaviour {

	int cNum = 0;
	int dlID = 0;

	// Use this for initialization
	void Start() {
		cNum = PlayerPrefs.GetInt("cNum");
		dlID = PlayerPrefs.GetInt("dlID");
	}
	
	// Update is called once per frame
	void OnGUI() {
		GUI.Window(0, new Rect(Screen.width/2-150, Screen.height/2-100, 300, 200), Confirm, "¿Deseas eliminar el personaje seleccionado?");
	}

	void Confirm(int id) {
		GUI.Label(new Rect(10, 20, 280, 20), "Los cambios no se pueden deshacer.");
		if(GUI.Button(new Rect(150-50-5, 200-40, 50, 30), "Sí")) {

			//Delete all keys relevant to the user
			PlayerPrefs.DeleteKey("c"+dlID+"_Name");
			PlayerPrefs.DeleteKey("c"+dlID+"_iJob");

			//Delete dlID
			PlayerPrefs.DeleteKey("dlID");

			//And substract one from Characyter Number
			PlayerPrefs.SetInt("cNum", cNum-1);

			//Go to CharacterSelector
			Application.LoadLevel("CharacterSelector");

		}
		if(GUI.Button(new Rect(150+5, 200-40, 50, 30), "No")) {
			Application.LoadLevel("CharacterSelector");
		}
	}

}
