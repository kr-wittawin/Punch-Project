using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//Script for skill testing
//Activate button automatically
public class skillTestManager : MonoBehaviour {

	GameObject button;
	// Use this for initialization
	void Start () {
		button = GameObject.FindGameObjectWithTag ("Skill1");
		button.GetComponent<Button> ().onClick.Invoke ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
