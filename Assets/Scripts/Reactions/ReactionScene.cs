using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionScene : Reaction {

	//nimbre de la escen a cambiar
	public string nextScene;
	//nombre del punto de entrada a la escena, donde posicionaremos al personaje
	public string nextStartPosition;

	protected override IEnumerator React(){
		
		yield return new WaitForSeconds (delay);

		//llamamos al metodo de scenecontroller que llevara a cabo la tarea del cambio de escena
		SceneController.SC.FadeAndLoadScene (nextScene, nextStartPosition);

	}
}
