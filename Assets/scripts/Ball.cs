using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
	// SET AUDIO
	public AudioSource oceanSound;
    public AudioSource ballCrack;
	public AudioSource audienceCheer;

	// SET REFERENCE TO (TRANSFORM) GAME OBJECTS
	public Transform player;
	public Transform bot;

	// SET STATES
	public bool playerIsHitter = true;
	public bool ballInPlay = false;
	public bool ballhitFrontWall;
	public int bounceCount;
	int playerScore;
	int botScore;
	public bool playerScoreAdded = false;
	public bool botScoreAdded = false;

	[SerializeField] Text playerScoreText;
	[SerializeField] Text botScoreText;

	public string currentServer;
	public string lastServer;
	public string lastServerPosition;

	// SET SERVE AND RECEIVE POSITIONS
	public Vector3 leftBox;
	public Vector3 rightBox;
	public Vector3 receiveLeft;
	public Vector3 receiveRight;


	// Use this for initialization
	void Start () {
		playerScore = 0;
		botScore = 0;
	}

	void Update () {
		// CONFETTI EFFECT BASED ON THE WINNER AND RESET SCORES
		if (playerScore == 11 || botScore == 11) {
			if (playerScore == 11) {
				GameObject.Find ("player").GetComponent<ParticleSystem>().Play();
			} else if (botScore == 11) {
				GameObject.Find ("bot").GetComponent<ParticleSystem>().Play();
			}

			playerScore = 0;
			botScore = 0;
			updateScores ();
			currentServer = "player";
		}
	}


	// WHEN RALLY ENDS, RESET POSITIONS OF PLAYER AND BOT
	public void Reset() {
		// IF PLAYER WON AND LAST SERVED RIGHT
		if (playerScoreAdded && lastServer == "player" && lastServerPosition == "right") {
			currentServer = "player";
			player.GetComponent<Transform>().position = leftBox;
			bot.GetComponent<Transform>().position = receiveRight;
		}

		// IF PLAYER WON AND LAST SERVED LEFT
		if (playerScoreAdded && lastServer == "player" && lastServerPosition == "left") {
			currentServer = "player";
			player.GetComponent<Transform>().position = rightBox;
			bot.GetComponent<Transform>().position = receiveLeft;
		}

		// IF PLAYER WON AND BOT LAST SERVED
		if (playerScoreAdded && lastServer == "bot") {
			currentServer = "player";
			player.GetComponent<Transform>().position = rightBox;
			bot.GetComponent<Transform>().position = receiveLeft;
		}

		// IF BOT WON AND PLAYER LAST SERVED
		if (botScoreAdded && lastServer == "player") {
			currentServer = "bot";
			bot.GetComponent<Transform>().position = rightBox;
			player.GetComponent<Transform>().position = receiveLeft;
		}

		// IF BOT WON AND LAST SERVED RIGHT
		if (botScoreAdded && lastServer == "bot" && lastServerPosition == "right") {
			currentServer = "bot";
			bot.GetComponent<Transform>().position = leftBox;
			player.GetComponent<Transform>().position = receiveRight;
		}

		// IF BOT WON AND LAST SERVED LEFT
		if (botScoreAdded && lastServer == "bot" && lastServerPosition == "left") {
			currentServer = "bot";
			bot.GetComponent<Transform>().position = rightBox;
			player.GetComponent<Transform>().position = receiveLeft;
		}

		playerScoreAdded = false;
		botScoreAdded = false;
	}
		

	void updateScores () {
		playerScoreText.text = "Player : " + playerScore;
		botScoreText.text = "Bot : " + botScore;
	}
		

	private void OnCollisionEnter(Collision collision) {
		
		// IF BALL HITS SIDE WALLS, PLAY BALL CRACK SOUND 
		if (collision.transform.CompareTag ("sideWall") && ballInPlay) {
			ballCrack.Play ();
		}


		// IF BALL HITS FRONT WALL, PLAY BALL CRACK SOUND AND RESET BOUNCECOUNT TO 0
		if (collision.transform.CompareTag ("frontWall") && ballInPlay) {
			ballhitFrontWall = true;
			ballCrack.Play ();
		} 

		else {
			ballhitFrontWall = false;
		}
	
		if (ballhitFrontWall) {
			bounceCount = 0;
		}


		// IF DOUBLE BOUNCE, CALL UPDATESCORES AND RESET FUNCTIONS
		if (collision.transform.CompareTag ("floor") && ballInPlay) {
			bounceCount++;
		}

		if (bounceCount == 2) {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			ballInPlay = false;
			audienceCheer.Play ();

			if (playerIsHitter) {
				playerScore++;
				playerScoreAdded = true;

			} else if (!playerIsHitter) {
				botScore++;
				botScoreAdded = true;

			}

			updateScores();
			Reset();
			bounceCount = 0;
		}


		// IF BALL GOES OUT, CALL UPDATESCORES AND RESET FUNCTIONS AS WELL
		if (collision.transform.CompareTag("out_zone") && ballInPlay) {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			ballInPlay = false;
			audienceCheer.Play ();

			if (playerIsHitter) {
				botScore++;
				botScoreAdded = true;

			} else if (!playerIsHitter) {
				playerScore++;
				playerScoreAdded = true;
			}

			updateScores();
			Reset();
		}
	}
}