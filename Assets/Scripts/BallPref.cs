using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPref : MonoBehaviour
{
    [SerializeField] private float speed = 200f;
    public Vector2 force;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;
        rb.AddForce(force.normalized * speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MissZone"))
        {
            Destroy(gameObject);
        }
    }

}
