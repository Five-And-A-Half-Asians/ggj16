using UnityEngine;
using System.Collections;

public class MonsterScript : MonoBehaviour {

	// Get the player object as a field 
	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// move the monster one position per update towards the player 
		Vector3 playerDistanceVector = player.transform.position - gameObject.transform.position;  
		gameObject.transform.position += playerDistanceVector * (Time.deltaTime * 0.01f);
	}

	// When monster touches the player, both player and character dies 
	void OnCollideEnter(Collision col){
		if(col.gameObject.tag == "Player"){
			Destroy(col.gameObject);
			Destroy(gameObject);
			Time.timeScale = 0;
		}
	}
}
