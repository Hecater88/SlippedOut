using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class GameOver : MonoBehaviour {
	public static GameOver GO;
	public GameObject gameOverMenu;

	public bool isDead = false;
	void Awake(){
		if (GO == null) {
			GO = GetComponent<GameOver> ();
		}
	}
	
	
	// Update is called once per frame
	void Update () {
		if(isDead){
			gameOverMenu.SetActive(true);
			//pausamos el juego
			Time.timeScale = 0f;
		}
	}

	/// <summary>
	/// Carga la escena MainMenu
	/// </summary>
	public  void LoadMenu(){
		Time.timeScale = 1f;
		isDead = false;
		SceneManager.LoadScene ("MainMenu");
	}
		/// <summary>
	/// Metodo que borra el archivo de guardado e inicia una nueva partida.
	/// </summary>
	public  void RetryGame(){
		//borramos el archivo data
		File.Delete (Application.persistentDataPath + "/" + "data.dat");
		isDead = false;
		////hacemos que el tiempo sea normal
		Time.timeScale = 1f;
		//y cargamos la escena MainMenu
		SceneManager.LoadScene ("Persistent");
	}
}
