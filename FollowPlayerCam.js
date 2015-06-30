#pragma strict

var target : Transform;
public var distance = 2.8;
public var targetHeight = 2.0;
var rotation : Quaternion;
private var x = 0.0;
private var y = 0.0;  

function Start () {
	//relativePosition = target.transform.position - transform.position;
	var angles = transform.eulerAngles;
	x = angles.x;
	y = angles.y;
}

function Update () {
	//transform.position = target.transform.position - relativePosition;
	if (!target) {
		return;
	} y = target.eulerAngles.y;
	
	// Rotates camera
	rotation = Quaternion.Euler(x, y, 0);
	transform.rotation = rotation;
	
	// Position camera
	var position = target.position - (rotation * Vector3.forward * distance + Vector3(0,-targetHeight,0));
	transform.position = position;
}