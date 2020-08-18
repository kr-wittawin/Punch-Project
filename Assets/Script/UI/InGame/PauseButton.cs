using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Pauses and unpauses the game
public class PauseButton: MonoBehaviour {

	public Button button;
	public GameObject pauseMenu; //Game object containing the pause menu's buttons

	private bool paused = false;
	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		button.GetComponent<Button>().onClick.AddListener(() => { Pause(); } );
	}

	public void Pause () {
		if (!paused) {
			paused = true;
			Time.timeScale = 0;
			pauseMenu.SetActive(true); //Set pause menu visible and active
			transform.FindChild("Text").GetComponent<Text>().text = "Continue";
		} else {
			paused = false;
			Time.timeScale = 1;
			pauseMenu.SetActive(false); //Set pause menu invisible and inactive
			transform.FindChild("Text").GetComponent<Text>().text = "Pause";
		}
	}
}
