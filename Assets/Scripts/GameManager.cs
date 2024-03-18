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
    private int score = 0;
    [SerializeField] private GameObject brickPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instantiate_Bricks(5);
    }

    private void Instantiate_Bricks(int bricksNo)
    {
        for (int row = 0; row < bricksNo; row++)
        {
            for (int brick = 0; brick < bricksNo; brick++)
            {
                Instantiate(brickPrefab, startingReference + new Vector3(bricksNo,0,0), Quaternion.identity);
            }
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
    }
}
