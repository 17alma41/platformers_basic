using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float friction;
    [SerializeField] float maxJump;
    //bool hasAppliedContinuosJumpForce;

    [SerializeField] float gravedadPredeterminada;
    [SerializeField] float gravityUp;
    [SerializeField] float gravityFall;


    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;

    [SerializeField] Collider2D boxCollider;


    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sp;

    bool lookRight = true;

    float remainingJumps;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();

        remainingJumps = maxJump;

    }

    // Update is called once per frame
    void Update()
    {
        MovementProcess();
        JumpProcess();

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

        //Aceleración del personaje
        if (movement != Vector2.zero)
        {
            rb.velocity += movement * acceleration * UnityEngine.Time.deltaTime;
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x / Mathf.Clamp(friction,1,5), rb.velocity.y);
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
        float jumpStartedTime = 0;


        if (EstaEnSuelo())
        {
            remainingJumps = maxJump;
            //hasAppliedContinuosJumpForce = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
        {
            float initialJumpForce = 3;
            rb.velocity = new Vector2(rb.velocity.x, initialJumpForce);
            //rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);

            //Cuando pulso me cuenta tiempo
            jumpStartedTime = Time.time;

            remainingJumps--;

            //hasAppliedContinuosJumpForce = false;
        }

        if (Input.GetKey(KeyCode.Space) && remainingJumps > 0)
        {
            //float maxJumpPressTime = 0.2f;
            float jumpForce = 1;

            /*
            if (rb.velocity.y > 0)
            {
                rb.gravityScale = gravityUp;
                print("ha subido la gravedad: " + rb.gravityScale);
            }
            else
            {
                rb.gravityScale = gravityFall;
                print("ha bajado la gravedad: " + rb.gravityScale);

            }
            */

            //rb.velocity.y(Vector2.up * JumpForce);

            //Limitar salto cuando presiona el espacio
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            print("Jump Force: " + jumpForce);

        }
        else
        {
            //Restaurar la gravedad normal cuando no salta
            rb.gravityScale = gravedadPredeterminada;
        }
    }
}
