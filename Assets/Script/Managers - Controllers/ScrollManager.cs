using UnityEngine;
using System.Collections;

public class ScrollManager : MonoBehaviour {
	public float scroll_speed;
	protected int direction;
	// Use this for initialization
	void Start () {
		direction = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//calculates the new offset of the background layer
		float x = Mathf.Repeat (gameObject.GetComponent<Renderer> ().material.GetTextureOffset("_MainTex").x+(scroll_speed*direction), 1);
		Vector2 offset = new Vector2 (x, 0);
		//sets the new offset of the background layer
		gameObject.GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", offset);
	}
	
	public void ChangeDirection(int new_direction){
		direction = new_direction;
	}
}
