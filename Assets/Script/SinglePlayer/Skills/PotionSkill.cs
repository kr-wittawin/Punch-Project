using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This skill will heal the player health by a specific amount
public class PotionSkill : BaseSkill {
	
	public float healPercentage; //How much player HP will be recovered 
	
	int healAmount;
	
	void Awake() {
		healPercentage = 20;
		skillCost = 20;
		imagePath = "Potion";
	}
	
	void Start() {
		base.Assign ();
	}
	
	//Heal the player and reduce the player mana
	public override void Activate ()
	{
		int previousHealth = playerHealth.currentHealth; //For unit testing
		healAmount = (int)(playerHealth.startingHealth * (healPercentage / 100));

		playerSkill.decreaseBar (skillCost);

		playerHealth.Heal (healAmount);
		
		Debug.Log ("Potion activated");
		
		//Check whether we heal correct amount of health
		if (playerHealth.currentHealth == (previousHealth + healAmount)) {
			IntegrationTest.Pass (gameObject);
		} 
		else if (playerHealth.currentHealth != (previousHealth + healAmount)) {
			IntegrationTest.Fail (gameObject);
		}
	}
	
	
}
