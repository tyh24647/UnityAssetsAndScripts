using UnityEngine;
using System.Collections;

public class DisableMouse : MonoBehaviour {
	bool hideMouse = false;
	KeyCode newKey;

	// TODO fix to get rid of mouse on screen


	// Initialize hidden mouse
	void Start () {
		hideMouse = true;
		//newKey = Input.GetKey();
	}
	
	// Update called once per frame
	void Update () {
		if (Input.anyKeyDown.Equals(KeyCode.Escape)) {
			hideMouse = false;
		} hideMouse = true;
	}
}
