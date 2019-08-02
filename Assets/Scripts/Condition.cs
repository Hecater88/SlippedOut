using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition {

	//nombre de la condicion, debe ser unico ya qeu se localizara por este
	public string name;
	//descripcion de apoyo, para usarse solo en el inspector
	public string description;
	//para inicar si ha sido cumplida la condicion
	public bool done;
}
