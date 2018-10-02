#pragma strict

/*

This script controlls all the AI script.
You should only change the exposed variables in the Inspector View.

It's a combination of playerStatus, playerMovement and playerCombat scripts, with randoms
decisions.

*/

var player : GameObject;

enum enemyState {offensive, defensive, idle}

var life : float = 100;
var stamina : float = 100;

var movSpeed : float = 2.3;
var rotSpeed : int = 5;

var staminaRecuperationFactor : int;

private var canRegenerateStamina = true;

private var currentState : enemyState;
private var iaAnimationScript : AiAnimation;

var damageJab : float;
var damageCross : float;
var damageUpperLeft : float;
var damageUpperRight : float;

private var canAttack : boolean = true;
var attackRatio : float;
private var timeForNextAttack : float;

private var canChangeState : boolean = true;
var changeStateRatio : float;
private var timeForNextState : float;

private var canChangeAction : boolean = true;
var actionRatio : float;
private var timeForNextAction : float;

private var hit : boolean = false;

private var hitType : String;

private var isDead : boolean = false;
private var damageCaused : float;

private var controller : CharacterController;
private var moveDirection : Vector3 = Vector3.zero;
private var rotInitial : Quaternion;
private var levelManagerScript : LevelManager;

var isCovered : boolean = false;

// Audio Vars
var attackMissed : AudioClip;
var attackLeft : AudioClip;
var attackRight : AudioClip;

function Start () {
	if(!player){
		print("WARNING: You must set one enemy (the player character) for this script in the Inspector View!");
	}
	levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent(LevelManager);
	iaAnimationScript = transform.GetComponent(AiAnimation);
	currentState = enemyState.idle;
}

function Update () {
	if(!isDead){
		RegenerateStamina();	
		if(player){
			// Aplicar AutoRotacion
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), rotSpeed * Time.deltaTime);
			transform.rotation.x = rotInitial.x;
			transform.rotation.z = rotInitial.z;
		}
		// Verificar si el estado necesita cambiarse.
		MakeDecisions();
		// Ejecutar los comandos del estado seleccionado.
		switch(currentState){	
			case enemyState.idle:
				break;
			case enemyState.offensive:			
				Offensive();
				break;
			case enemyState.defensive:
				Defensive();
				break;
		}
	}
}

function MakeDecisions(){

	if(timeForNextState > 0){
		timeForNextState -= Time.deltaTime;
		canChangeState = false;
	}
	if(timeForNextState < 0){
		timeForNextState = 0;
		canChangeState = true;
	}
	
	if(canChangeState){
		var randomState : int = Random.Range(1,4);
		
		if(randomState == 1){
			currentState = enemyState.idle;
			iaAnimationScript.walking = false;
			timeForNextState += changeStateRatio;
		}
		if(randomState == 2){
			currentState = enemyState.defensive;
			iaAnimationScript.walking = true;
			timeForNextState += changeStateRatio / 2;
		}
		if(randomState == 3){
			currentState = enemyState.offensive;
			iaAnimationScript.walking = true;
			timeForNextState += changeStateRatio;
		}
	}
	
	if(timeForNextAction > 0){
		timeForNextAction -= Time.deltaTime;
		canChangeAction = false;
	}
	if(timeForNextAction < 0){
		timeForNextAction = 0;
		canChangeAction = true;
	}
	
	if(canChangeAction){
		var randomAccion : int = Random.Range(1,11);
		if(randomAccion < 8){
			hit = true;
			gameObject.SendMessage("Uncovered");
			isCovered = false;
		}
		else if(randomAccion < 10){
			gameObject.SendMessage("Covered");
			isCovered = true;
			timeForNextAction = 1;
		}
		else{
			hit = false;
			gameObject.SendMessage("Uncovered");
			isCovered = false;
			timeForNextAction = 1;
		}
	}
	
	if(timeForNextAttack > 0){
		timeForNextAttack -= Time.deltaTime;
		canAttack = false;
	}
	if(timeForNextAttack < 0){
		timeForNextAttack = 0;
		canAttack = true;
	}
	
	if(hit){
		if(canAttack){
			var distOponente = Vector3.Distance(transform.position, player.transform.position);
				
			if(distOponente < 3){
				Attack();
				timeForNextAction = actionRatio;
			}				
		}
	}
}

function Covered(){
	isCovered = true;
}

function Uncovered(){
	isCovered = false;
}

function Offensive(){

	controller = GetComponent(CharacterController);
		
	moveDirection = Vector3(0, 0, 1);
	moveDirection = transform.TransformDirection(moveDirection);
	moveDirection *= movSpeed;
	controller.Move(moveDirection * Time.deltaTime);
}

function Defensive(){

	controller = GetComponent(CharacterController);
		
	moveDirection = Vector3(0, 0, -1);
	moveDirection = transform.TransformDirection(moveDirection);
	moveDirection *= movSpeed;
	controller.Move(moveDirection * Time.deltaTime);
}

function Attack () {
	
	var randomGolpe = Random.Range(1,5);
	
	if(randomGolpe == 1)
		hitType = "jab";
	if(randomGolpe == 2)
		hitType = "cross";
	if(randomGolpe == 3)
		hitType = "uppercutleft";
	if(randomGolpe == 4)
		hitType = "uppercutright";
	
	iaAnimationScript.Hit(hitType);
	
	timeForNextAttack += attackRatio;
	
	LoseStamina(5);
	
	var dist = Vector3.Distance(transform.position, player.transform.position);
	
	if(hitType == "jab"){
		if(dist < 1.95){
			damageCaused = stamina * damageJab / 100;
			player.SendMessage("Damage",damageCaused);
			player.SendMessage("Impact",hitType);
			LoseStamina(2);
			GetComponent.<AudioSource>().PlayOneShot(attackLeft);
		}else{
			GetComponent.<AudioSource>().PlayOneShot(attackMissed);
		}
	}
	if(hitType == "cross"){
		if(dist < 1.95){
			damageCaused = stamina * damageCross / 100;
			player.SendMessage("Damage",damageCaused);
			player.SendMessage("Impact",hitType);
			LoseStamina(2);
			GetComponent.<AudioSource>().PlayOneShot(attackRight);
		}else{
			GetComponent.<AudioSource>().PlayOneShot(attackMissed);
		}
	}
	if(hitType == "uppercutleft"){
		if(dist < 1.85){
			damageCaused = stamina * damageUpperLeft / 100;
			player.SendMessage("Damage",damageCaused);
			player.SendMessage("Impact",hitType);
			LoseStamina(5);
			GetComponent.<AudioSource>().PlayOneShot(attackLeft);
		}else{
			GetComponent.<AudioSource>().PlayOneShot(attackMissed);
		}
	}
	if(hitType == "uppercutright"){
		if(dist < 1.85){
			damageCaused = stamina * damageUpperRight / 100;
			player.SendMessage("Damage",damageCaused);
			player.SendMessage("Impact",hitType);
			LoseStamina(5);
			GetComponent.<AudioSource>().PlayOneShot(attackRight);
		}else{
			GetComponent.<AudioSource>().PlayOneShot(attackMissed);
		}
	}

			
}

function LoseLife(amount : float){
	
	var totalAmount : float;
	
	if(isCovered){
		totalAmount = amount / 5;
	}else{
		totalAmount = amount;
	}
	
	life -= totalAmount;
	
	if(life <= 0){
		life = 0;
		isDead = true;
		gameObject.SendMessage("Dead");
	}
}

function Dead(){
	isDead = true;
}

function Impact(){
	timeForNextAttack = 0.3;
}

function RegenerateStamina(){
	if(canRegenerateStamina){
		stamina += Time.deltaTime * staminaRecuperationFactor;
		stamina = Mathf.Clamp(stamina,0,100);
	}
}

function LoseStamina(amount : float){
	stamina -= amount;
	if(stamina < 0){
		stamina = 0;
		canRegenerateStamina = false;
		yield WaitForSeconds(3);
		canRegenerateStamina = true;
	}
}