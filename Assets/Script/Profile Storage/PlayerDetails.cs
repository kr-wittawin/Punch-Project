using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Profile storage for strings
public class PlayerDetails : MonoBehaviour {
	

	void Start () {
		//Creates this profile info if it hasn't been already
		if (!PlayerPrefs.HasKey (this.name)) PlayerPrefs.SetString(this.name,"");

		//Sets the gameobject's text to the field's value
		if (this.GetComponent<InputField> () != null) {
			this.GetComponent<InputField> ().text = PlayerPrefs.GetString (this.name);
		} else if (this.GetComponent<Text> () != null) {
			this.GetComponent<Text> ().text = PlayerPrefs.GetString (this.name);
		}
	}

	public void Save(){
		//Stores the inputfield to the profile
		PlayerPrefs.SetString(this.name,this.GetComponent<InputField>().text);
	}

}
