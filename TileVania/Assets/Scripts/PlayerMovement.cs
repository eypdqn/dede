using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    Vector2 moveInput;
    Vector2 climbInput;
    Rigidbody2D myRigidbody;
    Animator playerAnimator;
    [SerializeField] BoxCollider2D myFeetCollider2d;
    [SerializeField] CapsuleCollider2D myBodyCollider2d;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform Gun;
    bool isAlive = true;
    Vector2 deathKick = new Vector2(10f, 10f);
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) {
            return; }
        if (!myFeetCollider2d.IsTouchingLayers(LayerMask.GetMask("Ground"))){ return; }// Return means that if this condition doesn't match, don't go further in this method.

        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        
            Instantiate(Bullet, Gun.position, transform.rotation);
    }
    void Run()
    {
        Vector2 PlayerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = PlayerVelocity;

        //Runing animation code
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("IsRuning", playerHasHorizontalSpeed);

    }

    
    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1);
            
        }
        
    }

    void ClimbLadder()
    {
        
        if (!myFeetCollider2d.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = 2;
            playerAnimator.SetBool("IsClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * runSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("IsClimbing", playerHasVerticalSpeed);
    }

    

    void Die()
    {
        
        if (myBodyCollider2d.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {
            myRigidbody.velocity = deathKick;
             isAlive = false;
            playerAnimator.SetTrigger("Dying");
            myRigidbody.AddForce(Vector2.up * 10f);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        
    }
}
