using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ReactionObjectActivation : Reaction {

	//gameobject objeto de la activacion y desactivacion
	public GameObject tarjetObject;

	//booleana para activar o desactivar gameobject
	public bool active;

	/// <summary>
	/// Corrutina que sera ejecutada como reaccion
	/// </summary>
	protected override IEnumerator React(){
		yield return new WaitForSeconds (delay);

		//activa o desactiva el gameobject indicado como parametro
		tarjetObject.SetActive (active);

	}
}
