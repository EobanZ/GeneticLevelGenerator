using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private CharacterController2D characterController;
    private PlayerStats stats;
    private float horizontalMove = 0;
    private bool jump = false;
    private bool crouch = false;

    private SoundManager smInstance;
    public AudioSource sourceJmp;
    public AudioSource sourceDeath;

    public float runSpeed = 40f;



    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Setup sound
        smInstance = SoundManager.Instance;
        sourceJmp.clip = smInstance.playerJump;
        sourceDeath.clip = smInstance.playerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.stats.isDead()) 
            return;
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        crouch = Input.GetButton("Crouch");

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    
    }

    private void FixedUpdate()
    {
        characterController.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
       
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    public void OnBeginJump()
    {
        sourceJmp.Play();
    }

    public void OnKill()
    {
        
        sourceDeath.Play(); 
        stats.setDead(true);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("isJumping", false);
        animator.SetBool("IsDying", true);
    }

}
