using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fireDancer : MonoBehaviour {
	RawImage fireA;
	RawImage fireB;
	Vector3 fireApos;
	Vector3 fireBpos;

	public float ampxFireA;
	public float freqAx;
	public float ampyFireA;
	public float freqAy;

	public float ampxFireB;
	public float freqBx;
	public float ampyFireB;
	public float freqBy;

	// Use this for initialization
	void Start () {
		fireA = GameObject.Find (gameObject.name + "/fire1").GetComponent<RawImage>();
		fireB = GameObject.Find (gameObject.name + "/fire2").GetComponent<RawImage>();
		fireApos = new Vector3 ();
		fireApos = fireA.rectTransform.position;
		fireBpos = new Vector3 ();
		fireBpos = fireB.rectTransform.position;
	}
	
	// Update is called once per frame
	void Update () {
		fireApos.x += ampxFireA*Mathf.Sin(freqAx*Time.time)*Time.deltaTime;
		fireApos.y += ampyFireA*Mathf.Sin(freqAy*Time.time)*Time.deltaTime;
		fireA.rectTransform.position = fireApos;

		fireBpos.x += ampxFireB*Mathf.Cos(freqBx*Time.time)*Time.deltaTime;
		fireBpos.y += ampyFireB*Mathf.Cos(freqBy*Time.time)*Time.deltaTime;
		fireB.rectTransform.position = fireBpos;
	}
}
