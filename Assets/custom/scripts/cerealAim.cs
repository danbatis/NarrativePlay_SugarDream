using UnityEngine;
using System.Collections;

public class cerealAim : MonoBehaviour {
	Transform fatherTransform;
	LineRenderer line;
	public float sensitivity = 0.1f;
	public GameObject cerealRingPrefab;
	public float cerealForce = 0.1f;
	public float cerealForceMin = 1f;
	public float cerealForceMax = 5f;
	public float throwDist = 2f;

	// Use this for initialization
	void Start () {
		fatherTransform = transform.parent.transform;
		line = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isMouseinViewPort ()) {
			line.enabled = true;
			Vector3 fatherViewPos = Camera.main.WorldToScreenPoint(fatherTransform.position);
			Vector3 mouseViewPos = Input.mousePosition;
			Vector3 mouseDelta = mouseViewPos - fatherViewPos;
			line.SetPosition (0, fatherTransform.position);
			line.SetPosition (1, fatherTransform.position + sensitivity * mouseDelta);

			if (Input.GetMouseButtonDown (0) && Time.timeScale == 1) {
				Debug.Log ("clicked mouse!");
				GameObject newCerealRing = (GameObject)GameObject.Instantiate(cerealRingPrefab, fatherTransform.position+throwDist*fatherTransform.forward, Quaternion.identity );
				float currentForce = Mathf.Clamp(mouseDelta.magnitude,cerealForceMin,cerealForceMax);

				//Debug.Log ("unclamped force: "+mouseDelta.magnitude.ToString() + " currentForce: "+currentForce.ToString());
				newCerealRing.GetComponent<Rigidbody2D> ().AddForce (cerealForce*currentForce*mouseDelta.normalized);
			}
		} else {
			line.enabled = false;
		}
	}

	bool isMouseinViewPort(){
		if (Input.mousePresent) {
			Vector3 mouseViewPos = Camera.main.ScreenToViewportPoint (Input.mousePosition);
			return Camera.main.rect.Contains (mouseViewPos);
		}
		else{			
			Debug.Log ("Ops, no mouse detected!");
			return false;
		}	
	}
}
