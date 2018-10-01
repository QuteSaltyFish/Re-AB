#pragma strict

/*

This script controlls the player combat.

*/

// Damage for each attack type
var damageJab : float;
var damageCross : float;
var damageUpperLeft : float;
var damageUpperRight : float;

var attackRatio : float; // If you increment this, the player can hit more faster.

// Audio Vars
var attackMissed : AudioClip;
var attackLeft : AudioClip;
var attackRight : AudioClip;

// Some private vars.
private var timeForNextAttack : float;
private var canAct : boolean = true;
private var isDead : boolean = false;
private var damageCaused : float;
private var enemy : GameObject;
private var playerStatusScript : playerStatus;
private var levelManagerScript : LevelManager;

function Start(){
	playerStatusScript = transform.GetComponent(playerStatus);
	levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent(LevelManager);
	enemy = playerStatusScript.enemy;
}

function Update () {

	if(!isDead){

		if(timeForNextAttack > 0){
			timeForNextAttack -= Time.deltaTime;
			canAct = false;
		}
		if(timeForNextAttack < 0){
			timeForNextAttack = 0;
			canAct = true;
		}
	
		if(Input.GetKeyDown(KeyCode.R) && canAct){
			gameObject.SendMessage("Attack","jab");
		}
		if(Input.GetKeyDown(KeyCode.T) && canAct){
			gameObject.SendMessage("Attack","cross");
		}
		if(Input.GetKeyDown(KeyCode.F) && canAct){
			gameObject.SendMessage("Attack","uppercutleft");
		}
		if(Input.GetKeyDown(KeyCode.G) && canAct){
			gameObject.SendMessage("Attack","uppercutright");
		}
		
		if(Input.GetKey(KeyCode.E)){
			gameObject.SendMessage("Covered");
		}
		else{
			gameObject.SendMessage("Uncovered");
		}
	}
}

function Attack (attackType : String) {

	timeForNextAttack = attackRatio;
	playerStatusScript.LoseStamina(5);
	
	if(enemy){
		var dist = Vector3.Distance(transform.position, enemy.transform.position);
	}
	
	if(attackType == "jab"){
		if(dist < 1.95){
			damageCaused = playerStatusScript.stamina * damageJab / 100;
			enemy.SendMessage("LoseLife",damageCaused);
			enemy.SendMessage("Impact",attackType);
			playerStatusScript.LoseStamina(2);
			GetComponent.<AudioSource>().PlayOneShot(attackLeft);
		}else{
			GetComponent.<AudioSource>().PlayOneShot(attackMissed);
		}
	}
	if(attackType == "cross"){
		if(dist < 1.95){
			damageCaused = playerStatusScript.stamina * damageCross / 100;
			enemy.SendMessage("LoseLife",damageCaused);
			enemy.SendMessage("Impact",attackType);
			playerStatusScript.LoseStamina(2);
			GetComponent.<AudioSource>().PlayOneShot(attackRight);
		}else{
			GetComponent.<AudioSource>().PlayOneShot(attackMissed);
		}
	}
	if(attackType == "uppercutleft"){
		if(dist < 1.85){
			damageCaused = playerStatusScript.stamina * damageUpperLeft / 100;
			enemy.SendMessage("LoseLife",damageCaused);
			enemy.SendMessage("Impact",attackType);
			playerStatusScript.LoseStamina(5);
			GetComponent.<AudioSource>().PlayOneShot(attackLeft);
		}else{
			GetComponent.<AudioSource>().PlayOneShot(attackMissed);
		}
	}
	if(attackType == "uppercutright"){
		if(dist < 1.85){
			damageCaused = playerStatusScript.stamina * damageUpperRight / 100;
			enemy.SendMessage("LoseLife",damageCaused);
			enemy.SendMessage("Impact",attackType);
			playerStatusScript.LoseStamina(5);
			GetComponent.<AudioSource>().PlayOneShot(attackRight);
		}else{
			GetComponent.<AudioSource>().PlayOneShot(attackMissed);
		}
	}
	
	levelManagerScript.AddHit("player");
	
			
}

function Dead(){
	isDead = true;
}

// If the player was impacted, need to wait the attackRatio seconds for the next attack.
function Impact(){
	timeForNextAttack = attackRatio;
}