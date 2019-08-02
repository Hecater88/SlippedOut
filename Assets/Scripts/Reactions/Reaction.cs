using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reaction : MonoBehaviour {

	//descripcion de la reaccion que sera visible desde el inspector
	public string description;
	//tiempo de espera antes de ejecutar la reaccion
	public float delay;

	/// <summary>
	/// Metodo generico que ejecutara la reaccion, sera extendido a todas las clases que lo hereden
	/// </summary>
	public virtual void ExecuteReaction(){
		//iniciamos la corrutina reaccion
		StartCoroutine (React ());
	}

	/// <summary>
	/// corrutina que sera ejecutada como reaccion
	/// </summary>
	protected virtual IEnumerator React(){
		//realizamos la espera por el tiempo indicado
		yield return new WaitForSeconds (delay);
	}


}
