using UnityEngine;
using System.Collections;

public class elevatorRunner : MonoBehaviour {
	Vector3 initialPos;
	Vector3 newPos;
	public float speedX=5f;
	public float speedY=5f;
	public float screenBorderX = 4.17f;
	public float oscillationHeight = 4.17f;
	public float elevatorHeight;
	public float y_oscillationAmp=2f;
	public float osciFreq=5f;
	Transform myTransform;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		initialPos = transform.position;
		newPos = new Vector3 (initialPos.x,initialPos.y,initialPos.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (newPos.x >= screenBorderX || newPos.x <= -screenBorderX)
			speedX *= -1;
		
		newPos.x += speedX * Time.deltaTime;

		if (newPos.y < oscillationHeight) {
			newPos.y += speedY * Time.deltaTime;
		}
		else{
			newPos.y += speedY * Time.deltaTime + y_oscillationAmp*Mathf.Sin(osciFreq*Time.time)*Time.deltaTime;
		}
		myTransform.position = newPos;
	}
}
