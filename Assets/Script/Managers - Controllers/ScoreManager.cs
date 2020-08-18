using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public static int score;
	
	
	Text text;
	
	
	void Awake ()
	{
		text = GetComponent <Text> ();
		score = 0;
	}
	
	
	void Update ()
	{
		if (score > 0) {
			text.text = "Score: " + score;
		}
	}
}
