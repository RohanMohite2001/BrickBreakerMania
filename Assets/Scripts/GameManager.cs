using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI increamentScoreText;

    [SerializeField] private Transform _camera;
    [SerializeField] private Vector3 positionStrength;

    [SerializeField] private Transform startingPos;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject bricksSpace;
    public Ball ball;
    private Vector3 incrementTextOriginalPos;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winningPanel;

    [SerializeField] private Image live1, live2, live3, star1, star2, star3;
    [SerializeField] private Image[] winningStars;
    
    public int stars;
    public int lives;
    public int rows = 5;
    public int columns = 5;
    public int brickCount;
    
    private void Awake()
    {
        incrementTextOriginalPos = increamentScoreText.rectTransform.position;
        Instance = this;
    }

    private void Start()
    {
        winningStars = new Image[winningStars.Length]; 
        NewGame();
    }

    private void Update()
    {
        if (brickCount == rows * columns)
        {
            Debug.Log("Winner");
            Win();
        }
        
        switch (lives)
        {
            case 3: 
                live1.gameObject.SetActive(true);
                live2.gameObject.SetActive(true);
                live3.gameObject.SetActive(true);
                break;
            case 2:
                live1.gameObject.SetActive(false);
                live2.gameObject.SetActive(true);
                live3.gameObject.SetActive(true);
                break;
            case 1:
                live1.gameObject.SetActive(false);
                live2.gameObject.SetActive(false);
                live3.gameObject.SetActive(true);
                break;
            case 0:
                live1.gameObject.SetActive(false);
                live2.gameObject.SetActive(false);
                live3.gameObject.SetActive(false);
                Debug.Log("Game Over");
                gameOverPanel.SetActive(true);
                break;
        }

        switch (stars)
        {
            case 1:
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(false);
                star3.gameObject.SetActive(false);
                break;
            case 2:
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(true);
                star3.gameObject.SetActive(false);
                break;
            case 3:
                star1.gameObject.SetActive(true);
                star2.gameObject.SetActive(true);
                star3.gameObject.SetActive(true);
                break;
        }
    }

    public void Restart()
    {
        gameOverPanel.SetActive(false);
        winningPanel.SetActive(false);
        NewGame();
        ball.inPlay = false;
        ball.StartLevel();
    }

    public void Win()
    {
        winningPanel.SetActive(true);
        // for (int i = 0; i < stars; i++)
        // {
        //     winningStars[0].gameObject.SetActive(true);
        // }
        ball.gameObject.SetActive(false);
    }

    private void NewGame()
    {
        live1.gameObject.SetActive(true);
        live2.gameObject.SetActive(true);
        live3.gameObject.SetActive(true);
        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);
        lives = 3;
        stars = 0;
        brickCount = 0;
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

        Vector3 startPos = startingPos.position;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 instantiatePos = startPos + new Vector3(col * (brickWidth + rowGap), 0, 0);

                if (WithinBricksSpace(instantiatePos))
                {
                    Instantiate(brickPrefab, instantiatePos, Quaternion.identity);
                }
            }
            startPos += new Vector3(0, brickHeight + columnGap, 0);
        }
    }

    public void CameraShake()
    {
        _camera.DOShakePosition(.3f, positionStrength);
    }

    private float CalculateGap(float spaceBound, int count, float brickSize)
    {
        float totalBrickSize = count * brickSize;
        float remainingSpace = spaceBound - totalBrickSize;

        return remainingSpace / count;
    }
    
    private bool WithinBricksSpace(Vector3 position)
    {
        Bounds bound = bricksSpace.GetComponent<SpriteRenderer>().bounds;

        return bound.Contains(position);
    }
}
