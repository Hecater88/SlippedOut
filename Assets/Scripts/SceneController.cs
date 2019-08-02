using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	//group con el que haremos el oscurecido de pantalla
	public CanvasGroup faderCanvasGroup;
	//duracion del fade
	public float fadeDuration = 1f;
	//valor por defecto que utilizaremos si no esta definido el datamanager	
	public string startingSceneName = "BlockingScene";
	//valor por defecto que utilizaremos si no esta definido el datamanager
	public string initialPositionName = "ExteriorDoor";

	//para conttrolar si ya se esta haciendo un fundido de escena
	private bool isFading;

	public static SceneController SC;

	void Awake(){
		if (SC == null) {
			SC = GetComponent<SceneController> ();
		}
	}

	private IEnumerator Start(){
		//empezamos con la pantalla en negro
		faderCanvasGroup.alpha = 1f;
		//si hay definido un actualscene en el datamanager, utilizamos ese valor
		//en caso contario, dejamos el hardcodeado
		startingSceneName = DataManager.DM.data.actualScene != "" ? DataManager.DM.data.actualScene
																	:startingSceneName;

		initialPositionName = DataManager.DM.data.startPosition != "" ? DataManager.DM.data.startPosition
																		: initialPositionName;
		//solicitamos la carga de la escena y la ponemos activa
		yield return StartCoroutine (LoadSceneAndSetActive (startingSceneName));

		//fade in
		StartCoroutine (Fade (0f));
	}

	/// <summary>
	/// llamada publica para el cambio de escenas
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	/// <param name="startPosition">Start position.</param>
	public void FadeAndLoadScene(string sceneName, string startPosition){

		//actualizamos el valor de la escena actual
		DataManager.DM.data.actualScene = sceneName;
		//actualizamos el valor de la posicion inicial
		DataManager.DM.data.startPosition = startPosition;

		//guardamos el realizar un cambio de escena
		DataManager.DM.Save();

		if (!isFading) {
			StartCoroutine (FadeAndSwitchScenes (sceneName));
		}
	}
	/*
	/// <summary>
	/// LLamada publica para el guardado en las interacciones
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	/// <param name="startPosition">Start position.</param>
	public void SaveOnReaction(string sceneName, string startPosition){
		//actualizamos el valor de la escena actual
		DataManager.DM.data.actualScene = sceneName;
		//actualizamos el valor de la posicion inicial
		DataManager.DM.data.startPosition = startPosition;

		//guardamos el realizar un cambio de escena
		DataManager.DM.Save();
	}*/

	/// <summary>
	/// Corrutina que cambia de escena con un fade previo
	/// </summary>
	/// <returns>The and switch scenes.</returns>
	/// <param name="sceneName">Scene name.</param>
	private IEnumerator FadeAndSwitchScenes(string sceneName){
		//realizamos un fade out y esperamos que termine de ejecutarse
		yield return StartCoroutine (Fade (1f));

		//descargamos la escena actual
		yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene().buildIndex);

		//cargamos la escena seleccionada, esperando a que temine la carga
		yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

		//hacemos un fade in
		yield return StartCoroutine (Fade (0f));
	}

	/// <summary>
	/// Carga de una nueva escena
	/// </summary>
	/// <returns>The scene and set active.</returns>
	/// <param name="sceneName">Scene name.</param>
	private IEnumerator LoadSceneAndSetActive(string sceneName){
		//cargamos la escena de forma aditiva, sin destruir la escena principal
		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);

		//para recuperar la ultima escena añadidad, tomamos el numero actual de escenas menos 1,
		//esto nos dara eñ indice de la ultima escena añadida
		Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

		//ponemos esta ultima escena como la escena activa
		SceneManager.SetActiveScene (newlyLoadedScene);
	}

	/// <summary>
	/// raliza el fundido a negro y a visible
	/// </summary>
	/// <param name="finalAlpha">Final alpha.</param>
	private IEnumerator Fade(float finalAlpha){
		//indicamos que estamos relaizando ya un fundido
		isFading = true;

		//hacemos que el fader bloquee los raycast, para evita que se realicen pulsaciones durante el cambio de escena
		faderCanvasGroup.blocksRaycasts = true;

		//espacio / tiempo = velocidad
		float fadeSpeed = 1f / fadeDuration;

		//mientras el valor actual del alpha no sea un aproximado al final, realizaremos el siguiente bucle
		while (!Mathf.Approximately(faderCanvasGroup.alpha,finalAlpha)) {
			faderCanvasGroup.alpha = Mathf.MoveTowards (faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

			//para hacer qeu la corrutina se ejecute en el siguiente frame
			yield return null;
		}
		//indicamos que hemos terminado el fundidido
		isFading = false;

		//hacemos qye le fader deje de bloquear los raycast, para volver a detectar las pulsaciones
		faderCanvasGroup.blocksRaycasts = false;
	}
}
