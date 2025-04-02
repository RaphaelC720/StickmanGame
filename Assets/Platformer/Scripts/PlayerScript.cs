using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour
{
    //I like to make variables for all my components even
        //if I'm not sure if I'll use them in code
    public SpriteRenderer SR;
    public Sprite FaceLeft;
    public Sprite FaceRight;
    public Sprite JumpSprite;
    public Rigidbody2D RB;
    public Collider2D Coll;
    public ParticleSystem PS;
    public AudioSource AS;
    
    //My personal stats
    public float Speed = 5;
    public float JumpPower = 10;
    public float Gravity = 3;
    
    //Variables I use to track my state
    public bool OnGround = false;
    public bool FacingLeft = false;
      //If this is over 0, I'm stunned and can't move
    public float Stunned = 0;
    
    //My Sound Effects
    public AudioClip JumpSFX;

    public GameObject Wheel;
    
    void Start()
    {
        //Set our rigidbody's gravity to match our stats 
        RB.gravityScale = Gravity;
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = FaceRight;
        
    }

    void Update()
    {
        //If I'm stunned. . .
        if (Stunned > 0)
        {
            //Make my stun timer go down
            Stunned -= Time.deltaTime;
            //If I'm still stunned, change my appearance
            if(Stunned > 0)
                SR.color = Color.gray;
            else //But if enough time passed, make me not stunned-looking
                SR.color = Color.white;
            //Don't run any of the code below, because I'm stunned
            return;
        }
        
        //I'll use this variable to track the movement I want
        //By default, I move like I moved last frame
        Vector2 vel = RB.linearVelocity;

        if (Input.GetKey(KeyCode.D))
        { 
            //If I hit right, move right
            vel.x = Speed;
            //If I hit right, mark that I'm not facing left
            FacingLeft = false;
            Debug.Log("Right");
        }
        else if (Input.GetKey(KeyCode.A))
        { 
            //If I hit left, move right
            vel.x = -Speed;
            //If I hit left, mark that I'm facing left
            FacingLeft = true;
        }
        else
        {  //If I hit neither, come to a stop
            vel.x = 0;
        }

        if(FacingLeft == true)
        {
            SR.sprite = FaceLeft;
        }
        else 
        {
            SR.sprite = FaceRight;
        }

        //If I hit Z and can jump, jump
        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        { 
            vel.y = JumpPower;
            //Emit 5 dust cloud particles
            PS.Emit(5);
            //Play my jump sound
           // AS.PlayOneShot(JumpSFX);
        }

        //Here I actually feed the Rigidbody the movement I want
        RB.linearVelocity = vel;
        //If I fall into the void...
        if (transform.position.y < -20)
        {
            //Give me a game over
            SceneManager.LoadScene("You Lose");
        }

        //Other controlls. other than jump/move
        if (Input.GetKeyDown(KeyCode.T))
        {
            OpenWheel();
        }
    }


    public void OpenWheel()
    {
        Wheel.SetActive(!Wheel.activeSelf);
    }
    //I use this function to track if I can jump or not
    //Right now it's very simple, but has a secret edge case bug
    //Can you find the bug?
    public bool CanJump()
    {
        return OnGround;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {  
        //If I collide with something solid, mark me as being on the ground
        OnGround = true;

        //If what I hit was an enemy. . .
      //  EnemyScript es = other.gameObject.GetComponent<EnemyScript>();
       // if (es != null)
        //{
            //Set me to be stunned
          //  Stunned = 0.75f;
            //Pick a direction to throw me in
            //Vector2 vel = new Vector2(5, 5);
            //If the monster's to my right, throw me left
            //if (other.transform.position.x > transform.position.x)
            //    vel.x *= -1;
            //And toss me
        //    RB.AddForce(vel,ForceMode2D.Impulse);
    //    }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        //If I stop touching something solid, mark me as not being on the ground
        OnGround = false;
    }

    
}
