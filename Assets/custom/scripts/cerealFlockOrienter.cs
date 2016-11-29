using UnityEngine;
using System.Collections;

public class cerealFlockOrienter : MonoBehaviour {
	[SerializeField] private float timespan=2f;
	private Rigidbody2D myBody;
	public float adjustAngle = 195f;

	Vector3 newRight = new Vector3();

	// Use this for initialization
	void Start () {
		myBody = GetComponent<Rigidbody2D> ();
		GameObject.Destroy (gameObject, timespan);	
	}
	
	// Update is called once per frame
	void Update () {		
		newRight = Quaternion.Euler (0f,0f,adjustAngle) * myBody.velocity.normalized;
		transform.right = newRight;
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("Collided with: "+other.name);
	}
}
