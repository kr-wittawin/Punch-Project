using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Choose skill class is assigned to each skill button 
// 		in the skill selection scene
// Checks whether the maxSKillchosen number is reached
//		if yes -> does not allow for the skill to be selected
//				  until another skill is unselected
//		if not yet -> assign the skill on the button to the
//					  skill manager single and multiplayer
public class ChooseSkill : MonoBehaviour {
	
	public Button button; 
	public bool ischosen;
	private int maxSkillChosen;
	private int index;
	public Component[] component; 
	public Component[] componentMult; 
	private GameObject skillManager;
	private GameObject skillManagerMult;
	private BaseSkill skill;
	private BaseSkillMult skillMult;


	// checks with the skill manager whether is it selected
	// if yes -> display as selected
	void Start () {
		index = 0;
		ischosen = false;
		maxSkillChosen = 2;
		skillManager = GameObject.FindGameObjectWithTag ("SkillManagerSingle");
		skillManagerMult = GameObject.FindGameObjectWithTag ("SkillManagerMult");
		button.GetComponent<Button>().onClick.AddListener(() => { Choose(); } );
		if (SkillSelectionManager.skill [0] == button.name || SkillSelectionManager.skill [1] == button.name)  {
			transform.FindChild("Text").GetComponent<Text>().text = "Selected";
			ischosen = true;
		}
	}
	
	// Make sure both skill managers are there
	void Update () {
		if (skillManager == null) {
			skillManager = GameObject.FindGameObjectWithTag ("SkillManagerSingle");
		}
		if (skillManagerMult == null) {
			skillManagerMult = GameObject.FindGameObjectWithTag ("SkillManagerMult");
		}
	}

	// Choose function
	// if it is already chosen
	//		-> deselected from skill managers and display
	// if it isn't chosen
	//		-> select and assign skill to skill managers
	public void Choose () {
		if (!ischosen) {
			if(SkillSelectionManager.currentChosenSkill < maxSkillChosen){
				transform.FindChild("Text").GetComponent<Text>().text = "Selected";
				ischosen = true;
				SkillSelectionManager.currentChosenSkill += 1;
				index = 0;
				while(SkillSelectionManager.skill[index] != "empty") {
					index++;
				}
				SkillSelectionManager.skill[index] = button.name;
				component = transform.FindChild("Skill").GetComponents<Component>();
				componentMult = transform.FindChild("SkillMult").GetComponents<Component>();
				skill = skillManager.AddComponent(component[component.Length-1].GetType ()) as BaseSkill;
				skillMult = skillManagerMult.AddComponent(componentMult[componentMult.Length-1].GetType()) as BaseSkillMult;
				skill.enabled = !skill.enabled;
				skillMult.enabled = !skillMult.enabled;
			}
		} else {
			transform.FindChild("Text").GetComponent<Text>().text = " ";
			ischosen = false;
			SkillSelectionManager.currentChosenSkill -= 1;
			index = 0;
			while(SkillSelectionManager.skill[index] != button.name) {
				index++;
			}
			SkillSelectionManager.skill[index] = "empty";
			index = 0;

			//Destroy the skill
			//Get all the components in our skill manager and skillmanager mult
			//Then find the component that has the same type as the skill described in the button
			//And destroy it
			component = transform.FindChild("Skill").GetComponents<Component>();
			componentMult = transform.FindChild("SkillMult").GetComponents<Component>();
			
			Component[] skillManagerComponent = skillManager.GetComponents<Component>();
			Component[] skillManagerMultComponent = skillManagerMult.GetComponents<Component>();

			for(int i = 0 ; i < skillManagerComponent.Length ; i++) {
				if (skillManagerComponent[i].GetType () == component[component.Length-1].GetType ()) {
					Destroy(skillManagerComponent[i]);
				}
			}

			for (int i = 0 ; i <skillManagerMultComponent.Length ; i++) {
				if (skillManagerMultComponent[i].GetType () == componentMult[componentMult.Length-1].GetType()) {
					Destroy (skillManagerMultComponent[i]);
				}
			}
		}
	}
}
