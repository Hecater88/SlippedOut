using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
	public static PlayerMovement PM;
	//tiempo de espera entre pulsaciones
	public float inputHoldDelay = 0.5f;
	//umbral minimo de velocidad de giro del jugador
	public float turnSpeedthresHold = 0.5f;
	//suavizado del movimiento
	public float speedDampTime = 0.1f;
	//velocidad a la que ira frenando
	public float slowingSpeed = 0.17f;
	//suavizado del giro
	public float turnSmoothing = 15f;


	//clase que permite realizar una espera de un tiempo determinado
	private WaitForSeconds inputHoldWait;

	//posicion de destino del desplazamiento
	public Vector3 destinationPosition;

	//ultimo objeto interactivo pulsado
	private Interactable currentInteractable;
	//para controlar cuando se gestionará el imput
	public bool handleInput = true;

	//para calcular la distancia de parada
	private const float stopDistanceProportion = 0.1f;

	//la distancia a la que el navmesh puede estar alejada del click
	private const float navMeshSampleDistance = 4f;

	//referencias a componentes
	public Animator animator;
	public NavMeshAgent agent;

	//Vida máxima del jugador
	public int maxHealth = 100;
	//vida actual del jugador
	public int currentHealth;
	//referencia al slider
	public Slider healthSlider;
	//referencia a la imagen con la que flashearemos la pantalla al recibir daño
	public Image damageImage;
	//tiempo que durará el fade-out de la imagen de daño
	public float flashSpeed = 5f;
	//color de l aimagen de daño
	public Color flashColor = new Color (0.5f, 0.5f,0.5f,1f);

	//para controlar cuando el jugador está muerto
	bool isDead;
	//para controlar cuando el jugador recube daño
	public bool damaged;

	//duracion de tiempo entre ataques
	public float timeBetweenAttacks = 0.5f;
	//temporizador para la gestion del daño
	float timer;

	//para controlar si el jugador se encuentra en rango para recibir daño
	bool playerInRange;

	static public bool invencible;
	

	void Awake(){
		if (PM == null) {
			PM = GetComponent<PlayerMovement> ();
		}
	}

	// Use this for initialization
	void Start () {
		
		//recupero las referencias a los componentes
		animator = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();

		//para evitar que el navMesh rote al player, ya que lo rotaremos nosotros
		agent.updateRotation = false;

		//definimos la variable para la espera de tiempo entre pulsaciones
		inputHoldWait = new WaitForSeconds (inputHoldDelay);

		//inicialmente la posicion de destino, sera en la que se encuentre
		destinationPosition = transform.position;

		//buscamos el punto definido como entrada a la escena
		GameObject startPosition = GameObject.Find(DataManager.DM.data.startPosition);

		//si encontramos el punto de entrada en la escena
		if (startPosition) {
			//movemos al personaje hasta ese punto
			transform.position = startPosition.transform.position;
			//giramos al personaje para que mire en la direccion indicada por el punto de entrada
			transform.rotation = startPosition.transform.rotation;
		} else {
			//en caso de mo haber encontrdo la posicion de inicio, avisamos por consola con un warning
			Debug.LogWarning ("No se ha localizado la posicion de inicio :" + DataManager.DM.data.startPosition);
		}

		//inicializamos la vida actual para que sea igual a la vida máxima
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		//si ha pasado el tiempo entre ataques y el jugador se encuentra en rango para recibir daño
		if (timer >= timeBetweenAttacks && playerInRange) {
			DamagePlayer();
		}

		if (damaged) {
			//Si se ha recibido daño, hacemos que el color de la imagen de daño sea igual al predefinido
			damageImage.color = flashColor;
		} else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		//desactivamos el estado de daño recibido una vez coloreada la imagen de daño
		damaged = false;
		//cuando no este realizando ningun movimeinto, salimos del metodo
		if (agent.pathPending) {
			return;
		}

		//recupera la velocidad maxima deseada
		float speed = agent.desiredVelocity.magnitude;

		//cuando la distancia restante sea inferior  o igual al 10% de la distancia de parada
		if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion) {
			//llamamos a la funcion de parada
			Stopping(out speed);
		}else if (agent.remainingDistance <= agent.stoppingDistance) {
			Slowing (out speed, agent.remainingDistance);
		}else if (speed > turnSpeedthresHold) {
			Moving ();
		}

		//le indicamos al animator a que velocidad se esta moviendo el player 
		//para que ajuste el blend de la animacion en consecuencia
		animator.SetFloat ("Speed", speed, speedDampTime, Time.deltaTime);

		
	}

	/// <summary>
	/// Realiza las acciones de parada al llegar al destino
	/// </summary>
	/// <param name="speed">Speed.</param>
	private void Stopping(out float speed){

		//paramos el despalzamiento del Navmesh
		agent.isStopped = true;

		//ponemos la velocidad a 0
		speed = 0f;

		//si hay actualmente un objeto interactivo seleccionado en el momento de hacer la parada
		if (currentInteractable) {
			//hacemos que el pj mire hacia la posicion indicada en el objeto interactuable
			transform.rotation = currentInteractable.interactableLocation.rotation;

			//activamos la interaccion programada por el objeto
			currentInteractable.Interact();

			//ya que hemos interactuado con el objeto, lo ponemos a null para una futura pulsacion
			currentInteractable = null;

			//iniciamos la corrutina para realizar las esperas de tiempo
			//StartCoroutine (WaitForInteractions ());
		}
	}

	/// <summary>
	/// Reducimos la velocidad segun se acerca al destino
	/// </summary>
	/// <param name="speed">Speed.</param>
	/// <param name="distanceToDestination">Distance to destination.</param>
	private void Slowing(out float speed, float distanceToDestination){

		//paramos el desplazamiento del NavMesh
		agent.isStopped = true;

		//movemos al player hacia el destino, frenando su movimiento
		transform.position = Vector3.MoveTowards(transform.position, destinationPosition, slowingSpeed*Time.deltaTime);

		//distancia proporcional hacia el objetivo
		float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;

		//interpolamos la velocidad con la distancia restante, cuanto mas cerca, mas lento
		speed = Mathf.Lerp(slowingSpeed,0,proportionalDistance);

		//si como objetivo de desplazamiento esta asignado un interactivo haremos que la rotacion
		//final sea la indicada por la propiedad interactableLocation de objeto interactivo
		//en caso contrario, dejare que el pj siga con su rotacion actual
		Quaternion targetRotation = currentInteractable ? currentInteractable.interactableLocation.rotation
														: transform.rotation;

		//interpolamos el giro para suavizarlo
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, proportionalDistance);

	}

	/// <summary>
	/// Rotamos al personaje en la direccion del  movimiento
	/// </summary>
	private void Moving(){

		//averiguamos el angulo en el que tenemos que rotar para encarar el camino por el que se desplaza
		Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);

		//suavizamos el giro
		transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,turnSmoothing*Time.deltaTime);
	}


	public void OnGroungClick(BaseEventData data){

		//si no hay control del imput, no permitiremos hacer nada
		if (!handleInput) {
			return;
		}

		//si detectamos que se ha pulsado sobre el suelo, desactivamos la posible interaccion
		currentInteractable = null;

		//hacemos un cast del objeto recuperado por el evento de click
		//recuperando solo la informacion en un objeto que nos permita gestionarlo
		PointerEventData pData = (PointerEventData)data;

		//variable para almacenar el impacto recuperado con el click sobre el navmesh
		NavMeshHit hit;

		//mediante le pointerEventdata, podemos localizar un punto de impacto sobre el navMesh
		//recuperando la posicion de este para iniciar el desplazamiento
		if (NavMesh.SamplePosition (pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas)) {

			//si hemos localizado un impacto, lo asignamos como destino de desplazamiento
			destinationPosition = hit.position;
		} else {
			//si no se localiza un punto de impacto en el navmesh, movemos el player en la direccion del click
			destinationPosition = pData.pointerCurrentRaycast.worldPosition;
		}

		//fijamos en el navmesh el destino del desplazamiento
		agent.SetDestination (destinationPosition);

		//iniciamos el desplazamiento
		agent.isStopped = false;
	}

	public void OnInteractableClic (Interactable interactable){

		//si no hay control del imputn no hacemos nada
		if (!handleInput) {
			return;
		}

		//indicamos con que objeto interactivo vamos a interactuar
		currentInteractable = interactable;

		//fijamos la posicion de destino
		destinationPosition = currentInteractable.interactableLocation.position;

		// fijamos el destino del desplazamiento
		agent.SetDestination (destinationPosition);
		//iniciamos el desplazamiento
		agent.isStopped = false;

	}

	/// <summary>
	/// Funcion que realizara una espera hasta que la interaccion termine
	/// </summary>
	/// <returns>The for interactions.</returns>
	private IEnumerator WaitForInteractions(){
		
		//al inicio dejamos de gestional el imput
		handleInput = false;

		//realizamos una espera previa a la animacion, controlada por tiempo
		yield return inputHoldWait;

		//si la animacion es distinta a la de movimiento
		//mantenemos bloqueado el control del personaje
		while (!animator.GetCurrentAnimatorStateInfo (0).IsTag ("Locomotion")) {

			yield return null;
		}

		//una vez terminado el bucle devolvemos el control al jugador
		handleInput = true;
	}

	public void movePlayerAnimation(Transform target){
		destinationPosition = target.position;
		//fijamos en el navmesh el destino del desplazamiento
		agent.SetDestination (destinationPosition);

		//iniciamos el desplazamiento
		agent.isStopped = false;
	}


	public void DamagePlayer(){
		timer = 0f;
		//indicamos que el jugador ha recibido daño
		damaged = true;

		if(currentHealth > 0){
			//reducimos la vida del jugador
			currentHealth -= 20;
			//actualizamos la barra de vida
			healthSlider.value = currentHealth;
		}
		//si la vida es menor o igual que 0, consideramos que el jugador ha muerto
		if(currentHealth <= 0 && !isDead) {
			Death ();
		}
	}

	void  Death (){
		//indicamos que el ejugador está muerto
		isDead = true;
		GameOver.GO.isDead = true;
		animator.SetTrigger("PlayerDIE");

	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Fire"){
			if(invencible){
				return;
			}

			playerInRange = true;
		}
	}

}
