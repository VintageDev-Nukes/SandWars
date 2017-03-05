using UnityEngine;
using System.Collections;
using PHashtable = ExitGames.Client.Photon.Hashtable;

public class MPManager : Photon.MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		//if(photonView.isMine) {
			PHashtable PlayerCustomProps = new PHashtable();
			PlayerCustomProps.Add("Ping", PhotonNetwork.GetPing());
			//PlayerCustomProps["Ping"] = PhotonNetwork.GetPing();
			PhotonNetwork.player.SetCustomProperties(PlayerCustomProps);
		//}
	}
}
