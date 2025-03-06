using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMissile : MonoBehaviour
{
    public Transform target;
    public float Speed;
    public float Acceleration;
    public float RotationControl;
    
    float MovY, MovX = 1;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject enemyCandidate = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyCandidate != null)
            target = enemyCandidate.transform;
        else
        {
            GameObject playerCandidate = GameObject.FindGameObjectWithTag("Player");
            if (playerCandidate != null)
                target = playerCandidate.transform;
        }
    }

    void Update()
    {
        MovY = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            GameObject enemyCandidate = GameObject.FindGameObjectWithTag("Enemy");
            if (enemyCandidate != null)
                target = enemyCandidate.transform;
            else
            {
                GameObject playerCandidate = GameObject.FindGameObjectWithTag("Player");
                if (playerCandidate != null)
                    target = playerCandidate.transform;
            }
        }
        
        Vector2 direction = (transform.position - target.position).normalized;
        float cross = Vector3.Cross(direction, transform.right).z;
        rb.angularVelocity = cross * RotationControl;
        Vector2 Vel = transform.right * (MovX * Acceleration);
        rb.AddForce(Vel);
        float Dir = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.right));
        float thrustForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.down)) * 2.0f;
        Vector2 relForce = Vector2.up * thrustForce;
        rb.AddForce(rb.GetRelativeVector(relForce));
        if (rb.velocity.magnitude > Speed)
            rb.velocity = rb.velocity.normalized * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth == null)
                enemyHealth = collision.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(250);
            Destroy(gameObject);
        }
    }
}
