using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

//This script controls what will be displayed on the top middle screen
public class ScoreManagerMult : NetworkBehaviour
{
	
	public int myScore;
	public int enemyScore;
	public GameObject player; //This is the local player, will be set by PlayerScore
	public bool waitingForPlayer;
	public bool gameEnding; //whether game is ending
	public Text text; //text that will be displayed

	GameObject[] players;
	
	void Awake ()
	{
		text = GetComponent <Text> ();
		myScore = 0;
		enemyScore = 0;
		waitingForPlayer = false;
		gameEnding = false;

	}

	public override void OnStartServer ()
	{

		waitingForPlayer = true;

		base.OnStartServer ();

	}


	void Update ()
	{
		int Length = GameObject.FindGameObjectsWithTag("Player").Length;
		
		
		if (Length == 2)
		{
			waitingForPlayer = false;
		}

		if (waitingForPlayer == true)
		{
			var ipaddress = Network.player.ipAddress;

			text.text = "Waiting for opponent to join . . .\nip address: " + ipaddress;
		}

		if (waitingForPlayer == false)
		{

			players = GameObject.FindGameObjectsWithTag ("Player");
			
			if (players.Length == 2) {
				myScore = player.GetComponent<PlayerScore>().score;

				//Get the enemy score
				for (int i = 0 ; i < players.Length ; i++) {
					if(players[i].transform.position.y!= player.transform.position.y) {
						enemyScore = players[i].GetComponent<PlayerScore>().score;
					}
				}
			}

			//Only display this when game still going on
			//When it is ending, the text displayed will be controlled by GameControllerMult
			if (!gameEnding) {
				text.text = "MyScore: " + myScore + "\nEnemyScore: " + enemyScore;
			}
		}
		
	}
}