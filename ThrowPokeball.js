#pragma strict

var parentBone : GameObject;
var pokeballBody : Rigidbody;
var inertia : Vector3;

function Start () {
	transform.parent = parentBone.transform;
	pokeballBody.useGravity = false;
}

function Update () {
	ReleaseMe();
	setInertia();
}

function ReleaseMe() {
	if (Input.GetMouseButtonDown(0)) {
		transform.parent = null;
		pokeballBody.useGravity = true;
		transform.rotation = parentBone.transform.rotation;
		pokeballBody.AddForce(transform.forward * 20000);
	} else {
		transform.parent = parentBone.transform;
		pokeballBody.useGravity = false;
	}
}

function setInertia() {
	inertia.x = pokeballBody.mass * (1.0/12.0) * 1.6 * 1.6 + pokeballBody.mass * 0.25 * 0.15 * 0.15;
	inertia.y = pokeballBody.mass * 0.5 * 0.15 * 0.15;
	inertia.z = inertia.x;
	pokeballBody.inertiaTensor = inertia;
}