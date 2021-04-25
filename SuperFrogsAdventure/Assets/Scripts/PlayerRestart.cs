using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRestart : MonoBehaviour
{
    //serializing a field makes a private field accessible in the unity inspector
    [SerializeField] private LayerMask ground; 
    //LayerMask is the type of a layer
    [SerializeField] private float speed = 6.25f;
    [SerializeField] private float jumpForce = 12.8f;
    [SerializeField] private Collider2D feetCollider;
    [SerializeField] private Collider2D bodyCollider;

    private Rigidbody2D rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        Movement();
        Animation();
    }


    void OnTriggerEnter2D(Collider2D colliderTriggered)
    {
        if (colliderTriggered.tag == "DeathBox")
        {
        
            LivesScript.livesValue = 5;
             ScoreScript.scoreValue = 0;

           
           SceneManager.LoadScene("LevelUpdate");
            
        }
        if (colliderTriggered.tag == "Enemy")
        {
            //to add potentially: lose life
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            LivesScript.livesValue -=1;
             if(LivesScript.livesValue <=0)
            {
                SceneManager.LoadScene("MainLevel");
            }
        }
        if (colliderTriggered.tag == "CherryCollected"){
            Destroy(colliderTriggered.gameObject);
            ScoreScript.scoreValue += 5;
        }
        if (colliderTriggered.tag == "PineAppleCollected"){
            Destroy(colliderTriggered.gameObject);
            ScoreScript.scoreValue += 15;
        }
          if (colliderTriggered.tag == "TrophyCollected"){
            Destroy(colliderTriggered.gameObject);
            ScoreScript.scoreValue += 100;
        }
        
        if (colliderTriggered.tag == "Win"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
    void Movement()
    {
        var xAxis = Input.GetAxis("Horizontal");
        //left movement
        if (xAxis < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //right movement
        if (xAxis > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //jump only if key pressed and touching layer ground
        if (Input.GetButtonDown("Jump") && feetCollider.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping",true);
        }

    }

    void Animation()
    {
        //if jumping
        if (animator.GetBool("isJumping"))
        {
            //stop running
            animator.SetBool("isRunning", false);
            //if y velocity is negative
            if (rb.velocity.y < 0.1f)
            {
                //stop jumping, start falling
                animator.SetBool("isJumping",false);
                animator.SetBool("isFalling", true);
            }
        }
        //if falling
        else if (animator.GetBool("isFalling"))
        {
            //if touching ground
            if (feetCollider.IsTouchingLayers(ground))
            {
                //start idling
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
            }
        }
        //if absolute of x-velocity greater than 1
        else if (Mathf.Abs(rb.velocity.x) > 1f)
        {
            //start running
            animator.SetBool("isRunning", true);
        }
        //if none of above
        else
        {
            //start idling
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

}
