using UnityEngine;
using System.Collections;

public class CerealCounter : MonoBehaviour {
	bool hurting;
	public int cerealLife=10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	public void CerealHit(){
		if (!hurting) {
			StartCoroutine (CerealDamage());
		}	
	}

	IEnumerator CerealDamage() {
		hurting = true;
		cerealLife -= 1;
		if (cerealLife <= 0) {
			CerealDeath ();
		}
		yield return new WaitForSeconds (0.5f);
		hurting = false;
	}

	void CerealDeath(){
		GetComponent<MoovingPlataform> ().Death ();	
	}
}
