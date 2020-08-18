using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

//This scripts is our custom network manager script
public class MyNetworkManager : NetworkManager {

	//Buttons gameobject
	GameObject myHostButton;
	GameObject myClientButton;

	// Use this for initialization
	void Start () {

		//Assign the myHost and myClient function to the buttons
		myHostButton = GameObject.FindGameObjectWithTag ("MyHostButton");
		myClientButton = GameObject.FindGameObjectWithTag("MyClientButton");
		myHostButton.GetComponent<Button> ().onClick.AddListener (() => {
			MyHost (); });
		myClientButton.GetComponent<Button> ().onClick.AddListener (() => {
			MyClient (); });
	}

	//When this function is activated, that device will be the host of the game
	public void MyHost()
	{
		Debug.Log ("Starting host");
		StartHost();
		disableGUI();
	}

	//When this function is activated, that device will be a client 
	public void MyClient()
	{
		Debug.Log ("Starting client");
		networkAddress = GameObject.FindGameObjectWithTag("IpInputField").GetComponent<InputField>().text;
		StartClient();
	}

	//Disable the host and client button as game will start
	public void disableGUI()
	{
		GameObject.FindGameObjectWithTag("InitialMultGUI").SetActive(false);
	}

	//When a client get disconnected, makes it goes to the gameover scene
	public override void OnStopClient ()
	{
		Debug.Log ("I GO TO GAMEOVER SCENE");
		base.OnStopClient ();
		Application.LoadLevel ("GameOver");
	}

}
