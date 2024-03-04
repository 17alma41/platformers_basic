using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStats stats;

    [Header("Animations")]
    [SerializeField] SquashAndStretch jumpingAnimation;
    [SerializeField] SquashAndStretch squashAnimation;
    [SerializeField] SquashAndStretch fallAnimation;

    [Header("Ground")]
    float timeWhenPressSpace;
    int remainingJumps;
    bool playerOnGround;
    bool wasOnGround = false;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;


    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sp;

    [Header("Color Particle")]
    [SerializeField] ParticleSystem particleColor;
    [SerializeField] ParticleSystem particleJump;
    [SerializeField] ParticleSystem particleFall;
    [SerializeField] ParticleSystem particleSqueak;

    [Header("Visuals")]
    [SerializeField] Gradient groundColor;
    [SerializeField] Gradient airColor;

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
        wasOnGround = playerOnGround;
        playerOnGround = EstaEnSuelo();

        MovementProcess();
        JumpProcess();
        Gravity();
        HandleGroundedEffects();

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

        //Aceleración y fricción del personaje
        if (playerOnGround)
        {
            if (movement != Vector2.zero)
            {
                rb.velocity += movement * stats.groundAcceleration * Time.deltaTime;
            }
            else
            {
                //Fricción en el suelo
                rb.velocity = new Vector2(rb.velocity.x / Mathf.Clamp(stats.groundFriction, 1, Mathf.Infinity), rb.velocity.y);
            }

            //Máxima velocidad del personaje horizontal en el suelo
            if (Mathf.Abs(rb.velocity.x) > stats.maxGroundHorizontalSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * stats.maxGroundHorizontalSpeed, rb.velocity.y);
            }
        }
        else if (!playerOnGround)
        {
            if (movement != Vector2.zero)
            {
                rb.velocity += movement * stats.airAcceleration * Time.deltaTime;
            }
            else
            {
                //Fricción en el aire
                rb.velocity = new Vector2(rb.velocity.x / Mathf.Clamp(stats.airFriction, 1, Mathf.Infinity), rb.velocity.y);
            }

            //Máxima velocidad del personaje horizontal en el aire
            if (Mathf.Abs(rb.velocity.x) > stats.maxAirHorizontalSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * stats.maxAirHorizontalSpeed, rb.velocity.y);
            }
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            squashAnimation.PlaySquashAndStretch();
            particleSqueak.Play();
        }

        //Limitar la velocidad de caída
        if (rb.velocity.y < -5)
        {
            rb.velocity = new Vector2(rb.velocity.x, -stats.maxAirSpeed);
        }
    }

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
    
    void HandleGroundedEffects()
    {
        if (playerOnGround && !wasOnGround)
        {
            sp.color = groundColor.Evaluate(1f);
            particleColor.startColor = Color.white;
            fallAnimation.PlaySquashAndStretch();
            particleFall.Play();
        }
        else if (playerOnGround && wasOnGround)
        {
            particleColor.startColor = Color.white;
        }
        else
        {
            float jumpRatio = 1f * remainingJumps / stats.onAirJump;
            sp.color = airColor.Evaluate(jumpRatio);
        }
    }


    void JumpProcess()
    {
        if (EstaEnSuelo())
        {
            remainingJumps = stats.onAirJump;
        }

        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0)
        {
            particleJump.Play();
            jumpingAnimation.PlaySquashAndStretch();
            float initialJumpForce = 3;
            rb.velocity = new Vector2(rb.velocity.x, initialJumpForce);
            //rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);

            remainingJumps--;

            timeWhenPressSpace = 0.0f;
        }

        if (Input.GetKeyUp(KeyCode.Space)) 
        {
            particleJump.Stop();
        }

        if (Input.GetKey(KeyCode.Space) && remainingJumps > 0)
        {
            timeWhenPressSpace += Time.deltaTime;
            jumpingAnimation.PlaySquashAndStretch();

            //Limitar salto cuando presiona el espacio
            if (stats.maxJumpPressTime >= timeWhenPressSpace)
            {

                //Le doy una fuerza al salto
                rb.velocity = new Vector2(rb.velocity.x, stats.jumpStregth);
            }
            else
            {
                particleJump.Stop();
            }
        }
    }

    void Gravity()
    {
        if (rb.velocity.y > stats.yVelocityLowGravityThreshold)
        {
            rb.gravityScale = stats.defaultGravity;
            particleColor.startColor = Color.white;

        }
        else if (rb.velocity.y < stats.yVelocityLowGravityThreshold && rb.velocity.y > -stats.yVelocityLowGravityThreshold)
        {
            rb.gravityScale = stats.lowGravity;
            particleColor.startColor = Color.yellow;
        }
        else
        {
            rb.gravityScale = stats.fallingGravity;
            particleColor.startColor = Color.green;
        }
    }
}
