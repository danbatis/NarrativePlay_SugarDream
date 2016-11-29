using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {
	public Transform target;
	Transform myTransform;
	Vector3 camPos;
	public float offsetx=2f;
	public float offsety=2f;
	public float camSpeed=10f;

	// Use this for initialization
	void Start () {
		if (target == null) {
			Time.timeScale = 0;
			Debug.Log("Error! The target of the script cameraFollow was not assigned!");
		}
		camPos = new Vector3 ();
		camPos = transform.position;

		myTransform = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target.position.x >= camPos.x + offsetx) {
			camPos.x += camSpeed * Time.deltaTime;
		} 
		else {
			if(target.position.x <= camPos.x - offsetx)
				camPos.x -= camSpeed * Time.deltaTime;
		}
		//y
		if (target.position.y >= camPos.y + offsety) {
			camPos.y += camSpeed * Time.deltaTime;
		} 
		else {
			if(target.position.y <= camPos.y - offsety)
				camPos.y -= camSpeed * Time.deltaTime;
		}
		myTransform.position = camPos;	
	}
}
