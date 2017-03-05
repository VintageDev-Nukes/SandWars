using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {

	Texture2D logo;
	Texture2D artwork;
	Texture2D options;
	Texture2D exit;
	Texture2D back;

	public bool ShowLogo = true;
	public string BackScene = "";
	
	// Use this for initialization
	void Start() {
		logo = Resources.Load<Texture2D>("imgs/logo");
		artwork = Resources.Load<Texture2D>("artworks/artwork2");
		options = Resources.Load<Texture2D>("icons/Options");
		exit = Resources.Load<Texture2D>("icons/Exit");
		back = Resources.Load<Texture2D>("icons/Back");
	}

	void OnGUI() {

		GUI.depth = 0;

		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), artwork);

		if(ShowLogo) {
			GUI.DrawTexture(new Rect(Screen.width/2-615/2, 10, 615, 110), logo);
		}

		if(!System.String.IsNullOrEmpty(BackScene)) {
			if(GUI.Button(new Rect(Screen.width-150, Screen.height-50, 50, 50), back)) {
				Load.OnClick(BackScene);
			}
		}

		if(GUI.Button(new Rect(Screen.width-100, Screen.height-50, 50, 50), options)) {
			//Nada
		}

		if (GUI.Button(new Rect(Screen.width-50, Screen.height-50, 50, 50), exit)) {
			Application.OpenURL("http://dynawars.x10host.com/");	
		}

		GUI.depth = 2;

	}

}
