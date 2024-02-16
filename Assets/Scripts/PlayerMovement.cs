using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerStats stats;

    [Header("Animations")]
    [SerializeField] SquashAndStretch jumpingAnimation;
    [SerializeField] SquashAndStretch squashAnimation;


    [SerializeField] float maxSpeed;
    float timeWhenPressSpace;
    float remainingJumps;


    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Collider2D boxCollider;


    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sp;


    //bool lookRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        sp = GetComponentInChildren<SpriteRenderer>();

        remainingJumps = stats.onAirJump;

    }

    // Update is called once per frame
    void Update()
    {
        MovementProcess();
        JumpProcess();
        Gravity();
    }

    void MovementProcess()
    {
        //Movimiento de teclas del player
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
            movement.x += 1;

        if (Input.GetKey(KeyCode.A))
            movement.x -= 1;

        if (Input.GetKey(KeyCode.RightArrow))
            movement.x += 1;

        if (Input.GetKey(KeyCode.LeftArrow))
            movement.x -= 1;

        //Limitar la velocidad del player en horizontal
        //rb.velocity = movement.normalized * stats.maxGroundHorizontalSpeed;

        //Aceleración del personaje
        if (movement != Vector2.zero)
        {
            rb.velocity += movement * stats.groundAcceleration * Time.deltaTime;
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x / Mathf.Clamp(stats.groundFriction,1,5), rb.velocity.y);
        }

        //Máxima velocidad del personaje
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        //AnimacionVolteo(movement);
    }

    /*
    void AnimacionVolteo(Vector2 movement)
    {
        if ((lookRight == true && movement.x < 0) || (lookRight == false && movement.x > 0))
        {
            lookRight = !lookRight;
            //transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            sp.flipX = lookRight;
        }

    }
    */

    bool EstaEnSuelo()
    {
        /*
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center, 
            new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 
            0f, 
            Vector2.down, 
            0.2f,
            groundLayer
        );

        return raycastHit.collider != null;
        */

        RaycastHit2D raycastHit = Physics2D.BoxCast(
            groundCheckPoint.position,
            groundCheckPoint.localScale,
            0f,
            Vector2.down,
            0.2f,
            groundLayer
            );

        return raycastHit.collider != null;
    }


    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
            Gizmos.DrawWireCube(
                groundCheckPoint.position,
                groundCheckPoint.localScale
            );
    }
    

    void JumpProcess()
    {
        if (EstaEnSuelo())
        {
            remainingJumps = stats.onAirJump;
            //hasAppliedContinuosJumpForce = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
        {
            float initialJumpForce = 3;
            rb.velocity = new Vector2(rb.velocity.x, initialJumpForce);
            //rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);

            remainingJumps--;
            
            timeWhenPressSpace = 0.0f;
        }

        if (Input.GetKey(KeyCode.Space) && remainingJumps > 0)
        {
            timeWhenPressSpace += Time.deltaTime;

            //Limitar salto cuando presiona el espacio
            if (stats.maxJumpPressTime >= timeWhenPressSpace)
            {
                //Le doy una fuerza al salto
                rb.velocity = new Vector2(rb.velocity.x, stats.jumpStregth);
            }

        }
    }

    void Gravity()
    {
        if (rb.velocity.y > stats.yVelocityLowGravityThreshold)
        {
            rb.gravityScale = stats.defaultGravity;
            sp.color = Color.blue;
        }
        else if (rb.velocity.y < stats.yVelocityLowGravityThreshold && rb.velocity.y > -stats.yVelocityLowGravityThreshold)
        {
            rb.gravityScale = stats.lowGravity;
            sp.color = Color.yellow;
        }
        else
        {
            rb.gravityScale = stats.fallingGravity;
            sp.color = Color.green;
        }
    }
}
