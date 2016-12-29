using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject Target;

	// Use this for initialization
	void Start () {
		_z = -10;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Target.transform.position + new Vector3(0, 0, _z);
	}

	private float _z;

}
