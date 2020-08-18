using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This skill will remove the player miss cooldown for a period of time
//Basically for this period, the player will be to continue attacking without
//delay even though he/she miss.
public class RemoveCooldownSkill : BaseSkill {
	
	public float reduceTime;
	
	float reduceAmount;
	float originalTimer;
	
	void Awake() {
		skillCost = 60;
		imagePath = "WingedShoes";
		reduceTime = 7;
	}
	
	void Start() {
		base.Assign ();
	}

	public override void Activate ()
	{
		reduceAmount = playerMovement.missTimer;

		playerSkill.decreaseBar (skillCost);

		originalTimer = playerMovement.missTimer;
		//Reduce missTimer to zero
		playerMovement.missTimer -= reduceAmount;

		//For unit testing
		if (playerMovement.missTimer != 0) {
			Debug.Log (playerMovement.missTimer);
			IntegrationTest.Fail(gameObject);
		}

		Debug.Log ("MissTimerSKill Activate");
		StartCoroutine ("timerReduced");

		
		
	}
	//During this period, player wont have miss attack cooldown
	IEnumerator timerReduced ()
	{
		yield return new WaitForSeconds (reduceTime);
		playerMovement.missTimer = originalTimer;
		Debug.Log ("Timer return to normal");
		//For unit testing
		if (playerMovement.missTimer != originalTimer) {
			IntegrationTest.Fail (gameObject);
		}

		//Attack cooldown timer moves as expected
		IntegrationTest.Pass (gameObject);
	}


	
	
}

