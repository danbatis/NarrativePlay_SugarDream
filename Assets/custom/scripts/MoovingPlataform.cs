using UnityEngine;
using System.Collections;

public class MoovingPlataform : MonoBehaviour {
	public bool spectrum;
	public int spectrumIndex;
	public float specGain = 100f;
	AudioSpectrum mySpectrum;
	Vector3 initialPos;

	public float ampx=2f;
	public float freqx=1f;
	public float ampy=3f;
	public float freqy=2f;
	public float absorbAmp = 0.1f;
	public float dampRate = 1f;

	Vector3 newpos;
	Transform myTransform;
	float freqOffset = 0f;
	float oldAmpy;
	bool absorbing=false;
	bool damped=false;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		mySpectrum = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioSpectrum> ();
		initialPos = transform.position;
		newpos = new Vector3 ();
		newpos = initialPos;
		oldAmpy = ampy;
	}
	
	// Update is called once per frame
	void Update () {
		if (spectrum && mySpectrum != null) {
			newpos.y += mySpectrum.spectrum [spectrumIndex] * specGain*Time.deltaTime;	
		} 
		else{
			if (ampy != 0) {
				newpos.y += ampy * Mathf.Sin (freqy * Time.time + freqOffset) * Time.deltaTime;
				if (absorbing) {
					DecreaseOscillation (Mathf.Sin (freqy * Time.time + freqOffset) );					
				}
			}
		}
		if (ampx != 0f) {
			newpos.x += ampx * Mathf.Sin (freqx * Time.time)*Time.deltaTime;
		}

		myTransform.position = newpos;
	}
	public void absorbNewWeight(){
		Debug.Log (gameObject.name+" absorbing weight...");
		StartCoroutine (absorbNewLoad ());
		/*
		freqOffset = -90*Mathf.Deg2Rad - freqy * Time.time;		 
		ampy = absorbAmp;
		absorbing = true;
		*/
	}
	IEnumerator absorbNewLoad() {
		freqOffset = -90*Mathf.Deg2Rad - freqy * Time.time;		 
		ampy = absorbAmp;
		yield return new WaitForSeconds(0.2f);
		absorbing = true;
		//ResetOscillation ();
	}
	void DecreaseOscillation(float osciPhase){
		if (osciPhase >= 0) {
			if (!damped) {
				damped = true;
				ampy -= dampRate;
				if (ampy <= oldAmpy)
					ResetOscillation ();				
			}
		} 
		else {			
			damped = false;
		}
	}
	void ResetOscillation(){
		ampy = oldAmpy;
		freqOffset = 0f;
		newpos.y = initialPos.y;
		absorbing = false;
	}
}
