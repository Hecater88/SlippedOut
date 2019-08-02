using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {
	//variable de control booleana para saber en que estado está el juego
	public static bool GameIsPaused = false;
	//referencia publica al panel del pauseMenu
	public GameObject pauseMenuUI;

	void Update () {
		//Si pulsamos escape
		if (Input.GetKeyDown (KeyCode.Escape)) {
			//si pulsamos escape y estamos en la pantalla de pausa
			if (GameIsPaused) {
				//reanudamos el juego
				Resume ();

			} else {
				//si pulsamos escape y estamos en game, activamos la pantalla de pausa
				PauseGame();
			}
		}
	}

	public void Resume(){
		//desactivamos el panel
		pauseMenuUI.SetActive (false);
		//reanudamos el juego
		Time.timeScale = 1f;
		//y ponemos la booleana false, para indicar que no estamos en pausa
		GameIsPaused = false;
	}
	/// <summary>
	/// Pausa el juego
	/// </summary>
	void PauseGame(){
		//activamos el panel
		pauseMenuUI.SetActive (true);
		//pausamos el juego
		Time.timeScale = 0f;
		//y ponemos la booleana true, para indicar que estamos en pausa
		GameIsPaused = true;
	}

	/// <summary>
	/// Carga la escena MainMenu
	/// </summary>
	public  void LoadMenu(){
		Time.timeScale = 1f;
		SceneManager.LoadScene ("MainMenu");
	}
	/// <summary>
	/// Sale del juego
	/// </summary>
	public void QuitGame(){
		Application.Quit ();
	}
	public void SceneLoad(string name){
		SceneManager.LoadScene (name);
	}
}
