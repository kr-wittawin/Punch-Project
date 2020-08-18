using UnityEngine;
using System.Collections;

public class CloudScroller : ScrollManager {

	// Update is called once per frame
	void Update () {
		//Nearly identical to parent function, but is indepent of player movement
		//IE, it scrolls all the time, and only in one direction
		float x = Mathf.Repeat (gameObject.GetComponent<Renderer> ().material.GetTextureOffset("_MainTex").x+scroll_speed, 1);
		Vector2 offset = new Vector2 (x, 0);
		gameObject.GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", offset);
	}
}
