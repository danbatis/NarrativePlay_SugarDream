using UnityEngine;
using System.Collections;

public class story2Dmanager : MonoBehaviour {
	private bool paused;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("escape")) {
			if(paused){
				Time.timeScale = 1;
				paused = false;
			}
			else {
				Time.timeScale = 0;
				paused = true;
			}
		}	
	}

	public void playerDie(){
		Time.timeScale = 0;
		Debug.Log ("player died! Restart?");

	}
}
