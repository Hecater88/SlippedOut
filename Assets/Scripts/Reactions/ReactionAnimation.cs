using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionAnimation : Reaction {

	//objeto que sera animado
	public GameObject target;
	//nombre del trigger del animator a disparar
	public string triggerName;

	/// <summary>
	/// Metodo que ejecuta la reaccion, con override para que pise la corrutina heredada y se ejecute esta
	/// </summary>
	protected override IEnumerator React(){
		//tiempo que espera antes de iniciar la animacion
		yield return new WaitForSeconds (delay);

		target.GetComponent<Animator> ().SetTrigger (triggerName);
	}

}
