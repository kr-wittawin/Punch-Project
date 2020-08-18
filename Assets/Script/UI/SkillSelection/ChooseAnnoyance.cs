using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Choose Annoyance class is assigned to each annoyance button 
// 		in the skill selection scene
// Checks whether the maxSKillchosen number is reached
//		if yes -> does not allow for the skill to be selected
//				  until another skill is unselected
//		if not yet -> assign the skill on the button to the
//					  skill manager multiplayer
public class ChooseAnnoyance : MonoBehaviour {
	
	public Button button; 
	public bool ischosen;
	private int maxSkillChosen;
	private int index;
	public Component[] componentMult; 
	private GameObject skillManagerMult;
	private BaseSkillMult annoyanceMult;
	
	// checks with the skill manager whether is it selected
	// if yes -> display as selected
	void Start () {
		index = 0;
		ischosen = false;
		maxSkillChosen = 2;
		skillManagerMult = GameObject.FindGameObjectWithTag ("SkillManagerMult");
		button.GetComponent<Button>().onClick.AddListener(() => { Choose(); } );
		if (SkillSelectionManager.annoyance [0] == button.name || SkillSelectionManager.annoyance [1] == button.name)  {
			transform.FindChild("Text").GetComponent<Text>().text = "Selected";
			ischosen = true;
		}
	}
	
	// Make sure skill manager is there
	void Update () {
		if (skillManagerMult == null) {
			skillManagerMult = GameObject.FindGameObjectWithTag ("SkillManagerMult");
		}
	}

	// Choose function
	// if it is already chosen
	//		-> deselected from skill manager and display
	// if it isn't chosen
	//		-> select and assign skill to skill manager
	public void Choose () {
		if (!ischosen) {
			if(SkillSelectionManager.currentChosenAnnoyance < maxSkillChosen){
				transform.FindChild("Text").GetComponent<Text>().text = "Selected";
				ischosen = true;
				SkillSelectionManager.currentChosenAnnoyance += 1;
				while(SkillSelectionManager.annoyance[index] != "empty") {
					index++;
				}
				SkillSelectionManager.annoyance[index] = button.name;
				index = 0;
				componentMult = transform.FindChild("SkillMult").GetComponents<Component>();
				annoyanceMult = skillManagerMult.AddComponent(componentMult[componentMult.Length-1].GetType()) as BaseSkillMult;
				annoyanceMult.enabled = !annoyanceMult.enabled;
			}
		} else {
			transform.FindChild("Text").GetComponent<Text>().text = " ";
			ischosen = false;
			SkillSelectionManager.currentChosenAnnoyance -= 1;
			while(SkillSelectionManager.annoyance[index] != button.name) {
				index++;
			}
			SkillSelectionManager.annoyance[index] = "empty";
			index = 0;

			//Destroy the skill
			//Get all the components in our skillmanager mult
			//Then find the component that has the same type as the skill described in the button
			//And destroy it
			componentMult = transform.FindChild("SkillMult").GetComponents<Component>();
			Component[] skillManagerMultComponent = skillManagerMult.GetComponents<Component>();

			for (int i = 0 ; i <skillManagerMultComponent.Length ; i++) {
				if (skillManagerMultComponent[i].GetType () == componentMult[componentMult.Length-1].GetType()) {
					Destroy (skillManagerMultComponent[i]);
				}
			}
		}
	}
}
