using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {

	float timer;

	[HideInInspector]
	public object[] obj;

	bool isNotSet = true;

	// Use this for initialization
	void Start() {

		Vector3 curPos = transform.position;
		Vector3 newPos = new Vector3 (curPos.x, curPos.y+(Random.Range(20, 50)/10), -10);
		StartCoroutine(Vector.MoveFromTo(transform, curPos, newPos, 0.5f));

	}

	void Update() {

		if(obj != null && isNotSet) {

			float damage = (float)obj[0];
			bool critic = (bool)obj[1];
			
			GetComponent<TextMesh>().text = Mathf.Round(damage).ToString();
			float scale = (damage/10)*0.035f;
			transform.localScale = new Vector3(scale, scale, 1);
			if(critic) {
				GetComponent<TextMesh>().color = new Color(1, 0.5f, 0);
			}

			isNotSet = false;

		}

	}

	void FixedUpdate() {
		timer += Time.deltaTime;
		if(timer > 2) {
			Destroy(this.gameObject);
		}
	}

	void ApplyText(object[] obj) {
		this.obj = obj;
	}

}
