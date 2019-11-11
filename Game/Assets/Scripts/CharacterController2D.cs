
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                              // A mask determining what is ground to the character
    [SerializeField] private LayerMask m_WhatIsDeadlyGround;
    [SerializeField] private LayerMask m_WhatIsEnemy; 
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
    private PlayerStats stats;
    const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    const float k_deadlyRadius = .01f;
    const float k_EnemyRadius = .01f;
    const float k_EnemyRadiusCelling = .1f;
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;
 
    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public UnityEvent OnKillEvent;
    public UnityEvent OnDividerEnterEvent;
    public UnityEvent OnDividerExitEvent;
    public UnityEvent OnBeginJumpEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

  
    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();

        if (OnKillEvent == null)
            OnKillEvent = new UnityEvent();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("DeadlyGround"))
        {
            OnKillEvent.Invoke();
        }
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("divider"))
            OnDividerEnterEvent.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("divider"))
            OnDividerExitEvent.Invoke();
    }



    private void FixedUpdate()
    {
 

        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded && m_Rigidbody2D.velocity.y < 0)
                    OnLandEvent.Invoke();
            }
        }


        //Mit dem OnCollision Event besser/performanter + mehr kontrolle mit dem dot product
        /*
        colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_deadlyRadius, m_WhatIsDeadlyGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject) {
                OnKillEvent.Invoke();
                Destroy(m_Rigidbody2D);
                this.stats.setDead(); // disable Movement
            }
        }

        //destroes Enemies jumped on
        colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_EnemyRadius, m_WhatIsEnemy);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                Component[] enemyScripts = colliders[i].gameObject.GetComponents(typeof(IEnemy));
                if (enemyScripts.Length == 1)
                    (enemyScripts[0] as IEnemy).kill();
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
        colliders = Physics2D.OverlapCircleAll(m_CeilingCheck.position, k_EnemyRadiusCelling, m_WhatIsEnemy);
        for (int i = 0; i < colliders.Length; i++) {
            Debug.Log("killed from:" + colliders[i].gameObject.ToString());
            if (colliders[i].gameObject != gameObject) {
                OnKillEvent.Invoke();
                Destroy(m_Rigidbody2D);
                this.stats.setDead(); // disable Movement
            }
        }
        */

    }


    public void Move(float move, bool crouch, bool jump)
    {
        if (this.stats.isDead())
            return;
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

            OnBeginJumpEvent.Invoke();
        }

    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
