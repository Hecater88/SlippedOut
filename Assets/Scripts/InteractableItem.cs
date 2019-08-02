using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : Interactable {

	//nombre del item, este nombre tiene que ser el mismo que aparezaca en allItems
	public string itemName;

	// Use this for initialization
	void Start () {
		//verificamos si el objeto ha sido recogido
		if (IsPicked()) {
			//si ya ha sido recogido, lo retiramos de la escena
			gameObject.SetActive (false);
		}
	}

	/// <summary>
	/// verifica si el objeto ha sido recogido
	/// </summary>
	/// <returns><c>true</c> if this instance is picked; otherwise, <c>false</c>.</returns>
	bool IsPicked(){
		//recorremos el listado de objetos de nuestro juego
		foreach (Item item in DataManager.DM.data.allItems) {
			//si el nombre coincide con el del interactable
			if (item.name == itemName) {
				//devolvemos la informacion de si ya ha sido recogido
				return item.picked;
			}
		}

		//si llegamos hasta aqui, significará que el objeto no ha sido encontrado
		//informamos para que se revise que el nombre esté bien escrito
		Debug.LogWarning ("El nombre del item no existe en la lista: " + itemName);
		return false;
	}
}
