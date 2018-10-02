#pragma strict

/*

This script controlls all the player animations.

*/

var cover : AnimationState;
var uncover : AnimationState;

private var isCovered : boolean = false;
private var isDead : boolean = false;

function Start(){
		
	GetComponent.<Animation>()["jab"].layer = 1;
	GetComponent.<Animation>()["jab"].blendMode = AnimationBlendMode.Additive;

	GetComponent.<Animation>()["cross"].layer = 1;
	GetComponent.<Animation>()["cross"].blendMode = AnimationBlendMode.Additive;

	GetComponent.<Animation>()["uppercutleft"].layer = 1;
	GetComponent.<Animation>()["uppercutleft"].blendMode = AnimationBlendMode.Additive;

	GetComponent.<Animation>()["uppercutright"].layer = 1;
	GetComponent.<Animation>()["uppercutright"].blendMode = AnimationBlendMode.Additive;
	
	GetComponent.<Animation>()["cubrirse"].layer = 2;
	GetComponent.<Animation>()["cubrirse"].blendMode = AnimationBlendMode.Additive;
	
	GetComponent.<Animation>()["descubrirse"].layer = 2;
	GetComponent.<Animation>()["descubrirse"].blendMode = AnimationBlendMode.Additive;
	
	GetComponent.<Animation>()["impactoDerecho"].layer = 1;
	GetComponent.<Animation>()["impactoDerecho"].blendMode = AnimationBlendMode.Additive;
	
	GetComponent.<Animation>()["impactoIzquierdo"].layer = 1;
	GetComponent.<Animation>()["impactoIzquierdo"].blendMode = AnimationBlendMode.Additive;
	
	GetComponent.<Animation>()["impactoBajoDerecho"].layer = 1;
	GetComponent.<Animation>()["impactoBajoDerecho"].blendMode = AnimationBlendMode.Additive;
	
	GetComponent.<Animation>()["impactoBajoIzquierdo"].layer = 1;
	GetComponent.<Animation>()["impactoBajoIzquierdo"].blendMode = AnimationBlendMode.Additive;
	
}

function Update(){

	var controller : CharacterController = GetComponent(CharacterController);

	if(!isDead){
	
		if(Input.GetAxis("Vertical") > 0){
			GetComponent.<Animation>().CrossFade("avanzar",0.1);
		}
		else if(Input.GetAxis("Vertical") < 0){
			GetComponent.<Animation>().CrossFade("caminar_atras",0.1);
		}
		else if(Input.GetAxis("Horizontal") > 0){
			GetComponent.<Animation>().CrossFade("caminar_derecha",0.1);
		}
		else if(Input.GetAxis("Horizontal") < 0){
			GetComponent.<Animation>().CrossFade("caminar_izquierda",0.1);
		}
		else if(!Input.GetAxis("Vertical") && !Input.GetAxis("Horizontal")){
			GetComponent.<Animation>().CrossFade("idle",0.1);
		}
	}else{
		GetComponent.<Animation>().CrossFade("muerte",0.2);
		this.enabled = false;
	}
}

function Attack(attackType : String) {
	GetComponent.<Animation>().Stop();
	GetComponent.<Animation>().Blend(attackType,1,0.1);
}

function Impact(hitType) {
	
	if(!isDead){
		if(hitType == "jab"){
			GetComponent.<Animation>().CrossFadeQueued("impactoIzquierdo",0.1,QueueMode.PlayNow);
		}
		if(hitType == "cross"){
			GetComponent.<Animation>().CrossFadeQueued("impactoDerecho",0.1,QueueMode.PlayNow);
		}
		if(hitType == "uppercutleft"){
			GetComponent.<Animation>().CrossFadeQueued("impactoBajoIzquierdo",0.1,QueueMode.PlayNow);
		}
		if(hitType == "uppercutright"){
			GetComponent.<Animation>().CrossFadeQueued("impactoBajoDerecho",0.1,QueueMode.PlayNow);
		}
	}
	
}

function Covered () {
	if(!isCovered){
		GetComponent.<Animation>().Blend("cubrirse",1,0.1);
		isCovered = true;
	}
}

function Uncovered () {
	if(isCovered){
		GetComponent.<Animation>().CrossFade("descubrirse",0.2);
		isCovered = false;
	}
}

function Dead () {
	isDead = true;
}