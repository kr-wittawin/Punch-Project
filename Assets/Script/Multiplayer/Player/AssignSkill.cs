using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

// This script purpose is to get all the skills inside the SkillManagerMult gameobject
// And add all of the skills to the player component
// We have to do this as [Command] can only be done if issued on a player GameObject
// [Command] are the way for clients to request to do something on the server
// We need to use [Command] in order to centralise the system (server does all of the
// event handling)
public class AssignSkill : NetworkBehaviour {
	
	GameObject skillManager;
	Component[] allSkills; 

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) {
			return;
		}

		//Get the skill manager 
		skillManager = GameObject.FindGameObjectWithTag ("SkillManagerMult");
		//Get all the skills inside skillManager
		allSkills = skillManager.GetComponents<Component> (); 

		//Assign the skills
		//We start from i = 2, as the first component of skill manager will be its transform
		//and the second will be network identity
		for (int i = 1 ; i < allSkills.Length ; i++) {
			var theSkill = gameObject.AddComponent(allSkills[i].GetType ()) as BaseSkillMult;

			//Assign the skill to a button.
			theSkill.button = GameObject.FindGameObjectWithTag ("Skill" + (i)).GetComponent<Button> ();
		}
	}

}