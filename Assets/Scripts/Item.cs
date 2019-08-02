using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

	//nombre del objeto
	public string name;
	//descripcion interna para el desarrollador
	public string description;
	//nombre de la imagen que se recuperará de resources
	public string imageName;
	//indica si el onjeto ya ha sido recogido, esto será utilizado en el conjunto de allItems,
	//no en el propio inventario.
	public bool picked = false;



}
