using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//para serializar el binario
using System.Runtime.Serialization.Formatters.Binary;
//para guardar y leer ficheros (Input-Output)
using System.IO;

public class DataManager : MonoBehaviour {
	//objeto que contendrá toda la información de la partida, incluidas las condiciones
	public Data data;
	//nombre del fichero de guardado
	public string filename = "data.dat";


	public static DataManager DM;

	void Awake(){
		//solo queremos que se cargue una vez
		if (DM == null) {
			DM = GetComponent<DataManager> ();

			//recuperamos toda la inforamcion guardada en el disco al inicio del DataManager
			Load();
		}
	}
	/// <summary>
	/// Guardado de la información
	/// </summary>
	public void Save(){
		//objeto utilizado para serializar - deserealizar
		BinaryFormatter bf = new BinaryFormatter();
		//creamos-sobreescribimos el fichero con los datos
		FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
		//serializamosd el contenido de nuestro objeto de datos
		bf.Serialize(file,data);
		//cerramos el fichero una vez terminamos
		file.Close();
	}

	/// <summary>
	/// Recuperamos la información del disco
	/// </summary>
	public void Load(){
		//ruta de guardado del fichero de datos por consola, para acceder a ella facilmente
		Debug.Log(Application.persistentDataPath + "/" + filename);

		//verificamos si existe el fichero
		if (File.Exists(Application.persistentDataPath + "/" + filename)) {
			BinaryFormatter bf = new BinaryFormatter ();
			//abrimos el fichero para lectura
			FileStream file = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Open);
			//deserializamos el contenido del fichero de datos y lo volcamos a data
			data = (Data)bf.Deserialize (file);
			//cerramos el fichero una vez terminamos
			file.Close();
		}
	}

	/// <summary>
	/// Devuelve el estado en el que se encuentra la condicion recibida como parametro
	/// </summary>
	/// <returns><c>true</c>, if condition was checked, <c>false</c> otherwise.</returns>
	/// <param name="conditionName">Condition name.</param>
	public bool CheckCondition(string conditionName){
		//recorremos todas las condiciones del array buscando la indicada
		foreach (Condition cond in data.allCondition) {
			//si el nombre de la condicion actual coincide
			if (cond.name == conditionName) {
				//devolvemos el estado de dicha condicion
				return cond.done;
			}
		}

		//aviso de que la condicion no existe
		Debug.LogWarning (conditionName + " - no existe");
		return false;
	}

	/// <summary>
	/// Cambia el estado de una condicion
	/// </summary>
	/// <param name="conditionName">Condition name.</param>
	/// <param name="done">If set to <c>true</c> done.</param>
	public void SetCondition(string conditionName, bool done){
		//recorremos el array de condiciones en busca de la condicion
		foreach (Condition cond in data.allCondition) {
			//si encontramos la condicion 
			if (cond.name == conditionName) {
				//cambiamos su estado
				cond.done = done;
				return;
			}
		}
		Debug.LogWarning (conditionName + " - no existe");
	}

}
