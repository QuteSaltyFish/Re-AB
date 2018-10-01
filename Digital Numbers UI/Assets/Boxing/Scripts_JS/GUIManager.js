#pragma strict

/*

This script show the Life and Stamina bars for the Player and the Enemy.

*/

var barTexture : Texture2D;

private var levelManagerScript : LevelManager;
private var playerStatusScript : playerStatus;
private var enemyStatusScript : AiScript;

function Start () {
	levelManagerScript = transform.GetComponent(LevelManager);
	playerStatusScript = levelManagerScript.player.GetComponent(playerStatus);
	enemyStatusScript = levelManagerScript.enemy.GetComponent(AiScript);
}

function OnGUI(){

	GUI.depth = 1;

	var widthLifeBar = (Screen.width / 2) - 10;
	var widthStaminaBar = (Screen.width / 2) - 10;

	if(enemyStatusScript){
		GUI.color = Color.red;
		GUI.DrawTexture(new Rect(10, 10, widthLifeBar * enemyStatusScript.life / 100  , 15), barTexture);
		GUI.color = Color.white;
		
		GUI.color = Color.green;
		GUI.DrawTexture(new Rect(10, 30, widthStaminaBar * enemyStatusScript.stamina / 100  , 8), barTexture);
		GUI.color = Color.white;
	}
	
	if(playerStatusScript){
		GUI.color = Color.red;
		GUI.DrawTexture(new Rect(widthLifeBar + 20, 10, (widthLifeBar - 10) * playerStatusScript.life / 100  , 15), barTexture);
		GUI.color = Color.white;
		
		GUI.color = Color.green;
		GUI.DrawTexture(new Rect(widthStaminaBar + 20, 30, (widthStaminaBar - 10) * playerStatusScript.stamina / 100  , 8), barTexture);
		GUI.color = Color.white;
	}
}