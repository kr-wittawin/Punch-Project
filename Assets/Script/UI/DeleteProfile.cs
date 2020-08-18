using UnityEngine;
using System.Collections;

//Allows the deletion of the current profile, including stats
public class DeleteProfile : MonoBehaviour {

	public GameObject MainPanel;
	public GameObject YesNoPanel;



	public void Delete(){
		//Activates confirmation menu
		YesNoPanel.SetActive(true);
		MainPanel.SetActive(false);
	}
}
