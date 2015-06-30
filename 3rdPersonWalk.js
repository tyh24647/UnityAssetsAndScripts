#pragma strict

/**
* animates character for walking in 3rd person
*
* @author Tyler Hostager
* @version 12/31/14
*/
var animate : boolean;
var playerObj : GameObject;

function Start () {
	GetComponent.<Animation>().Play("idle_1 (1)");
	animate = false;
}

function Update () {
	if (animate) {
		GetComponent.<Animation>().Play("walking_1 (1)");
	} else {
		GetComponent.<Animation>().Play("idle_1 (1)");
	} if (Input.GetKeyDown("w")) {
		animate = true;	
	} if (Input.GetKeyUp("w")) {
		animate = false;
	}
}