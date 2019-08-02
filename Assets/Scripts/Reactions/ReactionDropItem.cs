using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionDropItem : Reaction {

	//nombre del objrto a eliminar del inventario
	public string itemName;

	protected override IEnumerator React (){
		yield return new WaitForSeconds (delay);

		//aliminamos el objeto del inventario
		InventoryManager.IM.RemoveItemInventory (itemName);

	}
}
