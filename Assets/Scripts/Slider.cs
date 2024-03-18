using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Slider : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    [SerializeField] private float speed = 30f;
    private float maxBouncingAngle = 75f;
    public Vector2 startingPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startingPos = this.transform.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.zero;
        }
    }
                                                                                            
    private void FixedUpdate()
    {
        if (direction != Vector2.zero)
        {
            //rb.AddForce(direction * speed);
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            float contactPoint = transform.position.x - other.GetContact(0).point.x;
            float centre = other.otherCollider.bounds.size.x / 2;

            float currentAngle = Vector2.SignedAngle(Vector2.up, other.gameObject.GetComponent<Rigidbody2D>().velocity);
            float bounceAngle = (contactPoint / centre) * maxBouncingAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -maxBouncingAngle, maxBouncingAngle);
            
            Quaternion afterBounceAngle = Quaternion.AngleAxis(newAngle, Vector3.forward);
            other.gameObject.GetComponent<Rigidbody2D>().velocity = afterBounceAngle * Vector2.up *
                                                                   other.gameObject.GetComponent<Rigidbody2D>().velocity
                                                                       .magnitude;
        }
    }
}
