using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class ReactionEventSystemActivation : Reaction {
	//event system del objeto para activarlo desactivarlo
	public EventSystem trigger;
	//booleana para activar o desactivar el eventsystem
	public bool active;

	/// <summary>
	/// Corrutina que sera ejecutada como reaccion
	/// </summary>
	protected override IEnumerator React(){
		yield return new WaitForSeconds (delay);

		//activa o desactiva el gameobject indicado como parametro
		trigger.enabled = active;

	}
}
