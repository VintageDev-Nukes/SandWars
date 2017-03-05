using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//Character Creator Menu GUI

public class CCMenuGUI : MonoBehaviour {

	readonly string[] Sex = new string[] {"Hombre", "Mujer"};

	string charName = "", Genre = "", SelectedJob = "";

	int sexIndex = 0, jobIndex = 0;

	bool paletteWin = false;

	Color finalColor;
	Texture2D plColorPreview;

	bool advancedPalette = true;

	float plRed = 0, plGreen = 0, plBlue = 0;

	float tempRed = 0, tempGreen = 0,tempBlue = 0;

	readonly int finalStrength = 0, finalResistence = 0,finalAgility = 0, finalHability = 0;

	int cNumber = 0; //Number of characters created
	string cID = "0";

	// Use this for initialization
	void Start() {
		plColorPreview = new Texture2D(1, 1);
		cNumber = PlayerPrefs.GetInt("cNum");
		cID = (cNumber+1).ToString();
	}

	void OnGUI() {

		GUI.Window(0, new Rect(50, Screen.height*0.1f, Screen.width*0.3f, Screen.height*0.8f), CharSelection, "Estadísticas");
		GUI.Window(1, new Rect(Screen.width-Screen.width*0.3f-50, Screen.height*0.1f, Screen.width*0.3f, Screen.height*0.8f), CharCustomization, "Personalización");

		if(paletteWin) {

			tempRed = plRed;
			tempGreen = plGreen;
			tempBlue = plBlue;

			GUI.Window(2, new Rect(Screen.width/2-270/2, Screen.height-315-Screen.height*0.175f, 270, 300), Palette, "Selecciona un color");
		}

		if(GUI.Button(new Rect(Screen.width/2-Screen.width*0.05f, Screen.height-Screen.height*0.175f, Screen.width*0.1f, Screen.height*0.075f), "Crear") && !String.IsNullOrEmpty(charName)) {
			PlayerPrefs.SetString("c"+cID+"_Name", charName);
			PlayerPrefs.SetInt("c"+cID+"_iJob", jobIndex);
			PlayerPrefs.SetInt("cNum", cNumber+1);
			Application.LoadLevel("CharacterSelector");
		}

	}

	void CharSelection(int id) {

		GUI.Label(new Rect(10, 30, 150, 20), "Nombre del personaje"); //Random button (?) Buscar pagina web que generaba nombres aleatorios
		charName = GUI.TextField(new Rect(10, 55, 150, 25), charName);

		GUI.Label(new Rect(10, 95, 100, 20), "Género");
		Genre = GUIExt.Slider(new Rect(10, 120, 75, 24), Sex, ref sexIndex);

		GUI.Label(new Rect(10, 160, 100, 20), "Clase");
		SelectedJob = GUIExt.Slider(new Rect(10, 185, 75, 24), Jobs.strJobs, ref jobIndex);

		//Job desc

		GUI.Label(new Rect(10, 230, Screen.width*0.3f-20, 50), "Bonus"+Environment.NewLine+Environment.NewLine+Jobs.JobsDesc[jobIndex]);

		//Final stats

		GUI.Label(new Rect(10, 290, Screen.width*0.3f-20, 50), "Estadísticas finales"+Environment.NewLine+Environment.NewLine);

	}

	void CharCustomization(int id) {

	}

	void Palette(int id) {

		if(advancedPalette) {

			GUI.Label(new Rect(10, 30, 100, 20), "Color");
			
			Texture2D palette = Resources.Load<Texture2D>("imgs/Palette");
			
			GUIStyle buttonEmpty = GUI.skin.GetStyle("Label");
			
			if(GUI.Button(new Rect(10, 20, 258, 200), palette, buttonEmpty)) {
				
				Vector2 mouseFix = new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y);
				Vector2 finalpos = new Vector2(mouseFix.x-34, mouseFix.y);
				
				finalColor = palette.GetPixel((int)finalpos.x, palette.height-(int)finalpos.y);
				
				plColorPreview.SetPixel(0, 0, finalColor);
				plColorPreview.Apply();
				
			}
			
			GUI.DrawTexture(new Rect(10, 230, 60, 60), plColorPreview);

			advancedPalette = GUI.Toggle(new Rect(80, 230, 150, 20), advancedPalette, "Selección avanzada");

			if(GUI.Button(new Rect(80, 260, 100, 30), "Aceptar")) {
				paletteWin = false;
			}
		
		} else {

			GUI.Label(new Rect(10, 20, 70, 30), "Rojo ("+(plRed*255).ToString("F0")+")");
			GUI.Label(new Rect(10, 60, 70, 30), "Verde ("+(plGreen*255).ToString("F0")+")");
			GUI.Label(new Rect(10, 100, 70, 30), "Azul ("+(plBlue*255).ToString("F0")+")");
			
			plRed = GUI.HorizontalSlider(new Rect(90, 25, 160, 30), plRed, 0, 1);
			plGreen = GUI.HorizontalSlider(new Rect(90, 65, 160, 30), plGreen, 0, 1);
			plBlue = GUI.HorizontalSlider(new Rect(90, 105, 160, 30), plBlue, 0, 1);
			
			finalColor = new Color(plRed, plGreen, plBlue, 1);
			
			if(plRed != tempRed || plGreen != tempGreen || plBlue != tempBlue) {
				plColorPreview.SetPixel(0, 0, finalColor);
				plColorPreview.Apply();
			}

			GUI.DrawTexture(new Rect(10, 230, 60, 60), plColorPreview);
			
			advancedPalette = GUI.Toggle(new Rect(80, 230, 150, 20), advancedPalette, "Selección avanzada");
			
			if(GUI.Button(new Rect(80, 260, 100, 30), "Aceptar")) {
				paletteWin = false;
			}

		}

	}

}