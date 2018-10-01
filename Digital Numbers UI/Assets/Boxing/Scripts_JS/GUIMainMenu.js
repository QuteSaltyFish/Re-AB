#pragma strict

/*

This script show the Start New Game button and the Controls Help in the Main Menu screen.

*/

// Make the script also execute in edit mode.
@script ExecuteInEditMode()

var controlsTexture : Texture2D;

function OnGUI(){
	
	GUI.depth = 0;
	
	if(GUI.Button(Rect(30,210,(Screen.width / 2) - 30,100),"Start New Game")){
		Application.LoadLevel("scene");
	}
	
	GUI.Box(Rect(0,Screen.height-110,Screen.width,110),"");
	GUI.DrawTexture(Rect(20,Screen.height-controlsTexture.height,controlsTexture.width,controlsTexture.height),controlsTexture);
}