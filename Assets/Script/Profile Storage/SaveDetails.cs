using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Saves all the stats and details in the current scene, then moves to next scene
public class SaveDetails : MonoBehaviour {


	public void Save(){
		GameObject [] fields;
		//Stats and details objects have been tagged as details
		fields = GameObject.FindGameObjectsWithTag ("Details");

		foreach (GameObject field in fields) {
			if (field.GetComponent<InputField> () != null) {
				PlayerPrefs.SetString(field.name,field.GetComponent<InputField>().text);
			} else if (field.GetComponent<Text> () != null) {
				PlayerPrefs.SetString(field.name,field.GetComponent<Text>().text);
			}
		}

		//move to next scene
		this.GetComponent<ChangeScene> ().moveScene ();

	}
}
