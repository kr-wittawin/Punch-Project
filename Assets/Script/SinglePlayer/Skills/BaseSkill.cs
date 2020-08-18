using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This script is the parent class for all of the skills script
//This script will assign the player components that the skills might need to 
//use
//This script will also assign the image of the skill to the button
public abstract class BaseSkill : MonoBehaviour {
	
	public int skillCost; //Cost of the skill
	public Button button;
	
	protected string imagePath; //Sprite Image Path
	protected GameObject player; //reference to the player game object
	protected PlayerSkill playerSkill; //reference to playerSkill script (control mana bar)
	protected PlayerHealth playerHealth; //reference to the player health script
	protected PlayerMovement playerMovement; //reference to the player movement script
	protected GameObject enemyManager;
	
	//Assign references
	protected void Assign() {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();
		playerSkill = player.GetComponent <PlayerSkill> ();
		playerMovement = player.GetComponent <PlayerMovement> ();
		enemyManager = GameObject.FindGameObjectWithTag ("EnemyManager");
		//Assign player components
		button.image.sprite = Resources.Load(imagePath,typeof(Sprite))as Sprite;
		button.GetComponent<Button>().onClick.AddListener(() => { Activate(); } ); //Assigning skill effect to the button
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
	
	public abstract void Activate();
}