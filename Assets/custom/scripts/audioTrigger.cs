using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class audioTrigger : MonoBehaviour {
	AudioSource myAudio;
	public string nextSceneName;

	// Use this for initialization
	void Start () {
		myAudio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!myAudio.isPlaying) {
			if(GameObject.FindGameObjectWithTag("Player").GetComponent<physics2DControl>().currGround == "EDdrone")
				PlayerPrefs.SetInt ("mounted",1);
			else
				PlayerPrefs.SetInt ("mounted",0);
			
			SceneManager.LoadScene (nextSceneName);
		}
	}
}
