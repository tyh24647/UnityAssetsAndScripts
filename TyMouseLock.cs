using UnityEngine;
using System.Collections;

/**
 * Locks the mouse until the escape key has been pressed
 * 
 * @author	Tyler Hostager
 * @version 6/30/15
 */
public class TyMouseLock : MonoBehaviour {

	void OnApplicationFocus(bool status)
	{
		if (status) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
