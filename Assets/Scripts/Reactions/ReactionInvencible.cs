using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionInvencible : Reaction  {

	public bool active;
	/// <summary>
	/// Metodo que ejecuta la reaccion, con override para que pise la corrutina heredada y se ejecute esta
	/// </summary>
	protected override IEnumerator React(){
		//tiempo que espera antes de iniciar la animacion
		yield return new WaitForSeconds (delay);

		PlayerMovement.invencible = true;
	}
}
