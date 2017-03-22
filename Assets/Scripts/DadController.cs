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
		if (carryingObject == Constants.food)
			Debug.Log ("i has food");
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
		var animator = gameObject.GetComponent<Animator>();
		switch (transform.tag) {
		case "Pet":
			Debug.Log ("found pet!");
			PetController pet = (PetController)transform.GetComponent (typeof(PetController));
			if (carryingObject == Constants.food && pet.feed ()) {
				carryingObject = Constants.none;
				animator.SetTrigger ("dadIdle");
			}
			break;
		case "Poop":
			Debug.Log ("found poop :(");
			if (carryingObject == Constants.none) {
				carryingObject = Constants.poop;
				animator.SetTrigger ("dadPickUpPoop");
				audio.PlayOneShot (itemPickup);
				Destroy (transform.gameObject);
			}
			break;
		case "Kitchen":
			Debug.Log ("found kitchen");
			if (carryingObject == Constants.delivery) {
				incrementKitchenFood ();
                carryingObject = Constants.none;
				animator.SetTrigger ("dadIdle");
			} else if (carryingObject == Constants.none) {
				if (decrementKitchenFood ()) {
                    carryingObject = Constants.food;
					animator.SetTrigger ("dadPickUpFud");
					audio.PlayOneShot (itemPickup);
				}
			}
			break;
		case "Door":
			Debug.Log ("found door");
			if (carryingObject == Constants.none) {
                carryingObject = Constants.delivery;
				animator.SetTrigger ("dadPickUpDelivery");
				audio.PlayOneShot (itemPickup);
			}
			break;
		case "Trash":
			Debug.Log ("found trash");
			if (carryingObject != Constants.none) {
				animator.SetTrigger ("dadIdle");
				carryingObject = Constants.none;
			}
			break;
		}
	}
}

