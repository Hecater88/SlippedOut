using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDesactive : MonoBehaviour {

	public static GameObjectDesactive GOD;


	void Awake(){
		if (GOD == null) {
			GOD = GetComponent<GameObjectDesactive> ();
		}
	}


	/// <summary>
	/// Deshabilita el script ctInteractbleItem del target.
	/// </summary>
	public void DisabledGameObject (GameObject target){
		target.GetComponent<InteractableItem> ().enabled = false;
		target.GetComponent<Collider> ().enabled = false;
	}
	/// <summary>
	/// Enableds the game object.
	/// </summary>
	/// <param name="target">Target.</param>
	public void EnabledGameObject (GameObject target){
		target.GetComponent<InteractableItem> ().enabled = true;
		target.GetComponent<Collider> ().enabled = true;
	}

}
