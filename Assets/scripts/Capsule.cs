using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour {
	// SET REFERENCE TO (TRANSFORM) GAME OBJECTS
	public Transform player;
	public Transform bot;

	// SET SPEED AND FORCE OF SWING
	public float speed = 20f;
	public float force = 70;

	public Animator animator;
	public ServeManager serveManager;
	public ServeType currentServe;


	// Use this for initialization
	public void Start () {
		animator = GetComponent<Animator> ();
		serveManager = GetComponent<ServeManager>();
	}


	// Update is called once per frame
	public void Update () {
		// PREVENT CAPSULE FROM PASSING THROUGH WALLS
		Vector3 capsulePos = transform.position;
		if (capsulePos.x < -12.11f) {
			capsulePos.x = -12.11f;
			transform.position = capsulePos;
		}

		if (capsulePos.x > 23.35793f) {
			capsulePos.x = 23.35793f;
			transform.position = capsulePos;
		}

		if (capsulePos.z < 0.2559745f) {
			capsulePos.z = 0.2559745f;
			transform.position = capsulePos;
		}

		if (capsulePos.z > 53.33958f) {
			capsulePos.z = 53.33958f;
			transform.position = capsulePos;
		}
	}
}