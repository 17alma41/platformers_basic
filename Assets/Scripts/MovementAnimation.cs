using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sp;
    Rigidbody2D rb;
    //bool lookRight = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RunAnimation();
    }

    void RunAnimation()
    {
        Vector2 movement = Vector2.zero;
        movement.x = rb.velocity.x;

        if (movement != Vector2.zero)
        {
            animator.SetBool("isRunning", true);

        }
        else
        {
            animator.SetBool("isRunning", false);

        }

        if (movement.y < 0)
        {
            animator.SetBool("isJump", true);
        }

        //Volteo(movement);
    }

    /*
    void Volteo(Vector2 movement)
    {
        if ((lookRight == true && movement.x < 0) || (lookRight == false && movement.x > 0))
        {
            lookRight = !lookRight;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            //sp.flipX = lookRight;
        }

    }
    */
}
