using UnityEngine;
using System.Collections;

public class Vector {

	public static IEnumerator MoveFromTo(Transform obj, Vector3 pointA, Vector3 pointB, float time) {
		if (obj.position != pointB) {               // Do nothing if already moving
			float t = 0f;
			while (t < 1.0f) {
				t += Time.deltaTime / time; // Sweeps from 0 to 1 in time seconds
				obj.position = Vector3.Lerp(pointA, pointB, t); // Set position proportional to t
				yield return 0;    // Leave the routine and return here in the next frame
			}
		}
		
	}

}
