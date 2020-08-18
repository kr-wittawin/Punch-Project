using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//This script handles the score addition to the player
//All of the score addition is done in server, client simply mimic the score
public class PlayerScore : NetworkBehaviour {

	//By using [SyncVar], the client will track any changes to the score variable
	//in server.
	[SyncVar]
	public int score;
		
	GameObject scoreText; //reference to score text
	ScoreManagerMult scoreManager; //Reference to the GUI text that display the score
	
	// Use this for initialization
	void Start () {
		score = 0;
		
		scoreText = GameObject.FindGameObjectWithTag ("ScoreText");
		scoreManager = scoreText.GetComponent<ScoreManagerMult> ();

		//Assign the local player to the scoreManager so it can display the correct
		//score.
		if (isLocalPlayer) {
			scoreManager.player = gameObject;
		}
	}

}