using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Controls the player health behaviour
//It also handles setting the score playerPrefs and ending the game
public class PlayerHealth : MonoBehaviour {
	
	public int startingHealth; 
	public int currentHealth;
	public Slider healthSlider; //reference sliderUI game object - we must include UnityEngine.UI
	public SceneFadeInOut scene; //Scene will fade out when the player health reach zero
	
	Animator anim; //Reference to player's animator
	AudioSource playerAudio;//reference to player audio
	PlayerMovement playerMovement; //reference to player movement script
	bool isDead;
	
	void Awake () {
		anim = GetComponent <Animator> ();
		playerMovement = GetComponent <PlayerMovement> ();
		currentHealth = startingHealth;
		healthSlider.value = currentHealth;
	}
		
	public void TakeDamage (int amount) //When enemy deal damage, THEY will call this function
	{	
		//for unit testing
		int previousHealth = currentHealth; 
		currentHealth -= amount;
		if (currentHealth < 0) {
			currentHealth = 0;
		}

		if (currentHealth != previousHealth - amount) {
			IntegrationTest.Fail (gameObject);
		} 
		IntegrationTest.Pass(gameObject);
		
		healthSlider.value = currentHealth; //Adjusting the player's health bar UI

		
		if(currentHealth <= 0 && !isDead)
		{
			Death ();
		}
	}
	
	public void Heal (int amount) {
		currentHealth += amount;
		if (currentHealth > 100) {
			currentHealth = 100;
		}
		healthSlider.value = currentHealth;
	}
	
	//Called when player's health reach zero
	void Death ()
	{
		isDead = true;
		
		anim.SetTrigger ("Death");
		
		playerMovement.enabled = false;
		
		//Game over
		int score = ScoreManager.score;

		if (!PlayerPrefs.HasKey ("HighScore")) {
			PlayerPrefs.SetInt ("HighScore", score);
		} else if (score > PlayerPrefs.GetInt("HighScore")) {
			PlayerPrefs.SetInt ("HighScore",score);
		}
		PlayerPrefs.SetInt ("LatestScore", score);

		Application.LoadLevel ("GameOver");
	}

}

