using UnityEngine;
using System.Collections;

public enum BuildTypes {Bank, Market, Storage}

public class BuildInvoker : MonoBehaviour {

	public BuildTypes buildtype;
	public KeyCode key;
	public bool actived;

	// Use this for initialization
	void Start() {
		
	}

	void OnGUI() {
		if(actived) {
			if(buildtype == BuildTypes.Bank) {

			} else if(buildtype == BuildTypes.Market) {
				
			} else if(buildtype == BuildTypes.Storage) {
				
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(key.ToString())) {
			actived = (actived == true) ? false : true;
		}
	}

}
