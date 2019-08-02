using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDestruct : MonoBehaviour {

	public GameObject target;

	/// <summary>
	/// Destruye target
	/// </summary>
	void Destruct(){
		Destroy (target);
	}
}
