using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResetButton : MonoBehaviour {

	public Button button;
	

	public void Reset(){
		Time.timeScale = 1; //restart time 
		Application.LoadLevel("Single_Player"); //reloads the level
	}
}
