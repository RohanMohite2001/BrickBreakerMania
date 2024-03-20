using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 force;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject powerUpScale;
    [SerializeField] private GameObject powerUpBlock;
    [SerializeField] private GameObject star;
    [SerializeField] public float speed = 400f;
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private int scaleProbabilityTime = 10;
    [SerializeField] private int blockProbabilityTime = 80;

    [SerializeField] private ParticleSystem brickExplosion;
    public Slider slider;
    public bool inPlay;
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
        speed = 300f;
        slider.transform.position = slider.startingPos;
        slider.transform.localScale = slider.originalScale;
        force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;
        // rb.AddForce(force.normalized * speed);
        
    }

    private void Update()
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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            GameManager.Instance.brickCount++;
            //power-up
            int probability = Random.Range(1, 100);
            if (probability < scaleProbabilityTime)
            {
                Instantiate(powerUpScale, other.transform.position, other.transform.rotation);
            }
            if (probability > blockProbabilityTime)
            {
                Instantiate(powerUpBlock, other.transform.position, other.transform.rotation);
            }

            if (probability < 20 && probability > 10 && GameManager.Instance.stars < 3)
            {
                Instantiate(star, other.transform.position, other.transform.rotation);
            }
            
            //explosion effect
            brickExplosion.transform.position = other.transform.position;
            brickExplosion.gameObject.SetActive(true);
            brickExplosion.Play();
            
            //Brick sfx
            AudioManager.Instance.PlaySfx(AudioManager.Instance.tap);
            other.gameObject.SetActive(false);
        }
        
        if (other.gameObject.CompareTag("MissZone"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.buzzer);

            inPlay = false;
            gameObject.SetActive(false);
            
            GameManager.Instance.CameraShake();
            GameManager.Instance.lives -= 1;
            if (GameManager.Instance.lives > 0)
            {
                Invoke(nameof(StartLevel), 1f);
            }
            else if (GameManager.Instance.lives <= 0)
            {
                Debug.Log("GameOver");
                gameObject.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("Block"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.tap);
        }
    }
}
