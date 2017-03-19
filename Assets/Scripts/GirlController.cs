using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlController : MovingObject {

	private Constants carryingObject = Constants.none; //poop, food, brush, 
    private string horizontalControls = "R_Horizontal";
    private string verticalControls = "R_Vertical";

	public AudioClip itemPickup;
	AudioSource audio;

    protected override void Start() {
        base.Start();
		audio = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
     void Update() {
        if (CanMove()) {
            int horizontal = (int)(Input.GetAxisRaw(horizontalControls));
            int vertical = (int)(Input.GetAxisRaw(verticalControls));
            if (horizontal != 0) {
                vertical = 0;
            } else if (vertical != 0) {
                horizontal = 0;
            }
            if (horizontal != 0 || vertical != 0) {
                RaycastHit2D hit;
                Move(horizontal, vertical, out hit);
            }
        }
    }
	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other) {
		if (other == null) {
			return;
		}
		GameObject obj = other.gameObject;
		switch (obj.tag) {
		case "pet":
			Debug.Log ("found pet!"); //TODO: assumed pet's bool methods: pet.feed() pet.done()
			PetController pet = (PetController)obj.GetComponent (typeof(PetController));
			if (carryingObject == Constants.food && pet.feed ()) {
				carryingObject = "";
			}
//				else if (carryingObject == Constants.brush) {
//				pet.groom ();
//			} else if (pet.play()) {
//				//some animation here
//			}
			break;
		case "poop":
			Debug.Log ("found poop :(");
			if (!isCarrying ()) {
				carryingObject = Constants.poop;
				Destroy (obj);
				audio.PlayOneShot (itemPickup);
			}
			break;
		case "groomingContainer":
			Debug.Log ("found brush");
			if (!isCarrying ()) {
				carryingObject = Constants.brush;
				Destroy (obj);
				audio.PlayOneShot (itemPickup);
			} else if (carryingObject == Constants.brush) {
				carryingObject = Constants.none;
			}
			break;			
		case "kitchen":
			Debug.Log ("found kitchen");
			if (!isCarrying ()) {
				audio.PlayOneShot (itemPickup);
				if (decrementKitchenFood ()) {
					carryingObject = Constants.food;
				}
			}
			break;
		case "trash":
			Debug.Log ("found trash");
			carryingObject = Constants.none;
			break;
		}
	}

	public bool isCarrying() {
		return carryingObject != Constants.none;
	}

	public Constants getCarryingObject() {
		if (isCarrying ()) {
			return carryingObject;
		} else {
			return Constants.none;
		}
	}

}

