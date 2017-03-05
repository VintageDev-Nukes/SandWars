using UnityEngine;
using System.Collections;

public enum NPCTypes {RepairMan, QuestsMan, ArmsDealer, SkillTrainer, ClanMaker}

public class NPCInvoker : MonoBehaviour {

	public NPCTypes npctype;
	public KeyCode key;
	public bool actived;
	
	// Use this for initialization
	void Start() {
		
	}
	
	void OnGUI() {
		if(actived) {
			if(npctype == NPCTypes.ArmsDealer) {
				
			} else if(npctype == NPCTypes.ClanMaker) {
				
			} else if(npctype == NPCTypes.QuestsMan) {
				
			} else if(npctype == NPCTypes.RepairMan) {
				
			} else if(npctype == NPCTypes.SkillTrainer) {
				
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
