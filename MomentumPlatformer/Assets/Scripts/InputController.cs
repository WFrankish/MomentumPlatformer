using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public bool Run { get; private set; }

	public bool Jump { get; private set; }

	public Vector2 Movement { get; private set; }

	void Start(){
		Movement = new Vector2 ();
	}

	void Update(){
		Movement = VectorTools.Clamp(new Vector2(Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")));
		Run = Input.GetButton ("Fire1");
		Jump = Input.GetButton ("Jump");
	}

}
