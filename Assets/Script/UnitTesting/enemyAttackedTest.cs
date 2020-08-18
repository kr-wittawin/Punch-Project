using UnityEngine;
using System.Collections;

//Script for enemy attack testing
//Make player move automatically
public class enemyAttackedTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		player.GetComponent<PlayerMovement> ().Punch (new Vector2(-1,0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
