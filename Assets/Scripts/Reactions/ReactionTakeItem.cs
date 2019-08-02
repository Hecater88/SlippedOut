using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionTakeItem : Reaction {

	private GameObject itemReference;
	//nombre del objeto a recoger
	private string itemName;





	// Use this for initialization
	void Start () {
		//recuperamos el gameobject interactable principal
		itemReference = transform.parent.transform.parent.gameObject;
		//recuperamos la referencia al componente interactableItem, para obtener el itemName
		itemName = itemReference.GetComponent<InteractableItem> ().itemName;
	}
	
	/// <summary>
	/// Realiza la recogida del objeto
	/// </summary>
	private void PickUpItem () {
		//recorremos los objetos del juego
		foreach (Item item in DataManager.DM.data.allItems) {
			if (item.name == itemName) {

				//intentamos añadir el objeto al inventario, si se puede, hacemos los pasos para su recogida
				if (InventoryManager.IM.AddInventory (itemName)) {
					
					//cambia su estado a recogido
					item.picked = true;

					//desactivamos el objeto de la escena, ya que pasará a estar en nuestro inventario
					itemReference.SetActive (false);
				} else {
					TextManager.TM.Displaymessage (TranslateManager.TM.GetString ("inventory_full"), Color.white);
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
