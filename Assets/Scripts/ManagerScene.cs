using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ManagerScene : MonoBehaviour {

	/// <summary>
	/// Cambia a la escena solicitada
	/// </summary>
	/// <param name="name">Name.</param>
	public void SceneLoad (string name){
		SceneManager.LoadScene (name);
	}

	public void Retry(){
		SceneLoad ("Level1");
	}

	public void QuitGame(){
		Application.Quit ();
	}
	
	/// <summary>
	/// Metodo que borra el archivo de guardado e inicia una nueva partida.
	/// </summary>
	public void RetryGame(){
		//borramos el archivo data
		File.Delete (Application.persistentDataPath + "/" + "data.dat");
		////hacemos que el tiempo sea normal
		Time.timeScale = 1f;
		//y cargamos la escena MainMenu
		SceneManager.LoadScene ("MainMenu");
	}
}
