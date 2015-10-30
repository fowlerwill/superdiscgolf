using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

	public float maxStretch = 3f;

	private SpringJoint spring;
	private Rigidbody rbody;


	private bool clickedOn = false;
	private bool springEnabled = true;

	void Awake () {
		this.spring = this.GetComponent<SpringJoint> ();
		this.rbody = this.GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown () {
		Debug.Log ("Mousedn");
		this.springEnabled = false;
		this.clickedOn = true;
	}

	void OnMouseUp () {
		Debug.Log ("Mouseup");
		this.springEnabled = true;
		this.rbody.isKinematic = false;
		this.clickedOn = false;
	}
}
