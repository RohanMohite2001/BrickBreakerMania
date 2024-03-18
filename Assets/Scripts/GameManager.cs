using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Vector3 startingReference;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    
    [SerializeField] private GameObject brickPrefab;
    private Vector3 instantiatePos;
    public Ball ball;
    
    public int score = 0;
    public int lives = 0;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;
        ShowLives(0);
        ShowScore(0);
        Instantiate_Bricks(5);
    }

    private void Instantiate_Bricks(int bricksNo)
    {
        for (int row = 0; row < bricksNo; row++)
        {
            for (int brick = 0; brick < bricksNo; brick++)
            {
                instantiatePos = startingReference + new Vector3(brick * 2.5f, 0, 0);
                Instantiate(brickPrefab, instantiatePos, Quaternion.identity);
            }

            startingReference += new Vector3(0, 1, 0);
        }
    }

    public void ShowScore(int score)
    {
        this.score += score;
        scoreText.text = "Score : " + this.score.ToString();
    }
    
    public void ShowLives(int lives)
    {
        this.lives += lives;
        livesText.text = "Lives : " + this.lives.ToString();
    }
}
