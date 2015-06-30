using UnityEngine;
using System.Collections;


/**
 * Creates a virtual joystick object in which the player can control
 * the game using a smartphone.
 * 
 * @author	Tyler Hostager.
 * @version 3/6/15
 */
public class VirtualJoystick : MonoBehaviour {

	/** Default control permission. */
	private bool isControllable = true;

	/** Instantiate Vector movement. */
	[HideInInspector] public Vector2 movement = Vector2.zero;

	/** Placeholders for 2D textures. */
	private Texture2D padBTexture, padCTexture;

	/** Initialize Rectangle objects fo contain joystick items. */
	private Rect padBRect = new Rect(0, 0, 100, 100),
				 padCRect = new Rect(0, 0, 100, 100);

	/** Initialize Vectors to set GUI element positions. */
	private Vector2 padBPos = Vector2.zero,
					padCPos = Vector2.zero;

	/** Constant to contain joystick radius. */
	private const float padRadius = 50.0f;

	/** Motion attribute. */
	private bool isMovingFinger = false;

	/** Color values for textures. */
	private Color background, controller;



	/**
	 * Initializes the Joystick object, applies specified
	 * textures, sets the pixel locations, and applies
	 * background texture settings.
	 */
	void Awake() {
		this.init_textureColors();
		this.init_Texture(padBTexture, 1, 1);
		this.init_Texture(padCTexture, 1, 1);
		this.applyGUISettings(padBTexture);
		this.applyGUISettings(padCTexture);
	}



	/* Updates position, functionality, and variable values every frame. */
	void Update() {
		if (this.isControllable && Input.touchCount == 1) {
			Touch newTouch = Input.touches[0];

			Vector2 touchPos = new Vector2(
				newTouch.position.x, 
				(Screen.height - newTouch.position.y)
			);

			switch (newTouch.phase) {
			case TouchPhase.Began:
				this.isMovingFinger = true;
				this.padBPos = touchPos;
				this.padCPos = touchPos;
				break;
			case TouchPhase.Moved:
				this.padCPos = touchPos;
				float pDistance = Vector2.Distance(this.padBPos, this.padCPos);
				if (pDistance > VirtualJoystick.padRadius) {
					Vector2 padDir = (this.padCPos - this.padBPos);
					float t = (VirtualJoystick.padRadius / pDistance);
					this.padBPos = Vector2.Lerp(this.padCPos, this.padBPos, t);
				} break;
			case TouchPhase.Stationary:
				break;
			case TouchPhase.Canceled:
				this.isMovingFinger = false;
				this.padBPos = this.padCPos;
				break;
			case TouchPhase.Ended:
				this.isMovingFinger = false;
				this.padBPos = this.padCPos;
				break;
			}

		}
		Vector2 direction = (this.padCPos - padBPos);
		float distance = Vector2.Distance(this.padCPos, this.padBPos);
		if ((VirtualJoystick.padRadius / distance) > 3.5f) {
			this.movement = Vector2.zero;
		} else {
			this.movement = direction.normalized;
			if ((VirtualJoystick.padRadius / distance) > 1.5) {
				this.movement /= 2.0f;
			}
		}
	}


	public void OnGUI() {
		if (isMovingFinger && isControllable) {
			Rect bRect = new Rect (
				(padBPos.x - padBRect.width / 2.0f),
				(padBPos.y - padBRect.height / 2.0f),
				padBRect.width, padBRect.height
			), cRect = new Rect (
				(padCPos.x - padCRect.width / 2.0f),
				(padCPos.y - padCRect.height / 2.0f),
				padCRect.width, padCRect.height
			);
			GUI.DrawTexture(bRect, padBTexture);
			GUI.DrawTexture(cRect, padCTexture);
		}
	}



	public void setIsControllable(bool newCond) {
		isControllable = newCond;
	}
	


	/**
	 * Assigns the specified Texture2D object to the specified texture, as
	 * well as applies the specified color settings.
	 * 
	 * @param newTexture
	 * @param newColor
	 */
	private void init_textureColors() {
		setColor(padCTexture, 1f, 1f, 1f);
		setColor(padBTexture, 0f, 0f, 0f, 0.5f);
	}



	private void applyGUISettings(Texture2D texture) {
		if (texture == padBTexture) {
			texture.SetPixel(0, 0, background);
		} else {
			texture.SetPixel(0, 0, controller);
		}
		texture.Apply();
	}


	/**
	 * 
	 * 
	 * @param newTexture
	 * @param newWidth
	 * @param newHeight
	 */
	private void init_Texture(Texture2D newTexture, int newWidth, int newHeight) {
		newTexture = new Texture2D(newWidth, newHeight);
	}



	/**
	 * Assigns the given texture's color value to the specified RGB value.
	 * 
	 * @param newTexture 	The texture to be colorized.
	 * @param r 			Red
	 * @param g 			Green
	 * @param b 			Blue
	 */
	private void setColor(Texture2D newTexture, float r, float g, float b) {
		if (newTexture.Equals(padCTexture)) {
			controller = new Color(r, g, b);
		} else if (newTexture.Equals(padBTexture)) {
			setColor(newTexture, r, g, b, 0.5f);
		} else {
			return;
		}
	}



	/**
	 * Assigns the given texture's color value to the specified RGBa value.
	 * 
	 * @param newTexture 	The texture to be colorized.
	 * @param r 			Red
	 * @param g 			Green
	 * @param b 			Blue
	 * @param a				Additional specifications.
	 */
	private void setColor(Texture2D newTexture, float r, float g, float b, float a) {
		if (newTexture.Equals(padBTexture)) {
			background = new Color(r, g, b, a);
		} else {
			return;
		}
	}


	public bool IsControllable() {
		return isControllable;
	}
}
