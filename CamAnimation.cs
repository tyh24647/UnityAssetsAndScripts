using UnityEngine;
using System.Collections;

/**
 * Creates the animation for when the player walks
 * 
 * @Author Tyler Hostager
 * @Version 12-22-14
 * */
public class CamAnimation : MonoBehaviour {
	public CharacterController playerController;
	public Animation anim; //Empty GameObject's animation component
	private bool isMoving;
	private bool left;
	private bool right;

	//! TODO fix this class to have better walking animations!


	/** Sets initial animation values */
	void Start () { 
		left = true;
		right = false;
	}
	
	/** Updates frames after motion */
	void Update () {
		float inputX = Input.GetAxis("Horizontal"); // Keyboard input to determine if player is moving
		float inputY = Input.GetAxis("Vertical");
		
		if(inputX  != 0 || inputY != 0) {
			isMoving = true;       
		} else if(inputX == 0 && inputY == 0) {
			isMoving = false;      
		} CameraAnimations();
	}

	/** Sets the appropriate animatinos depening on player's direction */
	void CameraAnimations() {
		if(playerController.isGrounded) {
			if(isMoving) {
				if(left) {
					if(!anim.isPlaying) {//Waits until no animation is playing to play the next
						anim.Play("walkLeft");
						left = false;
						right = true;
					}
				} if(right) {
					if(!anim.isPlaying) {
						anim.Play("walkRight");
						right = false;
						left = true;
					}
				}
			}                      
		}
	}
}
