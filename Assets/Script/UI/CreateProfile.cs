using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Allows editing of the username field
public class CreateProfile : MonoBehaviour {
	public Text UserName;
	public GameObject Editable;


	public void CreateNewProfile(){
		UserName.enabled = false;
		Editable.SetActive(true);
	}
}
