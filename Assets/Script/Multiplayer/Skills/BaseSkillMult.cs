using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

//This is the parent class for all of the skills script
//This script is responsible in assigning basic scripts of the player in case
//one of the skill child class needs it
//This script will also assign the activation of the skill to the button it is
//assigned to and set the picture of the skill to the button
public abstract class BaseSkillMult : MonoBehaviour {
	
	public int skillCost; //Cost of the skill
	public Button button; //The button that the skill will be assigned to
	
	protected string imagePath; //Sprite Image Path
	protected GameObject player; //reference to the player game object
	protected PlayerSkillMult playerSkill; //reference to playerSkill script (control mana bar)
	protected PlayerHealthMult playerHealth; //reference to the player health script
	protected PlayerMovementMult playerMovement; //reference to the player movement script
	protected SkillActivation skillActivation;
	protected GameObject enemyManager;
	
	//Assign references
	protected void Assign() {
		//Assigning player components
		player = gameObject; //Since the skills will be a component to the player
		playerHealth = player.GetComponent <PlayerHealthMult> ();
		playerSkill = player.GetComponent <PlayerSkillMult> ();
		playerMovement = player.GetComponent <PlayerMovementMult> ();
		skillActivation = player.GetComponent<SkillActivation> ();

		button.image.sprite = Resources.Load (imagePath, typeof(Sprite))as Sprite;
		button.GetComponent<Button> ().onClick.AddListener (() => {
			Activate (); }); //Assigning skill effect to the button
	}
	
	
	//Checking whether player have enough mana to activate the skill
	protected void Update () {
		if (playerSkill.currentSkillBar < skillCost) {
			button.interactable = false;
		} 
		else {
			button.interactable = true;
		}
	}

	//For multiplayer, all the activation logic will be done in skillActivation script 
	//Because we can only call [Command] on component that are attached to the player
	public abstract void Activate();
}