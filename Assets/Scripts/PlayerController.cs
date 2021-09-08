using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 newVelocity;
    private Animator anim;
    private BoxCollider2D bc;

    [Header("Movement params")]
    [SerializeField]
    [Range(0.0f,5.0f)]
    private float speed;

    [SerializeField]
    [Range(1, 10)]
    private int movementForce;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
    }

    public void moveRight(InputAction.CallbackContext context )
    {
        if(context.performed)
        {
            Debug.Log("Move Right");
            anim.SetBool("isWalking", true);
            newVelocity.Set(movementForce * speed, rb.velocity.y);
            rb.velocity = newVelocity;
            sr.flipX = false;
            bc.offset = new Vector2(Mathf.Abs(bc.offset.x), bc.offset.y);
        }
        else if (context.canceled)
        {
            cancelMovement();
        }
       
    }

    public void moveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetBool("isWalking", true);
            newVelocity.Set(-movementForce * speed, rb.velocity.y);
            rb.velocity = newVelocity;
            sr.flipX = true;
            bc.offset = new Vector2(-bc.offset.x, bc.offset.y);
        }
        else if (context.canceled)
        {
            cancelMovement();
        }
    }

    void cancelMovement()
    {
        anim.SetBool("isWalking", false);
        newVelocity.Set(0, rb.velocity.y);
        rb.velocity = newVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
