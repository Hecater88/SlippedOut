using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

	//referencia al canvas del inventario
	public GameObject inventoryCanvas;

	public static InventoryManager IM;


	void Awake () {
		if (IM == null) {
			IM = GetComponent<InventoryManager> ();
		}
	}

	void Start(){
		//actualizamos el inventario al iniciar, para mostrar si ya habia algo en el inventario
		UpdateInventory ();
	}
	
	/// <summary>
	/// Actualizará los objetos mostrados en el inventario
	/// </summary>
	public void UpdateInventory () {
		
		SortInventory ();

		//recorremos cada uno de los slots de nuestro inventario
		for (int i = 0; i < DataManager.DM.data.inventory.Length; i++) {
			//recupero la referencia a la imagen del slot actual
			Image tempImage = inventoryCanvas.transform.GetChild (i).Find ("Item").GetComponent<Image> ();

			//si hay un item en el slot actual
			if (DataManager.DM.data.inventory [i].name != "") {
				//cargamos este item dentro de la imagen del slot
				tempImage.sprite = Resources.Load<Sprite> ("Items/" + DataManager.DM.data.inventory [i].imageName);

				//si no hemos podido encontrar la imagen
				if (tempImage.sprite == null) {
					Debug.LogWarning ("La imagen no ha sido encontrada en resources: " + "Items/" +
						DataManager.DM.data.inventory [i].imageName);
				}

				//activamos la imagen para que sea visible
				tempImage.gameObject.SetActive (true);
			} else {
				
				//desactivamos la imagen para que sea visible
				tempImage.gameObject.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Localiza los datos del item y los añade al inventario
	/// Devuelve una bool indicando si ha podido o no recoger el objeto
	/// </summary>
	/// <param name="itemName">Item name.</param>
	public bool AddInventory(string itemName){
		//variable donde almacenaremos el nuevo item a añadir
		Item newItem = new Item ();

		foreach (Item item in DataManager.DM.data.allItems) {
			//si lo encontramos, guardamos la informacion en nuestra variable
			if (item.name == itemName) {
				newItem.name = item.name;
				newItem.description = item.description;
				newItem.imageName = item.imageName;
				newItem.picked = item.picked;
				//salimos del bucle
				break;
			}
		}

		//recorremos los huecos de nuestro inventario
		for (int i = 0; i < DataManager.DM.data.inventory.Length; i++) {
			//presuponemos que si el nombre esta en blanco, el hueco estará vacio
			if (DataManager.DM.data.inventory[i].name == "") {
				//introducimos el nuevo item en el inventario
				DataManager.DM.data.inventory[i] = newItem;
				//tras volcar la info, actualizamos el inventario para que se muestre la imagen
				UpdateInventory();
				//para evitar que nos agrege el nuevo objeto en cada hueco del inventario
				//saldremos del metodo mediante un return
				return true;
			}
		}
		//en caso de no haber hueco en el inventario, devolvemos false
		return false;
	}

	/// <summary>
	/// elimina el item del inventario
	/// </summary>
	/// <param name="itemName">Item name.</param>
	public void RemoveItemInventory(string itemName){
		//recorremos los huecos del inventario
		foreach (Item item in DataManager.DM.data.inventory) {
			if (item.name == itemName) {
				item.name = "";
				item.description = "";
				item.imageName = "";
				item.picked = false;

				//actualizamos el inventario, para que elimine la imagen del objeto borrado
				UpdateInventory();

				return;
			}
		}

		//si llegamos hasta aqui, significara que no hemos encontrado el objeto con ese nombre
		Debug.LogWarning ("No se ha encontrado el objeto en el inventario: " + itemName);
	}

	/// <summary>
	/// Ordena los items del inventario, para asegurar que nunca quede un hueco vacio en medio
	/// </summary>
	public void SortInventory(){

		//indice del primer espacio blanco encontrado
		int firstEmpty;
		//indicador para controlar si se ha encontrado un objeto traas un hueco vacio
		bool foundAfterEmpty = true;

		//mientras se sigan encontrando objetos tras huecos vacios, repetiremos la opcion
		while (foundAfterEmpty) {

			//para empezar ponemos el primer espacio a -1, para indicar que no se han encontrado
			firstEmpty = -1;
			//iniciamos el bool a false, para indicar que por el momentos no se ha encontrado ningun
			//item posterior a un hueco
			foundAfterEmpty = false;

			//recorremos el inventario
			for (int i = 0; i < DataManager.DM.data.inventory.Length; i++) {
				//si encontramos un hueco vacio, guardo el indice
				if (DataManager.DM.data.inventory[i].name == "") {
					firstEmpty = i;
				}else if (DataManager.DM.data.inventory[i].name != "" && firstEmpty >= 0) {
					//si encontramos un hueco con un item tras uno vacio, lo indicamos
					foundAfterEmpty = true;
				}

				//si se ha encontrado un hueco con item tras uno vacio, lo intercambiamos
				if (foundAfterEmpty) {
					CopyItem (firstEmpty, i);
					EmptyItem (i);
					break;
				}
			}
		}
	}

	/// <summary>
	/// Copia objeto de inventario de un hueco a otro
	/// </summary>
	/// <param name="toItem">To item.</param>
	/// <param name="fromItem">From item.</param>
	public void CopyItem(int toItem, int fromItem){
		
		DataManager.DM.data.inventory [toItem].name = DataManager.DM.data.inventory [fromItem].name; 
		DataManager.DM.data.inventory [toItem].description = DataManager.DM.data.inventory [fromItem].description; 
		DataManager.DM.data.inventory [toItem].imageName = DataManager.DM.data.inventory [fromItem].imageName; 
		DataManager.DM.data.inventory [toItem].picked = DataManager.DM.data.inventory [fromItem].picked; 
	}

	/// <summary>
	/// Vacia el hueco del inventario
	/// </summary>
	/// <param name="itemIndex">Item index.</param>
	public void EmptyItem(int itemIndex){

		DataManager.DM.data.inventory [itemIndex].name = "";
		DataManager.DM.data.inventory [itemIndex].description = "";
		DataManager.DM.data.inventory [itemIndex].imageName = "";
		DataManager.DM.data.inventory [itemIndex].picked = false;
	}
}
