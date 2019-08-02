using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para poder hacer la lectura del XML
using System.Xml;

public class TranslateManager : MonoBehaviour {

	//idioma por defecto
	public string defaultLanguage = "English";

	//listado de frases usando un Hashtable, es un tipo de array pero que permite indices alfanumericos
	public Hashtable strings;

	public static TranslateManager TM;


	void Awake () {
		if (TM == null) {
			TM = GetComponent<TranslateManager> ();
		}
	}

	void Start(){
		//recuperamos el idioma del sistema operativo
		string language = Application.systemLanguage.ToString ();

		//recuperamos el xml como texto
		TextAsset textAsset = (TextAsset)Resources.Load ("lang", typeof(TextAsset));

		//creamos una variable de tipo xmldocument para gestional el xml
		XmlDocument xml = new XmlDocument();

		//cargamos el xml desde el texto
		xml.LoadXml(textAsset.text);

		//verificamos si existe el idioma 
		if (xml.DocumentElement[language] == null) {
			//si no existe el idioma del sistema en las traducciones, usaremos el idioma por defecto
			language = defaultLanguage;
		}

		//llamamos al metodo que cargara los literales de texto en el hashtable
		SetLanguage (xml, language);
	}
		

	/// <summary>
	/// carga el idioma saleccionado del XML
	/// </summary>
	/// <param name="xml">Xml.</param>
	/// <param name="language">Language.</param>
	public void SetLanguage(XmlDocument xml,string language){
		//inicializamos el Hashtable
		strings = new Hashtable ();

		//recuperamos el bloque con el idioma seleccionado
		XmlElement element = xml.DocumentElement [language];

		//si se ha podido recuperar un bloque
		if (element != null) {
			//mediante este metodo recuperamos un tipo enumerador que nos permite
			//recorrer el xml como si fuese un foreach
			IEnumerator elemEnum = element.GetEnumerator ();

			//mientras queden elementos por recorrer
			while (elemEnum.MoveNext ()) {
				//recuperamos el valor actual
				XmlElement xmlItem = (XmlElement)elemEnum.Current;

				//añadimos el literal actual en el hashtable, utilizando como
				//indice el valor del atributo
				strings.Add (xmlItem.GetAttribute ("name"), xmlItem.InnerText);
			}
		} else {
			Debug.LogWarning ("El lenguaje especificado no existe: " + language);
		}
	
	}
		public string GetString(string name){
			//verificamos si no existe el indice
			if (!strings.ContainsKey (name)) {
				Debug.LogWarning ("La cadena no exixte " + name);
				return "";
			}else{
				//si existe, devolvemos el valor del indice
				return(string)strings [name];
			}
		}
	}

