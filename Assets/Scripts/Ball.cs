using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 force;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float speed = 400f;
    private Vector2 startingPos;
    public Slider slider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startingPos = this.transform.position;
        StartLevel();
    }

    public void StartLevel()
    {
        transform.position = startingPos;
        speed = 400f;
        slider.transform.position = startingPos;
        force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;
        
        rb.AddForce(force.normalized * speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        speed += 10f;
        if (other.gameObject.CompareTag("Brick"))
        {
            Instantiate(explosionPrefab, other.transform.position, other.transform.rotation);
            other.gameObject.SetActive(false);
            GameManager.Instance.ShowScore(1);
        }
        
        if (other.gameObject.CompareTag("MissZone"))
        {
            GameManager.Instance.ShowLives(-1);
            if (GameManager.Instance.lives > 0)
            {
                Invoke("StartLevel", .2f);
            }else if (GameManager.Instance.lives <= 0)
            {
                Debug.Log("GameOver");
                gameObject.SetActive(false);
            }
        }
    }
}
