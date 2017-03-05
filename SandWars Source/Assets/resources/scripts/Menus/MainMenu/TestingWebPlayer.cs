using UnityEngine;
using System.Collections;

public class TestingWebPlayer : MonoBehaviour {

	string KeyToSet = "";

	// Use this for initialization
	void OnGUI() {

		KeyToSet = GUI.TextField(new Rect(10, 10, 320, 20), KeyToSet);

		if(GUI.Button(new Rect(10, 40, 100, 50), "Guardar")) {
			PlayerPrefs.SetString("TestingString", KeyToSet);
		}

		if(GUI.Button(new Rect(120, 40, 100, 50), "Leer")) {
			KeyToSet = PlayerPrefs.GetString("TestingString");
		}

		if(GUI.Button(new Rect(230, 40, 100, 50), "Borrar todo")) {
			PlayerPrefs.DeleteAll();
		}

	}

}
