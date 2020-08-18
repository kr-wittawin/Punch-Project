using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

//This script will control the game from start from finish
//The major things that this script do:
//	1.If it is the server, gives name to the players so its easy for them to   
//    know which player are they controlling
//  2.If a client disconnect unexpectedly (before his/her health goes to zero,
//    this script will move the server player to gameover scene
//  3.It sets the score of the game, and win/lose playerPrefs
//  4.When someone dies, it will kills all of the enemies and stop the server
//    after 3 seconds, ending the game.

public class GameControllerMult : NetworkBehaviour {
	
	public GameObject enemyManagerPlayer1;
	public GameObject enemyManagerPlayer2;
	//The local player, we set this in playerMovementMult.
	public GameObject targetPlayer; 

	GameObject skillManager;
	GameObject opponent; //the opponent player object
	GameObject scoreText; //reference to score text
	ScoreManagerMult scoreManager; //Reference to the GUI text that display the score
	NetworkManager netManager;
	Component[] allSkills;
	int counter; //We used this to control some of the stuff that we only wants to happens once
	bool gameEnding; //Whether someone has won
	bool gameStart; //Whether both player have join and game has started
	float endingTime; //Wait for 1 seconds then end the game
	
	// Use this for initialization
	void Start () {
		//Rest the latestscore PlayerPrefs, otherwise if the game ends without any player
		//killing any of the enemies, then the score displayed at the end would be the one
		//from the player last game
		PlayerPrefs.SetInt ("LatestScore", 0);

		//We want to wait for both players to connect before spawning enemy
		enemyManagerPlayer1.SetActive (false);
		enemyManagerPlayer2.SetActive (false);
		
		//The image blocks our view when editing the game, so its only activated when the game start
		GameObject.FindGameObjectWithTag ("Skill1").GetComponent<Image> ().enabled = true;

		//Get the network manager
		netManager = GameObject.FindGameObjectWithTag ("NetManager").GetComponent<NetworkManager> ();

		counter = 0;
		gameEnding = false;
		gameStart = false;
		endingTime = 0;

		scoreText = GameObject.FindGameObjectWithTag ("ScoreText");
		scoreManager = scoreText.GetComponent<ScoreManagerMult> ();
	}
	
	
	// Update is called once per frame
	void Update () {

		//Get all the players gameObject on the scene
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		//Waiting for both client and server player to connect

		//Only server will be able to check this as if its the server that disconnect in 
		//the middle of the gameplay, the client will get disconnected automatically.
		if (gameStart == true && players.Length < 2) {
			//Someone has disconnect, end the game
			PlayerPrefs.SetInt ("LatestScore", targetPlayer.GetComponent<PlayerScore>().score);
			GameOver ();
		}

		if (players.Length == 2) {
			//When gameStart = true, the game has begun and thus the number of players
			//must always be 2
			gameStart = true; 

			//Get the opposition
			for (int i = 0 ; i < players.Length ; i++) {
				if (players[i].transform.position.y != targetPlayer.transform.position.y) {
					opponent = players[i];
				}
			}

			//Only activate once
			if (counter == 0) {
				if (!isServer) {
					//Disactivate the 'host' and 'client' button
					GameObject.FindGameObjectWithTag("InitialMultGUI").SetActive(false);
				}
				//Disactivate home button so player cant quit when playing multiplayer
				GameObject.FindGameObjectWithTag("HomeButton").SetActive(false);

				//Execute only if its the server to make sure that the first player will always be
				//the host.
				if (isServer) {
					players[0].GetComponent<PlayerHealthMult>().playerName = "Player1"; //First element is always the host
					players[1].GetComponent<PlayerHealthMult>().playerName = "Player2"; //Second element is always the client
				}

				//Do not do this again part of the update again.
				counter +=1;
				
			}
			
			//We only activate enemyManager on server as server will be the one who does enemy spawning
			if (isServer) {
				enemyManagerPlayer1.SetActive (true);
				enemyManagerPlayer2.SetActive (true);
			}

			//Handle GameOver
			int ourHealth = targetPlayer.GetComponent<PlayerHealthMult>().currentHealth;
			int opponentHealth = opponent.GetComponent<PlayerHealthMult>().currentHealth;
			if (ourHealth <= 0) {
				lose(targetPlayer);
			}
			else if (opponentHealth <= 0) {
				win(targetPlayer);
			}
			else if (ourHealth <= 0 && opponentHealth <= 0) {
				draw(targetPlayer);
			}

			//Ending the game
			//We wait for 3 seconds to make sure both client and server has update
			//their playerPrefs (win,lose,draw,and highscore)
			//The server side will be the one who ends the game
			if (gameEnding == true) {

				//Let the server disconnect
				if (isServer) {
					//Destroy all enemies
					GameObject[] enemy =  GameObject.FindGameObjectsWithTag ("Enemy");
					for(var i = 0 ; i < enemy.Length ; i ++)
						Destroy(enemy[i]);

					//Increment endingTime counter
					endingTime += Time.deltaTime;
					if (endingTime >= 3) {
						GameOver ();
					}
				}
			}
		} 
	}
	
	void win (GameObject player){
		//If the game have not end yet
		if (!gameEnding) {
			scoreManager.gameEnding = true;
			scoreManager.text.text = "YOU HAVE WON!"; //Update the text that will be displayed
			//Add win to player's stats
			if (!PlayerPrefs.HasKey ("GamesWon")) {
				PlayerPrefs.SetInt ("GamesWon", 1);
			} else {
				PlayerPrefs.SetInt ("GamesWon", PlayerPrefs.GetInt ("GamesWon") + 1);
			}
			//record latest score
			PlayerPrefs.SetInt ("LatestScore", player.GetComponent<PlayerScore>().score);
			gameEnding = true;
		}
	}


	void lose (GameObject player){
		//If the game have not end yet
		if (!gameEnding) {
			scoreManager.gameEnding = true;
			scoreManager.text.text = "YOU HAVE LOST!"; //Update the text that will be displayed
			//Add loss to player's stats
			if (!PlayerPrefs.HasKey ("GamesLost")) {
				PlayerPrefs.SetInt ("GamesLost", 1);
			} else {
				PlayerPrefs.SetInt ("GamesLost", PlayerPrefs.GetInt ("GamesLost") + 1);
			}
			//record latest score
			PlayerPrefs.SetInt ("LatestScore", player.GetComponent<PlayerScore>().score);
			gameEnding = true;
		}
	}

	void draw (GameObject player){
		//If the game have not end yet
		if (!gameEnding) {
			scoreManager.gameEnding = true;
			scoreManager.text.text = "YOU HAVE DRAWN!"; //Update the text that will be displayed
			//Add draw to player's stats
			if (!PlayerPrefs.HasKey ("GamesDraw")) {
				PlayerPrefs.SetInt ("GamesDraw", 1);
			} else {
				PlayerPrefs.SetInt ("GamesDraw", PlayerPrefs.GetInt ("GamesDraw") + 1);
			}
			//record latest score
			PlayerPrefs.SetInt ("LatestScore", player.GetComponent<PlayerScore>().score);
			gameEnding = true;
		}
	}

	//This will stop the server
	//The reason why only server will be allowed to access this function is 
	//because when we stop the server, it will also stop all of the clients 
	//that currently connected.
	//Thus, by not allowing the client to disconnect by itself, we can make sure that both
	//game ends at the same time.
	void GameOver(){
		if (isServer) {
			netManager.StopServer ();
		}
		netManager.StopClient ();
	}

}
