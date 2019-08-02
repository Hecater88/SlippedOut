using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionText : Reaction {

	//mendaje a mostrar
	public string text;
	//color del mensaje
	public Color textColor = Color.black;

	protected override IEnumerator React(){
		yield return new WaitForSeconds (delay);

		//llamamos el textmanager para que se haga cargo del dibujadodel texto con el color indicado
		TextManager.TM.Displaymessage (TranslateManager.TM.GetString(text), textColor);
	}

}
