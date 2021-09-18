using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerController : Agent
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 newVelocity;
    private Animator anim;
    private CapsuleCollider2D cc;



    [Header("Movement params")]
    [SerializeField]
    [Range(0.0f,5.0f)]
    private float speed;

    [SerializeField]
    [Range(1, 10)]
    private int movementForce;

    [SerializeField]
    [Range(1, 5)]
    private int jumpForce;

    [Header("Target component")]
    [SerializeField]
    private Transform targetTransform;
    


    // Start is called before the first frame update
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider2D>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-8, 8), transform.localPosition.y, transform.localPosition.z);
        targetTransform.localPosition= new Vector3(Random.Range(-8, 8), transform.localPosition.y, transform.localPosition.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int movementAction = actions.DiscreteActions[0];
        Debug.Log(movementAction);
        switch(movementAction)
        {
            case 0:
                moveRight();
                break;
            case 1:
                moveLeft();
                break;
            default:
                break;
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        if(Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            moveRight();
        }
        else if(Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            moveLeft();
        }
    }

    public void moveRight()
    {
            anim.SetBool("isWalking", true);
            newVelocity.Set(movementForce * speed, rb.velocity.y);
            rb.velocity = newVelocity;


            sr.flipX = false;
            cc.offset = new Vector2(0.63f, cc.offset.y); 
    }

    public void moveLeft()
    {
        anim.SetBool("isWalking", true);
        newVelocity.Set(-movementForce * speed, rb.velocity.y);
        rb.velocity = newVelocity;
            
        sr.flipX = true;
        cc.offset = new Vector2(-0.63f, cc.offset.y);
     
    }

    public void jump(InputAction.CallbackContext context)
    {
            rb.AddForce(Vector2.up* jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jump");
    }

    void cancelMovement()
    {
        anim.SetBool("isWalking", false);
        newVelocity.Set(0, rb.velocity.y);
        rb.velocity = newVelocity;

        float newOffset = 0.3f;
        if (sr.flipX == true) newOffset *= -1;
        cc.offset = new Vector2(newOffset, cc.offset.y);

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {


        //This reward system is simply based on getting the coin or getting out of bounds
        if(collision.CompareTag("Coin"))
        {
            AddReward(+1f);
            EndEpisode();
        }
        else if (collision.CompareTag("Bound"))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    // Update is called once per frame
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
    }

}
