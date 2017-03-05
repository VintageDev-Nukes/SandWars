using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public static Transform target;

	void Start() {
#if UNITY_EDITOR
		Camera.main.orthographicSize = 5;
#endif
	}
	
	void Update () {
		if(target){
			//ターゲットを追従
			Vector3 newPosition = new Vector3(target.position.x, target.position.y, -10);
			transform.position = newPosition;
		}
	}
}
