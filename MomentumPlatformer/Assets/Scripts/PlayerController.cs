Pusing UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour {

	public float Epsilon = 0.01f;

	public Vector2 Gravity;

	public float AccelStrength = 8f;

	public float DeccelStrength = 5f;

	// accessible
	public Vector2 Acceleration;
	public Vector2 Velocity;


	void Start () {
		_collider = GetComponent<CapsuleCollider> ();
		_body = GetComponent<Rigidbody> ();
		_proximity = GetComponentInChildren<ProximityCollider> ();
	}

	void Update () {
		var dt = Time.deltaTime;
		var input = VectorTools.Clamp (new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"))); 
		Vector2 gravity = Gravity;
		Vector2 drag = new Vector2();
		Acceleration *= 0;

		bool ducking = false;

		if (_proximity.Count > 0) {
			// touching at least one surface
			var contacts = _proximity.GetContactNormals();

			// check each contact point
			foreach(var contact in contacts){
				//gravity
				var paraGrav = ComponentOfInDirectionOf(gravity, contact);
				// if in opposite directions
				if(Vector2.Dot (paraGrav, contact) <= 0){
					gravity -= paraGrav;
				}

				// movement
				var paraMove = ComponentOfInDirectionOf(input, contact);
				input -= paraMove;

				if(Vector2.Dot (paraMove, contact) <= 0 && paraMove.magnitude > 0.5){
					ducking = true;
					transform.rotation = Quaternion.LookRotation(contact, Vector3.forward);
				}
			}

			// accelerate or deccelerate
			if(Vector2.Dot(Velocity, input) > 0){
				Acceleration += input * AccelStrength;
			} else {
				Acceleration += input * DeccelStrength;
			}

		}

		if(ducking){
			transform.localScale = new Vector3(1f, 1f, 0.5f);
			_collider.radius = 0.25f;
			_collider.center = new Vector3(0f, 0f, -0.5f);
		} else {
			transform.localScale = new Vector3(1f, 1f, 1f);
			_collider.center = new Vector3(0f, 0f, 0f);
			_collider.radius = 0.5f;
		}

		// accelerate
		Velocity += (Acceleration + gravity + drag) * dt;

		// move
		transform.localPosition += (Vector3) Velocity * dt;
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject != _proximity.gameObject) {
			//Debug.Log ("Enter: " + Time.realtimeSinceStartup + ", " + collision.gameObject + ", " + collision.contacts [0].normal);
			HandleCollision(collision);
		}
	}

	void OnCollisionStay(Collision collision){
		if (collision.gameObject != _proximity.gameObject) {
			//Debug.Log ("Stay: " + Time.realtimeSinceStartup + ", " + collision.gameObject + ", " + collision.contacts [0].normal);
			HandleCollision(collision);
		}
	}

	private void HandleCollision(Collision collision){
		Vector3 contactNormal = collision.contacts [0].normal;
		Vector3 contactPoint = collision.contacts [0].point;
		contactPoint.z *= 0;
		
		// resolve collision;
		var dif = contactPoint - transform.position;
		var paraDif = ComponentOfInDirectionOf (dif, contactNormal);
		transform.position += (Vector3) paraDif + 0.5f * contactNormal;

		// resolve velocity
		var paraV = ComponentOfInDirectionOf (Velocity, contactNormal);
		if (Vector2.Dot (paraV, contactNormal) <= 0) {
			Velocity -= paraV;
		}
		
		// resolve acceleration;
		var paraA = ComponentOfInDirectionOf (Acceleration, contactNormal);
		if (Vector2.Dot (paraA, contactNormal) <= 0) {
			Acceleration -= paraA;
		}
	}

	private Vector2 ComponentOfInDirectionOf(Vector2 one, Vector2 two){
		return VectorTools.ComponentOfInDirectionOf (one, two, Epsilon);
	}

	// self
	CapsuleCollider _collider;
	Rigidbody _body;

	// children
	ProximityCollider _proximity;

}
