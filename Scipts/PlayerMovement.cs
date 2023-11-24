using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer sprite;
    private BoxCollider2D collision;

    public float xAxisMovement;
    public float moveSpeed = 7f;
    public float jump = 14f;
    public LayerMask Ground;


    private enum MovementAnimations {
        idle,
        running,
        jumping,
        falling };

    // Start is called before the first frame update
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        collision = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        xAxisMovement = Input.GetAxisRaw("Horizontal");
        rigidBody.velocity = new Vector3(xAxisMovement * moveSpeed, rigidBody.velocity.y, 0);

        if (Input.GetKeyDown("space") && JumpCheck()) // handles code regarding jumping using the space bar
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jump, 0);
        }

        Animations();
    }

    private void Animations()
    {
        MovementAnimations state;

        if (xAxisMovement > 0f) // if the 2d character is running to the right
        {
            state = MovementAnimations.running;
            sprite.flipX = false;
        }
        else if (xAxisMovement < 0f) // if the 2d charcter is running to the left
        {
            state = MovementAnimations.running;
            sprite.flipX = true; //flips direction player faces
        }
        else
        {
            state = MovementAnimations.idle; // the player is stationary
        }

        if (rigidBody.velocity.y > .1f) //allows to check if you are jumping, y velocity will only be zero if you are on the ground
        {
            state = MovementAnimations.jumping;
        }
        else if (rigidBody.velocity.y < -.1f) //velocity is downward to player is falling
        {
            state = MovementAnimations.falling;
        }

        animator.SetInteger("AnimationState", (int)state);
    }

    private bool JumpCheck() //this method handles only being able to jump once the player is grounded
    {
        //Box cast method needed to be used. dectecting a box collider works but lead to unwanted functionality in the 2d game
        return Physics2D.BoxCast(collision.bounds.center, collision.bounds.size, 0f, Vector2.down, .1f, Ground);
    }
}
