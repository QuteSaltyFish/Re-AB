#pragma strict

/*

This script control the player movements.

*/

// Player speeds
var movSpeed : float = 2.3;
var rotSpeed : int = 5;

// Some private variables.
private var isDead : boolean = false;
private var controller : CharacterController;
private var moveDirection : Vector3 = Vector3.zero;
private var rotInitial : Quaternion;
private var enemy : GameObject;

function Start(){
	// Set the enemy for the player.
	enemy = transform.GetComponent(playerStatus).enemy;
	// Set this initial Rotation.
	rotInitial = transform.rotation;
}

function Update () {
	
	// If the player is not dead, can be controlled.
	if(!isDead){
		controller = GetComponent(CharacterController);
		
		if(enemy){
			// Autorotate to the enemy.
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(enemy.transform.position - transform.position), rotSpeed * Time.deltaTime);
			transform.rotation.x = rotInitial.x;
			transform.rotation.z = rotInitial.z;
		}
		
		// Walk movement
		moveDirection = Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= movSpeed;
	   	controller.Move(moveDirection * Time.deltaTime);
	}
	
}

function Dead(){
	isDead = true;
}