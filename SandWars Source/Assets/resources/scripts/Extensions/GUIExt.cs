using UnityEngine;
using System.Collections;

public class GUIExt {

	public static string Slider(Rect pos, string[] contents, ref int currentIndex) {

		if(GUI.Button(new Rect(pos.x, pos.y, 24, pos.height), "<")) {

			currentIndex--;

			if(currentIndex < 0) {
				currentIndex = contents.Length-1;
			}

		}

		if(GUI.Button(new Rect(pos.x+pos.width+24, pos.y, 24, pos.height), ">")) {

			currentIndex++;
			
			if(currentIndex > contents.Length-1) {
				currentIndex = 0;
			}
			
		}

		GUI.Label(new Rect(pos.x+24, pos.y, pos.width, pos.height), contents[currentIndex], GUI.skin.GetStyle("Box"));

		return contents[currentIndex];

	}

}
