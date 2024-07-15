using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator animator;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform respawnPoint;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpingPower = 15f;
    private float horizontal;
    private bool isFacingRight = true;
    private bool isRespawning;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        animator.SetTrigger("Respawn");
    }
    
    void Update()
    {
        if (isRespawning)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        // Update animator parameters
        animator.SetBool("isRunning", horizontal != 0);
        animator.SetFloat("verticalSpeed", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded());
    }
    
    private void FixedUpdate()
    {
        if (isRespawning) return;

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
    }
    

    public void Jump(InputAction.CallbackContext context)
    {
        if (isRespawning)
            return;

        if (context.performed && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        /*
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        //Nice on PC, results in problems for mobile */
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (isRespawning)
            return;

        Vector2 moveInput = context.ReadValue<Vector2>();
        horizontal = moveInput.x;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathZone"))
        {
            StartCoroutine(DespawnAndRespawn());
        }
        else if (other.CompareTag("Goal"))
        {
            StartCoroutine(GoalReached());
        }
    }

    private IEnumerator DespawnAndRespawn()
    {
        isRespawning = true;
        
        animator.SetTrigger("Despawn");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        transform.position = respawnPoint.position;
        
        animator.SetTrigger("Respawn");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        isRespawning = false;
    }
    
    private IEnumerator GoalReached()
    {
        isRespawning = true;
        
        animator.SetTrigger("Despawn");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int savedLevelIndex = PlayerPrefs.GetInt("LastLevel", 1);
        
        if (currentLevelIndex > savedLevelIndex)
        {
            PlayerPrefs.SetInt("LastLevel", currentLevelIndex);
        }
        
        SceneManager.LoadScene(currentLevelIndex + 1);
    }
}
