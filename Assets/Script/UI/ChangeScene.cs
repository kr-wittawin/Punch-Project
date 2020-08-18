using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

// ChangeScene class, generic class for all scene transitions
// Will be attached to a button
public class ChangeScene : MonoBehaviour
{
	// variable, depends on what is the destination scene
	public string sceneName;

	// Network manager for multiplayer mode
	GameObject netmanager;

	// normal move scene
	public void moveScene() {
		Application.LoadLevel(sceneName);
	}

	// move scene for multiplayer
	// delete the network manager as it quits to other scenes
	public void moveSceneMult() {
		netmanager = GameObject.FindGameObjectWithTag ("NetManager");
		if (netmanager != null) {
			netmanager.GetComponent<NetworkManager> ().StopHost ();
		}

		Application.LoadLevel(sceneName);
		if (Application.loadedLevelName != sceneName) {
			IntegrationTest.Fail (gameObject);
		}
		IntegrationTest.Pass (gameObject);
	}
}

