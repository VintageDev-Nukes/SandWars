using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class InRoomChat : Photon.MonoBehaviour 
{
    public Rect GuiRect = new Rect(0,0, 250,300);
    public bool IsVisible = true, IsChatShown, AlignBottom;
    public List<string> messages = new List<string>();
    private string inputLine = "";
    private Vector2 scrollPos = Vector2.zero, lastPos = Vector2.zero;

    public static readonly string ChatRPC = "Chat";

    public void Start()
    {
        if (this.AlignBottom)
        {
            this.GuiRect.y = Screen.height - this.GuiRect.height;
        }
    }

    public void OnGUI()
    {
        if (!this.IsVisible || PhotonNetwork.connectionStateDetailed != PeerState.Joined)
        {
            return;
        }
        
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(this.inputLine))
            {
                this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);
                this.inputLine = "";
                GUI.FocusControl("");
                return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
            }
            else
            {
                GUI.FocusControl("ChatInput");
            }
        }

        GUI.SetNextControlName("");
        GUILayout.BeginArea(this.GuiRect);

		if(IsChatShown) {
        	scrollPos = GUILayout.BeginScrollView(scrollPos);
			lastPos = scrollPos;
		} else {
			scrollPos = Vector2.zero;
		}
        GUILayout.FlexibleSpace();
        for (int i = messages.Count - 1; i >= 0; i--)
        {
            GUILayout.Label(messages[i]);
        }
		if(IsChatShown) {
        	GUILayout.EndScrollView();
		}

		if(IsChatShown) {

	    	GUILayout.BeginHorizontal();
	    	GUI.SetNextControlName("ChatInput");
	    	inputLine = GUILayout.TextField(inputLine);
	    	if (GUILayout.Button("Send", GUILayout.ExpandWidth(false)))
	    	{
	        	this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);
	        	this.inputLine = "";
	        	GUI.FocusControl("");
	    	}
	    	GUILayout.EndHorizontal();

		} else {
			GUILayout.Space(25);
		}

		GUILayout.EndArea();

    }

	public void Update() {
		if(Input.GetKeyDown(KeyCode.T)) {
			if(!IsChatShown) {
				scrollPos = lastPos;
			}
			IsChatShown = (IsChatShown == true) ? false : true;
		}
	}

    [RPC]
    public void Chat(string newLine, PhotonMessageInfo mi)
    {
        string senderName = "anonymous";

        if (mi != null && mi.sender != null)
        {
            if (!string.IsNullOrEmpty(mi.sender.name))
            {
                senderName = mi.sender.name;
            }
            else
            {
                senderName = "player " + mi.sender.ID;
            }
        }

        this.messages.Add(senderName +": " + newLine);
    }

    public void AddLine(string newLine)
    {
        this.messages.Add(newLine);
    }
}