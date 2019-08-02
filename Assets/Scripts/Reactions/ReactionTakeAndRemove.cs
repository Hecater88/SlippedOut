using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionTakeAndRemove : Reaction {

	//el objeto recogido pasa al inventario
	public bool moveToInventory = false;
	//referencia la objeto que será desctivado
	public InteractableItem itemReference;
	//nombre del onjeto a recoger
	private string itemName;





	// Use this for initialization
	void Start () {

		//recuperamos la referencia al componente interactableItem, para obtener el itemName
		itemName = itemReference.itemName;
	}

	/// <summary>
	/// Realiza la recogida del objeto
	/// </summary>
	private void PickUpItem () {
		//recorremos los objetos del juego
		foreach (Item item in DataManager.DM.data.allItems) {
			if (item.name == itemName) {
				//cambia su estado a recogido
				item.picked = true;

				//desactivamos el objeto de la escena, ya que pasará a estar en nuestro inventario
				itemReference.gameObject.SetActive (false);

				if (moveToInventory) {
					//añadimos eñ objeto al inventario
					InventoryManager.IM.AddInventory (itemName);

				}

				return;
			}
		}

		Debug.LogWarning ("El nombre de objeto buscado, no ha sido ebcontrado: " + itemName);
	}

	/// <summary>
	/// corrutina que sera ejecutada como reaccion
	/// </summary>
	protected override IEnumerator React(){

		yield return new WaitForSeconds (delay);

		//metodo que realizará las acciones de recogida
		PickUpItem();
	}
}
