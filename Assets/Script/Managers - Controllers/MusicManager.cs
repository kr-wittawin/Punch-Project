using UnityEngine;
using System.Collections;

//This script will control the music mainly on main menu and gameover scene
public class MusicManager : MonoBehaviour {

	public float delay;
	public AudioSource audioClip;
	// Use this for initialization
	void Start () {
		//If the script is attached to the gameover background music, destroy it when we change scene
		if (Application.loadedLevelName != "GameOver") {
			DontDestroyOnLoad (transform.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine ("musicDelayTimer");
		GameObject[] menuMusic = GameObject.FindGameObjectsWithTag ("MenuMusic");

		//So when we go back to main menu, it wont create another background music
		if (menuMusic.Length == 2) {
			Destroy (menuMusic [1]);
		}

		//Destroy it when we go to single player or multiplayer as they already have their own music
		if (Application.loadedLevelName == "Multiplayer" || Application.loadedLevelName == "Single_Player") {
			Destroy (menuMusic[0]);
		}

	}

	//In case we want a delay before the music start
	IEnumerator musicDelayTimer ()
	{
		yield return new WaitForSeconds (delay);
		audioClip.enabled = true;
	}
}
