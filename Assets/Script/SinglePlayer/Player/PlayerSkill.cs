using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This script handles the player mana/skill bar
public class PlayerSkill : MonoBehaviour {
	
	public int startingSkillBar; 
	public int currentSkillBar;
	public Slider SkillSlider; //reference sliderUI game object - we must include UnityEngine.UI
	
	void Awake () {
		currentSkillBar = startingSkillBar;
		SkillSlider.value = currentSkillBar;
	}
		
	public void increaseBar (int amount) {
		currentSkillBar += amount;
		if (currentSkillBar > 100) {
			currentSkillBar = 100;
		}
		SkillSlider.value = currentSkillBar;
	}
	
	public void decreaseBar (int amount) {
		currentSkillBar -= amount;
		if (currentSkillBar < 0) {
			currentSkillBar = 0;
		}
		SkillSlider.value = currentSkillBar;
	}
}