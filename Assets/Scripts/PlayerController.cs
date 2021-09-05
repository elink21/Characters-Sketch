using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 newVelocity;

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

    }

    public void moveRight()
    {
        Debug.Log("Right");
        newVelocity.Set(movementForce* speed, rb.velocity.y);
        rb.velocity = newVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
