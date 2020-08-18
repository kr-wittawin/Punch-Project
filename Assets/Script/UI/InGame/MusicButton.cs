using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Turns the game music off and on again
public class MusicButton : MonoBehaviour {
	public Button button;
	public GameObject backgroundMusic;

	private bool musicPlaying = true;
	
	// Use this for initialization
	void Start () {
		button.GetComponent<Button>().onClick.AddListener(() => { ToggleMusic(); } );
	}
	
	void Update () {
		
	}
	
	public void ToggleMusic(){
		if (musicPlaying) {
			musicPlaying = false;
			backgroundMusic.GetComponent<AudioSource>().volume = 0;
			transform.FindChild("Text").GetComponent<Text>().text = "Turn Music On";
		} else {
			musicPlaying = true;
			backgroundMusic.GetComponent<AudioSource>().volume = 1;
			transform.FindChild("Text").GetComponent<Text>().text = "Turn Music Off";
		}
	}
}
