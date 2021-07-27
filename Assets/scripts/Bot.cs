using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Capsule
{
    // SET REFERENCES TO OTHER GAME OBJECTS
    public Transform tPosition;
    public Transform aimTargetBot;

    // SET LIST OF RANDOMIZED TARGETS
    public Transform[] targets;

    // SET INITIAL POSITION OF BOT
    Vector3 initialBotPos;


    // Use this for initialization
    new void Start()
    {
        base.Start();
        initialBotPos = transform.position;
        InvokeRepeating("botExecuteServe", 6, 8);
    }


    // BOT MOVEMENT DURING RALLY
    void Move()
    {
        // IF PLAYER IS HITTER, MAKE BOT FOLLOW BALL AND ENABLE COLLIDER
        if (GameObject.Find("ball").GetComponent<Ball>().ballInPlay && GameObject.Find("ball").GetComponent<Ball>().playerIsHitter)
        {
            initialBotPos.x = GameObject.Find("ball").transform.position.x;
            transform.position = Vector3.MoveTowards(transform.position, initialBotPos, speed * Time.deltaTime);
            GetComponent<BoxCollider>().enabled = true;
        }

        // IF PLAYER IS BOT, MAKE BOT MOVE BACK TO THE T POSITION AND DISABLE CCOLLIDER
        if (GameObject.Find("ball").GetComponent<Ball>().ballInPlay && !GameObject.Find("ball").GetComponent<Ball>().playerIsHitter)
        {
            transform.position = Vector3.MoveTowards(transform.position, tPosition.position, speed * 0.75f * Time.deltaTime);
            GetComponent<BoxCollider>().enabled = false;
        }
    }


    // GENERATE A RANDOM VALUE AND ASSIGN THAT TO CORRESPONDING BOT SERVE
    ServeType PickServeType()
    {
        int randomValue = Random.Range(0, 2);
        if (randomValue == 0)
            return serveManager.lob;
        else
            return serveManager.hard;
    }


    // IF CURRENT SERVER IS NOW BOT AND BALL IS NOT IN PLAY
    void botExecuteServe()
    {
        if (GameObject.Find("ball").GetComponent<Ball>().currentServer == "bot" && !GameObject.Find("ball").GetComponent<Ball>().ballInPlay)
        {
            GameObject.Find("ball").transform.position = transform.position + new Vector3(1f, 1, 1);
            currentServe = PickServeType();
            Vector3 dir = aimTargetBot.position - transform.position;
            GameObject.Find("ball").GetComponent<Rigidbody>().velocity = dir.normalized * currentServe.hitForce * 2 + new Vector3(0, currentServe.upForce, 0);
            animator.Play("serve");
            GameObject.Find("ball").GetComponent<Ball>().playerIsHitter = false;
            GameObject.Find("ball").GetComponent<Ball>().ballInPlay = true;
            GameObject.Find("ball").GetComponent<Ball>().lastServer = "bot";
            GameObject.Find("ball").GetComponent<Ball>().currentServer = null;
            if (transform.position == GameObject.Find("ball").GetComponent<Ball>().rightBox)
            {
                GameObject.Find("ball").GetComponent<Ball>().lastServerPosition = "right";
            }
            else if (transform.position == GameObject.Find("ball").GetComponent<Ball>().leftBox)
            {
                GameObject.Find("ball").GetComponent<Ball>().lastServerPosition = "left";
            }
        }
    }


    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Move();
    }


    // GENERATE A RANDOM VALUE AND ASSIGN THAT TO CORRESPONDING BOT TARGET
    Vector3 PickTarget()
    {
        int randomValue = Random.Range(0, targets.Length);
        return targets[randomValue].position;
    }


    // WHEN BALL COLLIDES WITH BOT
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            Vector3 dir = PickTarget() - transform.position;
            other.GetComponent<Rigidbody>().velocity = dir.normalized * force + new Vector3(0, 10, 0);

            Vector3 botToBall = GameObject.Find("ball").GetComponent<Transform>().position - transform.position;
            if (botToBall.x >= 0)
                animator.Play("forehand");
            else
                animator.Play("backhand");
            // SET HITTER TO BOT
            GameObject.Find("ball").GetComponent<Ball>().playerIsHitter = !GameObject.Find("ball").GetComponent<Ball>().playerIsHitter;
        }
    }
}