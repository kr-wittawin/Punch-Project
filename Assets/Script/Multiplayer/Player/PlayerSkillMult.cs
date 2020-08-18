using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

//This script display and calculate the mana/skill bar of the player
//All of the mana calculation is done in server
public class PlayerSkillMult : NetworkBehaviour {
	
	public int startingSkillBar; 
	public Texture box; //player skill bar texture/box

	//[SyncVar] is used so that client will be able to mimic any changes
	//done to the currentSkillBar in server.
	[SyncVar]
	public int currentSkillBar;
		
	void Awake () {

		currentSkillBar = startingSkillBar;
		
	}
	
	public void increaseBar (int amount) {
		currentSkillBar += amount;
		if (currentSkillBar > 100) {
			currentSkillBar = 100;
		}
	}
	
	public void decreaseBar (int amount) {
		currentSkillBar -= amount;
		if (currentSkillBar < 0) {
			currentSkillBar = 0;
		}
	}

	//This draw the mana bar
	void OnGUI () {
		Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
		float mana = (float)currentSkillBar;
		//draw skill bar background
		GUI.color = Color.white;
		GUI.DrawTexture (new Rect (pos.x - 26, Screen.height - pos.y - 40, 52, 8), box);
		
		//draw skill bar amount
		GUI.color = Color.blue;
		GUI.DrawTexture (new Rect (pos.x - 26, Screen.height - pos.y - 41, mana / 2, 8), box);
	}
}