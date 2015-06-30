#pragma strict

/**
* Switches views from 1st person to 3rd person
*
* @author Tyler Hostager
* @version 12/31/14
*/

var thirdPerson : GameObject;
//var firstPerson : Camera;
var fpHands : GameObject;

function Start () {
	//playerBody.SetActive(false);
	fpHands.SetActive(true);
	thirdPerson.SetActive(false);
	//firstPerson.active = true;
	//thirdPerson.active = false;
}

function Update () {
	if (Input.GetKeyDown("e")) {
		if (!fpHands.activeInHierarchy) {
			fpHands.SetActive(true);
			thirdPerson.SetActive(false);
		} else {
			fpHands.SetActive(false);
			thirdPerson.SetActive(true);
		}
	}
}