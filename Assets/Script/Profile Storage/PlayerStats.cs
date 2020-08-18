using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Profile storage for integers
public class PlayerStats : MonoBehaviour {


	void Start () {
		//Creates this profile info if it hasn't been already
		if (!PlayerPrefs.HasKey (this.name)) PlayerPrefs.SetInt(this.name,0);

		//Sets the gameobject's text to the field's value
		if (this.GetComponent<InputField> () != null) {
			this.GetComponent<InputField> ().text = PlayerPrefs.GetInt (this.name).ToString();
		} else if (this.GetComponent<Text> () != null) {
			this.GetComponent<Text> ().text = PlayerPrefs.GetInt (this.name).ToString();
		}
	}

	public void Save(){
		//Stores the inputfield to the profile
		PlayerPrefs.SetInt(this.name,int.Parse(this.GetComponent<InputField>().text));
	}

}
