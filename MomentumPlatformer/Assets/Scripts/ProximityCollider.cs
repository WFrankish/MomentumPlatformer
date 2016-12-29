using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProximityCollider : MonoBehaviour {

	public int Count{
		get{
			return _normalDict.Count;
		}
	}

	public IList<Vector3> GetContactNormals(){
		return _normalDict.Values.ToList ();
	}

	void Start () {
		_normalDict = new Dictionary<GameObject, Vector3> ();
		_parentObject = GetComponentInParent<PlayerController> ().gameObject;
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject != _parentObject) {
			_normalDict[collision.gameObject] = collision.contacts[0].normal;
		}
	}

	void OnCollisionStay(Collision collision){
		if (collision.gameObject != _parentObject) {
			_normalDict[collision.gameObject] = collision.contacts[0].normal;
		}
	}

	void OnCollisionExit(Collision collision){
		if (collision.gameObject != _parentObject) {
			_normalDict.Remove (collision.gameObject);
		}
	}
	
	private GameObject _parentObject;

	private Dictionary<GameObject, Vector3> _normalDict;
}
