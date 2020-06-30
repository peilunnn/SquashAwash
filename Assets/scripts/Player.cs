using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Capsule {
	// SET PARTICLE SYSTEM FOR CONFETTI
	public ParticleSystem confetti;

	// SET REFERENCES TO OTHER GAME OBJECTS
	public Transform aimTarget;

	// SET STATES
	bool aiming;


	// Use this for initialization
	new void Start () {
		base.Start();
		GameObject.Find("ball").GetComponent<Ball> ().currentServer = "player";
		transform.position = GameObject.Find("ball").GetComponent<Ball>().rightBox;
	}


	// Update is called once per frame
	new void Update() {
		base.Update();

		// MOVEMENT OF PLAYER USING WASD OR ARROW KEYS
		float LR = Input.GetAxisRaw ("Horizontal");
		float FB = Input.GetAxisRaw ("Vertical");


		// USE H KEY TO TOGGLE AIMING MODE
		if (Input.GetKeyDown (KeyCode.H)) {
			aiming = true;
		}
		else if (Input.GetKeyUp (KeyCode.H)) {
			aiming = false;
		}


		// TOGGLE BETWEEN MOVING THE PLAYER AND PLAYER TARGET
		if (aiming) {
			aimTarget.Translate (new Vector3 (LR, 0, FB) * speed * 2 * Time.deltaTime);
		}

		if ((LR != 0 || FB != 0) && !aiming) {
			transform.Translate (new Vector3 (LR, 0, FB) * speed * Time.deltaTime);
		}


		// HOLD DOWN J KEY TO PREP LOB SERVE
		if (GameObject.Find("ball").GetComponent<Ball>().currentServer == "player" && (Input.GetKeyDown(KeyCode.J)) && !GameObject.Find("ball").GetComponent<Ball>().ballInPlay)
		{
			aiming = true;
			currentServe = serveManager.lob;
			GameObject.Find("ball").transform.position = transform.position + new Vector3 (1f, 1, 1);
			GetComponent<BoxCollider>().enabled = false;
		}


		// HOLD DOWN K KEY TO PREP HARD SERVE
		if (GameObject.Find("ball").GetComponent<Ball>().currentServer == "player" && (Input.GetKeyDown(KeyCode.K) && !GameObject.Find("ball").GetComponent<Ball> ().ballInPlay))
		{
			aiming = true;
			currentServe = serveManager.hard;
			GameObject.Find("ball").transform.position = transform.position + new Vector3 (1f, 1, 1);
			GetComponent<BoxCollider>().enabled = false;
		}


		// RELEASE J OR K KEY TO ACTUALLY SERVE
		if (GameObject.Find("ball").GetComponent<Ball>().currentServer == "player" && (Input.GetKeyUp(KeyCode.J) || (Input.GetKeyUp(KeyCode.K))) && !GameObject.Find("ball").GetComponent<Ball> ().ballInPlay)
		{
			aiming = false;
			GetComponent<BoxCollider>().enabled = true;
			Vector3 dir = aimTarget.position - transform.position;
			GameObject.Find("ball").GetComponent<Rigidbody>().velocity = dir.normalized * currentServe.hitForce * 2 + new Vector3 (0, currentServe.upForce, 0);
			animator.Play ("serve");
			GameObject.Find("ball").GetComponent<Ball>().playerIsHitter = true;
			GameObject.Find("ball").GetComponent<Ball>().ballInPlay = true;
			GameObject.Find("ball").GetComponent<Ball>().lastServer = "player";
			GameObject.Find("ball").GetComponent<Ball>().currentServer = null;
			if (transform.position == GameObject.Find("ball").GetComponent<Ball>().rightBox) {
				GameObject.Find("ball").GetComponent<Ball>().lastServerPosition = "right";
			} else if (transform.position == GameObject.Find("ball").GetComponent<Ball>().leftBox) {
				GameObject.Find("ball").GetComponent<Ball>().lastServerPosition = "left";
			}
		}


		// PREVENT PLAYER TARGET FROM PASSING THROUGH FLOOR AND CEILING
		Vector3 playerTargetPos = aimTarget.GetComponent<Transform>().position;
		if (playerTargetPos.y < -0.8f) {
			playerTargetPos.y = -0.8f;
			aimTarget.GetComponent<Transform>().position = playerTargetPos;
		}

		if (playerTargetPos.y > 25f) {
			playerTargetPos.y = 25f;
			aimTarget.GetComponent<Transform>().position = playerTargetPos;
		}
	}


	// PLAYER HITS THE BALL
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("ball")) {
			Vector3 dir = aimTarget.position - transform.position;
			other.GetComponent<Rigidbody> ().velocity = dir.normalized * force + new Vector3 (0, 10, 0);
		
			Vector3 playerToBall = GameObject.Find("ball").transform.position - transform.position;
			if (playerToBall.x >= 0) {
				animator.Play("forehand");
			}
			else
			{
				animator.Play("backhand");
			}
		// SET HITTER TO PLAYER
			GameObject.Find("ball").GetComponent<Ball>().playerIsHitter = true;
		}
	}
}