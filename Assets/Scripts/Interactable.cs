using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	//posicion en la que debera estar el presonaje al interactuar con el objeto
	public Transform interactableLocation;


	[Header("Condiciones")]
	public string[] conditions;


	/// <summary>
	/// Gestiona las condiciones y las reacciones
	/// </summary>
	public void Interact(){
		
		//variable para controlar si se cumplen las variables
		bool success = true;



		//recorremos la lista de condiciones
		foreach (string cond in conditions) {
			//verificamos si la condicion no se cumple
			if (!DataManager.DM.CheckCondition(cond)) {
				//indicamos que no se ha cumplido la lista de condiciones
				success = false;
				//rompemos el bucle para ahorrar ciclos
				break;
			}

		}

		//si se cumplen las condiciones
		if (success && conditions.Length >0) {
			//recupero el objeto vacio de positive reactions
			Transform positiveReactions = transform.Find ("Positive Reactions");
			//recorremos todas las reacciones que esten dentro del objeto vacio Positive Reactions
			for (int i = 0; i < positiveReactions.childCount; i++) {
				//voy ejecutando las reacciones
				positiveReactions.GetChild (i).gameObject.GetComponent<Reaction> ().ExecuteReaction ();
			}
		
		} else {
			//recupero el objeto vacio de default reactions
			Transform defaultReactions = transform.Find ("Default Reactions");
			//recorremos todas las reacciones que esten dentro del objeto vacio Default Reactions
			for (int i = 0; i < defaultReactions.childCount; i++) {
				//voy ejecutando las reacciones
				defaultReactions.GetChild (i).gameObject.GetComponent<Reaction> ().ExecuteReaction ();
			}
		}
	}
}
