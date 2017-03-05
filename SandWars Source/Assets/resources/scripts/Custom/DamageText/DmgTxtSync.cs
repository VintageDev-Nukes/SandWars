using UnityEngine;
using System.Collections;

public class DmgTxtSync : Photon.MonoBehaviour {

	public object[] obj;

	DamageText dmgtxt;

	// Use this for initialization
	void Start () {
		if(!photonView.isMine) {
			dmgtxt = GetComponent<DamageText>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!photonView.isMine) {
			dmgtxt.obj = obj;
		}
	}
}
