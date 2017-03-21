using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadController : MovingObject {

    private Constants carryingObject = Constants.none;	
    private string horizontalControls = "L_Horizontal";
    private string verticalControls = "L_Vertical";

	public AudioClip itemPickup;
	new AudioSource audio;

    protected override void Start() {
        base.Start();
		audio = GetComponent<AudioSource> ();
    }

    void Update() {
        if (CanMove()) {
            int horizontal = (int)(Input.GetAxisRaw(horizontalControls));
            int vertical = (int)(Input.GetAxisRaw(verticalControls));
            if (horizontal != 0) {
                vertical = 0;
            }
            if (horizontal != 0 || vertical != 0) {
                RaycastHit2D hit;
                Transform transform = Move(horizontal, vertical, out hit);
                if (transform)
                    interactWithObject(transform);
            }
        }
	}

    private void RemovePet(GameObject pet) {
        Destroy(pet);
    }

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void interactWithObject (Transform transform) {
		switch (transform.tag) {
		case "Pet":
			Debug.Log ("found pet!"); //TODO: assumed pet's bool methods: pet.feed() pet.done()
			PetController pet = (PetController)transform.GetComponent (typeof(PetController));
			if (carryingObject == Constants.food && pet.feed ()) {
				carryingObject = Constants.none;
			} else if (pet.isDone) {
				Destroy (transform.gameObject);
			}
			break;
		case "Poop":
			Debug.Log ("found poop :(");
			if (!isCarrying ()) {
				carryingObject = Constants.poop;
				Destroy (transform.gameObject);
				audio.PlayOneShot (itemPickup);
			}
			break;
		case "Kitchen":
			Debug.Log ("found kitchen");
			if (carryingObject == Constants.delivery) {
				incrementKitchenFood ();
                carryingObject = Constants.none;
			} else if (!isCarrying ()) {
				audio.PlayOneShot (itemPickup);
				if (decrementKitchenFood ()) {
                    carryingObject = Constants.food;
				}
			}
			break;
		case "Door":
			Debug.Log ("found door");
			if (!isCarrying ()) {
                carryingObject = Constants.delivery;
				audio.PlayOneShot (itemPickup);
			}
			break;
		case "Trash":
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

