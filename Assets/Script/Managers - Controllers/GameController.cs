using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This sript will firstly assign all the skills in skillmanager to the player,
//and assign the buttons.
//Then it will disable skillmanager
public class GameController : MonoBehaviour {
	
	GameObject player;
	GameObject skillManager;
	Component[] allSkills;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		skillManager = GameObject.FindGameObjectWithTag ("SkillManagerSingle");
		skillManager.SetActive (true);
		allSkills = skillManager.GetComponents<Component> (); //Get the skills inside skillmanager

		//Put the skills to the player component, and then assign the buttons
		for (int i = 1 ; i < allSkills.Length ; i++) {
			var theSkill = player.AddComponent(allSkills[i].GetType ()) as BaseSkill;
			theSkill.button = GameObject.FindGameObjectWithTag ("Skill" + (i)).GetComponent<Button> ();
		}

	}

}