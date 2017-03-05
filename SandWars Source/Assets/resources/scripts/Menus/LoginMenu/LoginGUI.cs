using UnityEngine;
using System.Collections;

public class LoginGUI : MonoBehaviour {

	Texture2D logo;
	Texture2D artwork;
	
	string username = "user";
	string password = "pass";
	
	bool remember = false;
	
	// Use this for initialization
	void Start () {
		logo = Resources.Load<Texture2D>("imgs/logo");
		artwork = Resources.Load<Texture2D>("artworks/artwork1");
	}
	
	void OnGUI() {
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), artwork);
		GUI.DrawTexture(new Rect(Screen.width/2-615/2, 10, 615, 110), logo);
		GUI.Window(0, new Rect(Screen.width/2-200, Screen.height/2-150, 400, 300), LoginWindow, "Login");
	}
	
	void LoginWindow(int id) {
		
		GUI.Label(new Rect(50, 72.5f, 75, 20), "Nombre");
		GUI.Label(new Rect(50, 112.5f, 75, 20), "Contraseña");
		
		username = GUI.TextField(new Rect(125, 70, 225, 25), username);
		password = GUI.PasswordField(new Rect(125, 110, 225, 25), password, '*');
		
		remember = GUI.Toggle(new Rect(125, 155, 150, 20), remember, "¿Recordar contraseña?");
		
		if(GUI.Button(new Rect(200-50, 195, 100, 40), "Conectarse")) {
			DoLogin();
		}
		
		//Abajo poner los errores del logeo
		
	}
	
	void DoLogin() {
		if(username == "user" && password == "pass") {
			Application.LoadLevel("CharacterSelector");
		}
	}

}
