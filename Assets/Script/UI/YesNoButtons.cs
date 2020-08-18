using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Confirm or deny deletion of profile
public class YesNoButtons : MonoBehaviour {
	public GameObject MainPanel;
	public GameObject YesNoPanel;


	public void Press () {
		if (this.name == "Yes") {
			//Delete profile, reload scene to refresh page
			PlayerPrefs.DeleteAll();
			Application.LoadLevel("Profile");
		} else if (this.name == "No") {
			//Deactivate yes/no menu, return to main screen
			YesNoPanel.SetActive(false);
			MainPanel.SetActive(true);
		}
	}
}
