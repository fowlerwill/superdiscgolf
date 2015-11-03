using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

	public float maxStretch = 3f;
	public GameObject player;

	private SpringJoint spring;
	private Rigidbody rbody;


	private bool clickedOn = false;
	private bool springEnabled = true;
	private Ray playerToMouseRay;
	private Vector3 prevVelocity;

	public float MoveSpeed = 5.0f;
	public float frequency = 20.0f;  // Speed of sine movement
	public float magnitude = 0.5f;   // Size of sine movement
	private Vector3 axis;
	private Vector3 pos;


	//raycast to this plane to find where the mouse is pointing 
	private Plane targetingPlane;        

	//Raycasts from current mouse position to the targetting plane, returns the resulting Vector3
	Vector3 getCursor() { 
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition ); // Construct a ray from the current mouse coordinates
		
		float hitdistance; 
		
		if (targetingPlane.Raycast (ray, out hitdistance )) return ray.GetPoint(hitdistance);
		
		return transform.position + transform.forward;  //just in case ;)
	}

	void Awake () {
		this.spring = this.GetComponent<SpringJoint> ();
		this.rbody = this.GetComponent<Rigidbody> ();
		this.prevVelocity = this.rbody.velocity;
	}

	// Use this for initialization
	void Start () {

		// Set the mouse position getter
		targetingPlane.SetNormalAndPosition(transform.up, transform.position);

		this.playerToMouseRay = new Ray (player.transform.position, Vector3.zero);

		//testing sine movement
		pos = transform.position;
		axis = transform.right;  // May or may not be the axis you want
	}
	
	// Update is called once per frame
	void Update () {
		if (this.clickedOn) {
			Dragging ();
		}

		if (this.spring != null) {
			//If we're past the sping origin
			if ( this.prevVelocity.sqrMagnitude > this.rbody.velocity.sqrMagnitude ) {
				Destroy( this.spring );
				this.rbody.velocity = this.prevVelocity;
			}

			if ( !this.clickedOn ) {
				this.prevVelocity = this.rbody.velocity;
			}
		} else {
			//Here we calculate the disc arc.
			this.DiscArc();
		}
	}

	void DiscArc() {

//		pos += transform.up * Time.deltaTime * MoveSpeed; // moves in a forward motion
//
//		// here, we want to set the 
//
//		transform.position = pos + axis * Mathf.Sin (Time.time * frequency) * magnitude;
		float magnitudeOfWave = transform.position.x / 2;

		float distanceOfThrow = 8f;
		float newz = magnitudeOfWave * Mathf.Sin( (1 / distanceOfThrow) * transform.position.x );
		Vector3 discPath = new Vector3 (transform.position.x, transform.position.y, newz);
		transform.position = discPath;
	}

	void Dragging() {
		// Set the location of the disc
		Vector3 mouseWorldPoint = this.getCursor ();
		mouseWorldPoint.y = 1f;
		transform.position = mouseWorldPoint;

		// if they're dragging too far
		Vector3 playerToMouse = mouseWorldPoint - player.transform.position;
		if ( Mathf.Abs( playerToMouse.x ) > this.maxStretch || Mathf.Abs(playerToMouse.z) > this.maxStretch ) {
			playerToMouseRay.direction = playerToMouse;
			transform.position = playerToMouseRay.GetPoint(this.maxStretch);
		}

	}

	void OnMouseDown () {
		Debug.Log ("Mousedn");
		this.springEnabled = false;
		this.rbody.isKinematic = true;
		this.clickedOn = true;
	}

	void OnMouseUp () {
		Debug.Log ("Mouseup");
		this.springEnabled = true;
		this.rbody.isKinematic = false;
		this.clickedOn = false;
	}
}
