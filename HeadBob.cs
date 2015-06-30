using UnityEngine;
using System.Collections;

/**
 * HeadBob class creates the illusion of a bobbing head by taking
 * the initial camera loaiton, moving the camera angle, and diisplaying the
 * new angle according to the desired speed. 
 * 
 * @author Tyler Hostager
 * @version
 */
public class HeadBob : MonoBehaviour {

	/** Initialize default camera y-coordinate */
	private float StartY;

	/** Default delta camera angle */
	private float Angle = 0;

	/** Initialize head bobbing amount container */
	public float BobAmount; 

	/** Initialize bobbing speed amount container */
	public float BobbingSpeed;

	/** Player CharacterController container */
	public CharacterController Controller; 


	/** Initializes default camera location */
	void Start () {
		StartY = transform.localPosition.y;  
		//Controller = transform.parent.gameObject.GetComponent<CharacterController> ();


		if (transform.parent.gameObject.name.Equals("Player 1") || 
                                    transform.parent.gameObject.name.Equals("Player")) {
			Controller = transform.parent.gameObject.GetComponent<CharacterController> ();
		}// else {
		//	Controller = transform.parent.parent.parent.gameObject.GetComponent<CharacterController>();
		//	}*/
	}
	
	/** Updates frame once per second */
	void Update () {
		if ((Mathf.Abs (Controller.velocity.x) > 1f) 
	    		|| (Mathf.Abs (Controller.velocity.z) > 1f)) {  
			transform.localPosition = new Vector3 (0, StartY + BobAmount * Mathf.Sin (Angle), 0);  
			Angle += BobbingSpeed * Time.deltaTime;  
			if (Angle >= 2 * Mathf.PI)  
				Angle = 0;  
		} else {  
			Angle = 0;  
			transform.localPosition = Vector3.Lerp (
				transform.localPosition,
				new Vector3 (0, StartY, 0), 0.2f
			);  
		}  
	}
}