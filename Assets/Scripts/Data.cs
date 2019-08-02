using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data {

	//nombre de la escena actual
	public string actualScene;

	//nombre del punto de entrada a la escena
	public string startPosition;

	//array con el estado de todas las condiciones
	public Condition[] allCondition;

	//Array con todos los items del juego
	public Item[] allItems;

	//array con el inventario actual del jugador, definimos la dimension inicial en funcion de los slots
	public Item[] inventory = new Item[4];
}
