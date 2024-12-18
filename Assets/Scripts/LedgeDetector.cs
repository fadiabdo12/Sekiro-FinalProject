using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public float raycastDistance = 1f;
    public float grabDistance = 0.5f;
    public float grabSpeed = 1f;
    [SerializeField] public LayerMask whatIsGround;
    private bool isGrabbing = false;
    private Vector3 grabPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, raycastDistance);
        if (hit.collider != null && hit.collider.IsTouchingLayers(whatIsGround))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrabbing = true;
                grabPosition = hit.point;
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;
            }
        }

        if (isGrabbing)
        {
            transform.position = Vector3.MoveTowards(transform.position, grabPosition, grabSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, grabPosition) < grabDistance)
            {
                isGrabbing = false;
                rb.gravityScale = 1f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.IsTouchingLayers(whatIsGround))
        {
            isGrabbing = true;
            grabPosition = collision.contacts[0].point;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.IsTouchingLayers(whatIsGround))
        {
            transform.position = collision.contacts[0].point;
            rb.velocity = Vector2.zero;
        }
    }
}
