using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Skill Selection Manager class does the house-keeping stuff 
//		for the skill selection system
// skill array stores what skill is chosen, same goes for the annoyance array
// currentChosenSkill/Annoyance keeps count of the skills chosen
// 2 game objects, container for chosen skills to the sc
public class SkillSelectionManager : MonoBehaviour {
	
	public static int currentChosenSkill;
	public static int currentChosenAnnoyance;
	public static string[] skill = new string[] {"empty","empty"};  //2 empty strings for later changes
	public static string[] annoyance = new string[] {"empty","empty"};
    public GameObject skillManagerSingle;
	public GameObject skillManagerMult;


	// Use this for initialization
	// Protect the skill managers from being destroy on scene change
	void Start () {
		currentChosenSkill = 0;
		currentChosenAnnoyance = 0;
		if (GameObject.FindGameObjectWithTag ("SkillManagerSingle") == null) {
			var singleMgr = Instantiate(skillManagerSingle);
			DontDestroyOnLoad (singleMgr.transform.gameObject);
		}
		if (GameObject.FindGameObjectWithTag ("SkillManagerMult") == null) {
			var multMgr = Instantiate(skillManagerMult);
			DontDestroyOnLoad (multMgr.transform.gameObject);
		}
	}
	
	// Check what skill is chosen by right click
	void Update () {
		if (Input.GetButtonDown ("Fire2")) {
			Debug.Log (skill [0] + " , " + skill [1]);
			Debug.Log (annoyance [0] + " , " + annoyance [1]);
		}
	}

	
}
