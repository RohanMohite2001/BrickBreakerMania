using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI increamentScoreText;


    [SerializeField] private Transform startingPos;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject bricksSpace;
    public Ball ball;
    private Vector3 incrementTextOriginalPos;
    
    public int score = 0;
    public int lives = 0;
    public int rows = 5;
    public int columns = 5;
    
    private void Awake()
    {
        incrementTextOriginalPos = increamentScoreText.rectTransform.position;
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
        Instantiate_Bricks(rows, columns);
    }

    private void Instantiate_Bricks(int rows, int columns)
    {
        float brickWidth = brickPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float brickHeight = brickPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        float brickSpaceBoundX = bricksSpace.GetComponent<SpriteRenderer>().bounds.size.x;
        float brickSpaceBoundY = bricksSpace.GetComponent<SpriteRenderer>().bounds.size.y;

        float rowGap = CalculateGap(brickSpaceBoundX, columns, brickWidth);
        float columnGap = CalculateGap(brickSpaceBoundY, rows, brickHeight);
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 instantiatePos = startingPos.position + new Vector3(col * (brickWidth + rowGap), 0, 0);

                if (WithinBricksSpace(instantiatePos))
                {
                    Instantiate(brickPrefab, instantiatePos, Quaternion.identity);
                }
            }
            startingPos.position += new Vector3(0, brickHeight + columnGap, 0);
        }
        
        // for (int row = 0; row < rows; row++)
        // {
        //     for (int column = 0; column < columns; column++)
        //     {
        //         instantiatePos = startingpos.position + new Vector3(column * 2.5f, 0, 0);
        //         Instantiate(brickPrefab, instantiatePos, Quaternion.identity);
        //     }
        //
        //     startingpos.position += new Vector3(0, 1, 0);
        // }
    }

    private float CalculateGap(float spaceBound, int count, float brickSize)
    {
        float totalBrickSize = count * brickSize;
        float remainingSpace = spaceBound - totalBrickSize;

        return remainingSpace / count;
    }

    public void IncreamentPoints()
    {
        increamentScoreText.rectTransform.DOScale(1, .3f).OnComplete(()=>
        {
            increamentScoreText.rectTransform.DOMove(scoreText.rectTransform.position, .8f).SetDelay(.5f)
                .SetEase(Ease.OutBack).OnComplete((() => increamentScoreText.rectTransform.DOScale(0, .3f)));
        });
        ShowScore(5);
        increamentScoreText.rectTransform.position = incrementTextOriginalPos;
    }
    
    private bool WithinBricksSpace(Vector3 position)
    {
        Bounds bound = bricksSpace.GetComponent<SpriteRenderer>().bounds;

        return bound.Contains(position);
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
