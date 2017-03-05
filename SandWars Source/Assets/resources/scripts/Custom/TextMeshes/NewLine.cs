using UnityEngine;
using System;
using System.Collections;

public class NewLine : MonoBehaviour {

	public TextMesh tx;
	public string chartofind = ";";
	bool statechanged;
	string tempStr;

	// Use this for initialization
	void Start() {
		if(tx == null) {
			tx = gameObject.GetComponent<TextMesh>();
		}
	}

	// Update is called once per frame
	void Update() {
		if(tempStr != tx.text) {
			statechanged = true;
		}
		if(!String.IsNullOrEmpty(tx.text) && statechanged) {
			tx.text = tx.text.Replace(chartofind, "\n");
			statechanged = false;
		}
		tempStr = tx.text;
	}

}
