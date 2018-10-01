#pragma strict

/*

This script controlls the player status.

*/

// The enemy target for this player.
var enemy : GameObject;

var life : float = 100; // Max life.
var stamina : float = 100; // Max stamina.
var staminaRecuperationFactor : int = 10; // If you increment this, it will gain stamina faster.

// Some private variables.
private var isDead : boolean = false;
private var canRegenerateStamina = true;

// Variables to access to others scripts.
private var playerAnimationScript : playerAnimation;
private var playerCombatScript : playerCombat;
private var playerMovementScript : playerMovement;
private var levelManagerScript : LevelManager;

private var isCovered = false;

function Start () {
	// Set external scripts.
	playerAnimationScript = transform.GetComponent(playerAnimation);
	playerCombatScript = transform.GetComponent(playerCombat);
	playerMovementScript = transform.GetComponent(playerMovement);
	levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent(LevelManager);
	
	if(!enemy){
		print("WARNING: You must set one enemy (the PC character) for this script in the Inspector View!");
	}
}

function Update () {
	// if player is not dead, regenerate stamina.
	if(!isDead){
		RegenerateStamina();	
	}
}

// This function apply damage for this player.
function Damage(amount : float){
	
	var totalAmount : float;
	
	if(isCovered){
		totalAmount = amount / 5;
	}else{
		totalAmount = amount;
		// If the player was damaged, add one successful hit to the enemy in LevelManager.js.
		levelManagerScript.AddSuccessfulHit("enemy");
	}
		
	life -= totalAmount;
	
	if(life <= 0){
		life = 0;
		isDead = true;
		gameObject.SendMessage("Dead");
	}
}

// this function reduce the stamina.
// Is called externally from PlayerCombat.js.
function LoseStamina(cantidad : float){
	stamina -= cantidad;
	if(stamina < 0){
		stamina = 0;
		canRegenerateStamina = false;
		yield WaitForSeconds(3);
		canRegenerateStamina = true;
	}
}

// this function regenerate stamina every frame.
// Is called in Update function.
function RegenerateStamina(){
	if(canRegenerateStamina){
		stamina += Time.deltaTime * staminaRecuperationFactor;
		stamina = Mathf.Clamp(stamina,0,100);
	}
}

function Covered(){
	isCovered = true;
}

function Uncovered(){
	isCovered = false;
}

// Player Dead.
// Informs to LevelManager.js that the player was Knockout.
function Dead(){
	isDead = true;
	levelManagerScript.KO("player");
}