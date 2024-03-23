using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 force;
    [SerializeField] private GameObject powerUpScale;
    [SerializeField] private GameObject powerUpBlock;
    [SerializeField] private GameObject star;
    [SerializeField] public float speed = 400f;
    [SerializeField] private Vector3 startingPos;

    public Slider slider;
    public bool inPlay = false;
    [SerializeField] private GameObject ballPos;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startingPos = transform.position;
        StartLevel();
    }

    public void StartLevel()
    {
        gameObject.SetActive(true);
        transform.position = startingPos;
        speed = 200f;
        slider.transform.position = slider.startingPos;
        slider.transform.localScale = slider.originalScale;
        force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;
        rb.AddForce(force.normalized * speed);
        
    }

    /*private void Update()
    {
        if (!inPlay)
        {
            transform.position = ballPos.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !inPlay)
        {
            inPlay = true;
            rb.AddForce(force.normalized * speed);
        }
    }*/

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MissZone"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.buzzer);

            inPlay = false;
            gameObject.SetActive(false);
            
            GameManager.Instance.CameraShake();
            GameManager.Instance.lives -= 1;
            GameManager.Instance.CheckLives();
            GameManager.Instance.CheckStars();
            if (GameManager.Instance.lives > 0)
            {
                Invoke(nameof(StartLevel), 1f);
            }
            else
            {
                GameManager.Instance.CheckLivesCount();                
            }
        }

        if (other.gameObject.CompareTag("Block"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.tap);
        }
    }
}
