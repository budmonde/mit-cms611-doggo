using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadController : MovingObject {

    public float moveDuration = .2f;

	private string carryingObject = ""; //poop, food, delivery, 
    private float moveCd = 0;

    protected override void Start() {
        base.Start();
    }

    void Update() {
        if (moveCd >= 0) {
            Debug.Log(moveCd);
            moveCd -= Time.deltaTime;
            return;
        }

		int horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		int vertical = (int) (Input.GetAxisRaw ("Vertical"));
		if (horizontal != 0) {
			vertical = 0;
		}
		if (horizontal != 0 || vertical != 0) {
			RaycastHit2D hit;
			Move(horizontal, vertical, out hit);
            moveCd = moveDuration;
		}
	}

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other) {
		if (other == null) {
			return;
		}
		GameObject obj = other.gameObject;
		switch (obj.tag) {
//		case "pet":
//			Debug.Log ("found pet!"); //TODO: assumed pet's bool methods: pet.feed() pet.done()
//			PetController pet = (PetController)obj.GetComponent (typeof(PetController));
//			if (carryingObject == "food" && pet.feed ()) {
//				carryingObject = "";
//			} else if (pet.done ()) {
//				Destroy (obj);
//			}
//			break;
		case "poop":
			Debug.Log ("found poop :(");
			if (!isCarrying ()) {
				carryingObject = "poop";
				Destroy (obj);
			}
			break;
		case "kitchen":
			Debug.Log ("found kitchen");
			if (carryingObject == "delivery") {
				incrementKitchenFood ();
				carryingObject = "";
			} else if (!isCarrying ()) {
				if (decrementKitchenFood ()) {
					carryingObject = "food";
				}
			}
			break;
		case "door":
			Debug.Log ("found door");
			if (!isCarrying ()) {
				carryingObject = "delivery";
			}
			break;
		case "trash":
			Debug.Log ("found trash");
			carryingObject = "";
			break;
		}
	}

	public bool isCarrying() {
		return carryingObject.Length < 1;
	}

	public string getCarryingObject() {
		if (isCarrying ()) {
			return carryingObject;
		} else {
			return "";
		}
	}
}

