using UnityEngine;
using System.Collections;


/**
 * This class animates the player's gun object, providing and queuing animations, sounds
 * and rotation information during gameplay.
 * 
 * @author	Tyler Hostager
 * @version	6/30/15
 */
public class PlayerGun_semiAuto : MonoBehaviour {

	// init public vars
	public GameObject playerHands;
	public Animation gunAnimation;
	public AudioSource audioSource;
	public AudioSource reloadSource;
	public EnergyBar energyBar;

	// Init private vars
	private AudioClip shotFired, reload;
	private int runningTime, shotsRemaining, clipsRemaining, currentDelta, stamina, tmp;
	private bool isRunning, shotsAllowed, allowAnimations, gameStart, staminaDrain;
	private Transform playerHandsTransform;
	private float currentY, originalX, originalZ;
	private Quaternion normalRotation;
	private Space rotationContext; 

	// Init defaults/constants
	private static sealed int DEFAULT_NUM_SHOTS = 16, DEFAULT_NUM_CLIPS = 5, LEFT = -1, RIGHT = 1, QUARTER_ROTATION = 90,
								DEFAULT_STAMINA = 500;
	private static sealed bool DEFAULT_ANIMATION_PERMISSIONS = true;
	private static sealed Vector3 DEFAULT_VECTOR_SHIFT_AMT = new Vector3(1, 0, 0);
	private static sealed Space DEFAULT_WORLD = Space.World;


	// Use this for initialization
	void Start() {
		init_defaultRunStatus();
		init_playerHandsTransform();
		init_startRotation();
		init_originalRotations();
		init_audioClips();
		init_shotsRemaining();
		init_shotsAllowed();
		init_clipsRemaining();
		init_allowAnimations();
		init_world();
		gameStart = true;
		stamina = DEFAULT_STAMINA;
		staminaDrain = false;
		tmp = 1000;

	}
	
	// Update is called once per frame
	void Update() {
		applyAppropriateAnimation();


		if (staminaDrain) {
			if (tmp % 3 == 0) {
			stamina--;
			} else {
				tmp--;
			}
			print ("energy remaining: " + stamina);
			//energyBar.SetValueCurrent(stamina);
		} if (stamina == 0) {
			setPlayerRunning(false);
			staminaDrain = false;
			stamina++;
		} 
		/*
		if (stamina == 500) {
			setPlayerRunning(true);
			staminaDrain = true;
		}*/


		energyBar.SetValueCurrent(stamina);
	}
	
	private void applyAppropriateAnimation() {

		// Check for sprint button
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift)) {

			// Enable player sprint
			setPlayerRunning(true);
			staminaDrain = true;

			// Rotate if applicable
			if (playerIsRunning() && currentY != -90) {

				//energyBar.SetValueCurrent(stamina);
				//print("Remaining energy: " + stamina);
				updateGunRotation();
				playerHandsTransform.transform.position += DEFAULT_VECTOR_SHIFT_AMT;
				updateDeltaCount();
				print(currentDelta);
				//playerHandsTransform.transform.position.Set(0.95f, 0.8f, 3.25f);
				setCurrentY(-90);
				setShotsAllowed(false);
			}
		} else {
			staminaDrain = false;
			// Disable sprinting
			setPlayerRunning(false);
			setShotsAllowed(true);

			// Rotate if applicable
			if (currentY != 0) {
				updateGunRotation();
				//playerHandsTransform.transform.position -= DEFAULT_VECTOR_SHIFT_AMT;

				//playerHandsTransform.transform.position = Vector3.Lerp(
				//	 playerHandsTransform.transform.position, playerHands.transform.position, 2 * Time.deltaTime);

				/****/
				//playerHandsTransform.transform.position.Set(1f, 0.8f, 3.25f);
				playerHandsTransform.position.Set(0, 0.8f, 0);


				//resetCurrentDelta();
				//playerHandsTransform.transform.position -= new Vector3(0, currentDelta, 0);
				//playerHandsTransform.transform.position.Set(0.95f, 0.8f, 3.25f);
				//playerHandsTransform.transform.position.Set(-1f, 0.8f, -0.3f);
				setCurrentY(0);
			}
		} if (Input.GetMouseButtonDown(0) && getShotsAllowed() && !gunAnimation.IsPlaying("reload")) {
			this.animateShotFired();
			this.fireBullet();
		} else if (Input.GetKeyDown(KeyCode.R)) {// && !gunAnimation.isPlaying) {
			animateReload();
		}
	
		//this.animateIdle();
	}

	private void animateIdle() {
		if (!gunAnimation.IsPlaying("reload") || !gunAnimation.IsPlaying("fire") || !gunAnimation.IsPlaying("ready")) {
			gunAnimation.PlayQueued("idle");
		}
	}

	private void init_world() {
		setRotationContext(DEFAULT_WORLD);
		setCurrentDelta(0);
	}

	private void setCurrentDelta(int newDelta) {
		currentDelta = newDelta;
	}

	private void updateDeltaCount() {
		currentDelta++;
	}

	private void resetCurrentDelta() {
		currentDelta = 0;
	}

	Space getCurrentRotationSpace() {
		if (rotationContext == null) {
			print("ERROR: Cannot retrieve current rotation space. Null reference.");
		}
		return rotationContext;
	}

	private void setRotationContext(Space newSpace) {
		if (newSpace != null) {
			rotationContext = newSpace;
		}
	}

	private void updateGunRotation() {
		if (playerIsRunning() && !getCurrentY().Equals(-QUARTER_ROTATION)) {
			object_rotateAboutY(LEFT);
		} else {
			object_rotateAboutY(RIGHT);
		}
	}

	private void object_rotateAboutY(int newRotation) {
		int tmpRotationAmt;

		if (newRotation == LEFT) {
			tmpRotationAmt = -QUARTER_ROTATION;
		} else {
			tmpRotationAmt = QUARTER_ROTATION;
		}

		playerHandsTransform.Rotate(originalX, tmpRotationAmt, originalZ, rotationContext);
		//playerHandsTransform.parent.transform.Rotate(originalX, tmpRotationAmt, originalZ, Space.Self);
	}

	private void init_allowAnimations() {
		setAnimationPermissions(DEFAULT_ANIMATION_PERMISSIONS);
	}

	private void setAnimationPermissions(bool newAnimationPermissions) {
		if (newAnimationPermissions) {
			enable_animations();
		} else {
			disable_animations();
		}
	}

	private void enable_animations() {
		allowAnimations = true;
	}

	private void disable_animations() {
		allowAnimations = false;
	}

	bool canAnimateShots() {
		return allowAnimations;
	}

	private void init_clipsRemaining() {
		setClipsRemaining(DEFAULT_NUM_CLIPS);
	}

	private void setClipsRemaining(int newClipsRemaining) {
		clipsRemaining = newClipsRemaining;
	}

	int getClipsRemaining() {
		return clipsRemaining;
	}

	private void init_shotsAllowed() {
		setShotsAllowed(true);
	}

	private void setShotsAllowed(bool newShotsAllowed) {
		shotsAllowed = newShotsAllowed;
	}

	bool getShotsAllowed() {
		return shotsAllowed;
	}

	private void animateShotFired() {
		gunAnimation.Stop();
		gunAnimation.PlayQueued("fire");
		audioSource.PlayOneShot(shotFired);
	}

	private void animateReload() {
		setShotsAllowed(false);
		if (getShotsRemaining() < 16) {
			gunAnimation.Stop();
			gunAnimation.PlayQueued("reload");
			reloadSource.PlayOneShot(reload);
		} else {
			print("ERROR: Unable to reload clip. Clip already full.");
		}
		updateInfoAfterReload();
	}

	private void init_shotsRemaining() {
		setShotsRemaining(DEFAULT_NUM_SHOTS);
	}

	private void fireBullet() {
		if (getShotsRemaining() >= 0) {
			setShotsRemaining(shotsRemaining - 1);
			print("Shots remaining: " + getShotsRemaining());
			if (getShotsRemaining() < 1) {
				setShotsAllowed(false);
				animateReload();
				setClipsRemaining(clipsRemaining - 1);
				print("Clips remaining: " + getClipsRemaining());
				if (getClipsRemaining() < 1) {
					//setShotsAllowed(false);
					print("Disabling shots. No more bullets or magazines found.");
				}
			}
		} else {
			print("ERROR: Cannot fire shot. No bullets left in the magazine.");
		}
	}

	private void updateInfoAfterReload() {
		bool waitForAnimation = true;

		//if (gunAnimation.isPlaying) {
			//while (waitForAnimation) {
				if (!gunAnimation.isPlaying) {
					waitForAnimation = false;
				}
			//}
		//}
		setShotsRemaining(DEFAULT_NUM_SHOTS);

		if (getClipsRemaining() > 0) {
			setShotsAllowed(true);
		}
	}

	private void setShotsRemaining(int newShotsRemaining) {
		if (newShotsRemaining >= 0) {
			shotsRemaining = newShotsRemaining;
		} else {
			print("ERROR: invalid number of shots remaining. Please debug.");
		}
	}

	int getShotsRemaining() {
		return shotsRemaining;
	}

	private void init_audioClips() {
		shotFired = audioSource.clip;
		reload = reloadSource.clip;
	}

	private void init_originalRotations() {
		//originalX = playerHandsTransform.rotation.x;
		//originalZ = playerHandsTransform.rotation.z;
		originalX = originalZ = 0;
	}

	private void init_startRotation() {
		normalRotation = playerHandsTransform.rotation;
	}

	/*
	private void setCurrentX(int newCurrentX) {
		currentX = newCurrentX;
	}*/

	private void setCurrentY(int newCurrentY) {
		currentY = newCurrentY;
	}

	/*
	private void setCurrentZ(int newCurrentZ) {
		currentZ = newCurrentZ;
	}*/

	private void init_playerHandsTransform() {
		setPlayerHandsTransform(playerHands);
	}

	private void setPlayerHandsTransform(GameObject newPlayerArms) {
		playerHandsTransform = newPlayerArms.transform;
	}

	private void init_defaultRunStatus() {
		setPlayerRunning(false);
	}

	private void setPlayerRunning(bool newIsRunning) {
		isRunning = newIsRunning;
	}

	private bool playerIsRunning() {
		return isRunning;
	}

	/*
	private float getCurrentX() {
		return currentX;
	}*/

	private float getCurrentY() {
		return currentY;
	}

	/*
	private float getCurrentZ() {
		return currentZ;
	}*/
}
