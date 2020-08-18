using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

// In multiplayer, all of the damage calculation is done on the server side
// Client simply mimic the changes of the player's current health
// The same apply with death animation
public class PlayerHealthMult : NetworkBehaviour {
	
	public int startingHealth; 
	public Texture box; //texture for player health bar

	[SyncVar]
	public string playerName; //This will be the name displayed under the sprite

	[SyncVar]
	public int currentHealth;
	
	//This does not work?
//	SceneFadeInOut scene; //Scene will fade out when the player health reach zero

	Animator anim; //Reference to player's animator
	AudioSource playerAudio;//reference to player audio
	PlayerMovementMult playerMovement; //reference to player movement script
	bool isDead;
	
	void Awake () {
		
		anim = GetComponent <Animator> ();
		playerMovement = GetComponent <PlayerMovementMult> ();
		currentHealth = startingHealth;

		//Get scene
//		scene = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<SceneFadeInOut> ();
		
	}

	//When enemy deal damage, THEY will call this function
	public void TakeDamage (int amount) 
	{	
		//Only calculate damage on server
		//Synchronise health and mana bar by SyncVar
		if (!isServer) {
			return;
		}

		currentHealth -= amount;
		
		if (currentHealth < 0) {
			currentHealth = 0;
		}

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
	}
	
	//Called when player's health reach zero
	public void Death ()
	{
		isDead = true;
		
		RpcDeathAnim ();
		
		playerMovement.enabled = false;

	}

	//We only change animations in server side
	//And by using [ClientRpc], we sync it to the client
	[Command]
	void CmdDeathAnim() {
		RpcDeathAnim ();
	}

	//So client can see the animation as well
	[ClientRpc]
	void RpcDeathAnim() {
		anim.SetTrigger ("Death");
	}

	//This draw the player healthbar
	void OnGUI () {
		Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
		float health = (float)currentHealth;
		//draw health bar background
		GUI.color = Color.white;
		GUI.DrawTexture (new Rect (pos.x - 26, Screen.height - pos.y - 47, 50, 8), box);
		
		//draw health bar amount
		GUI.color = Color.red;
		GUI.DrawTexture (new Rect (pos.x - 26, Screen.height - pos.y - 46, health / 2, 8), box);

		//draw player name
		GUI.color = Color.black;
		GUI.Label (new Rect(pos.x - 30, Screen.height - pos.y + 20 ,100,100) , playerName);

	}
}