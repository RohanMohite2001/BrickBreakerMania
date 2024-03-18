using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 force;
    [SerializeField] private float speed = 100f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;
        
        rb.AddForce(force * speed);
    }
}
