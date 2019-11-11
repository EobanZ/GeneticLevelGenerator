using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class FrogController : MonoBehaviour, IEnemy
{
    [Range(0,800)][SerializeField] private float m_JumpForce = 200f;
    [Range(1.0f, 5)] [SerializeField] private float m_RandomJumpTimeMin = 1;
    [Range(1.5f, 10)] [SerializeField] private float m_RandomJumpTimeMax = 5;
    [SerializeField] private Transform m_GroundCheck;   
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character                
    const float k_GroundedRadius = .1f;                                         // Radius of the overlap circle to determine if grounded        
    private Animator animator;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_isGrounded = true;
    [SerializeField]private int m_score = 100;
    private GameManager m_GameManager;
    private bool isDead = false;

    Coroutine jmpRoutine;
    Coroutine quackRoutine;

    //sound
    SoundManager smInstance;
    public AudioSource audioSourceQack;
    public AudioSource audioSourceEffects;
    AudioClip quackClip;
    AudioClip jumpClip;
    AudioClip dieClip;
    
  

    void Awake() 
    {
        animator = GetComponent<Animator>();
	    m_Rigidbody2D = GetComponent<Rigidbody2D>(); 
        m_GameManager = GameManager.Instance;

        //sound
        smInstance = SoundManager.Instance;
        quackClip = smInstance.frogQuack;
        audioSourceQack.clip = quackClip;
        jumpClip = smInstance.frogJump;
        dieClip = smInstance.frogDeath;
    }

    private void OnEnable()
    {
        jmpRoutine = StartCoroutine(Jump());
        quackRoutine = StartCoroutine(RandomQuack());
    }

    private void OnDisable()
    {
        StopCoroutine(jmpRoutine);
        StopCoroutine(quackRoutine);
    }

     IEnumerator RandomQuack()
    {
        while(true)
        {
            audioSourceQack.Play();
            yield return new WaitForSeconds(Random.Range(2, smInstance.quackIntervall));
        }
        
    }

    IEnumerator Jump()
    {
        while(true)
        {
            if (!m_isGrounded && m_Rigidbody2D.velocity.y <= 0)
                yield return null;

            m_Rigidbody2D.AddForce(new Vector2(0, m_JumpForce));
            audioSourceEffects.PlayOneShot(jumpClip);

            yield return new WaitForSeconds(Random.Range(m_RandomJumpTimeMin, m_RandomJumpTimeMax));
        }
        
    }

 
    private void FixedUpdate()
    {
        m_isGrounded = GroundCheck();

        if (m_isGrounded && m_Rigidbody2D.velocity.y <= 0)
            animator.SetBool("isJumping", false);
        else
            animator.SetBool("isJumping", true);
    }

    bool GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        return (colliders.Length > 0);
    }

    public void kill() 
    {
        StopCoroutine("Jump");
        StopCoroutine("RandomQuack");
        animator.SetBool("Dead", true);
        isDead = true;
        
        smInstance.GetComponents<AudioSource>()[1].PlayOneShot(dieClip);//because frog is destroyed too soon -> use second audosource on SoundManager
        Destroy(gameObject, .3f);
        
    }

    public int getKillScore()
    {
        return this.m_score;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || isDead)
            return;

        Vector2 direction = (collision.gameObject.transform.position - this.gameObject.transform.position);
        direction.Normalize();
        float dot = Vector2.Dot(direction, Vector2.up);
        
        if(dot > 0.1f) //1 direkt von oben 0 von der seite
        {
            collision.gameObject.GetComponent<PlayerStats>().addToScore(m_score);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 5.0f);
            kill();
        }
        else
        {
            collision.gameObject.GetComponent<CharacterController2D>().OnKillEvent.Invoke();
        }
     
    }
}
