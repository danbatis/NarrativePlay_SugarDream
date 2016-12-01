using UnityEngine;
using System.Collections;

public class creepingFire : MonoBehaviour {
	Vector3 newpos;
	public float creepSpeed = 10f;
	public GameObject deathShort;

	// Use this for initialization
	void Start () {
		newpos = new Vector3 ();
		newpos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		newpos.x += creepSpeed * Time.deltaTime;
		transform.position = newpos;
	}

	void OnCollisionEnter2D(Collision2D other){		
		if (other.transform.tag == "Player") {
			Debug.Log ("Got player!!!!!"); 
			GameObject.Instantiate (deathShort, Vector3.zero, Quaternion.identity);
		}
	}
}
