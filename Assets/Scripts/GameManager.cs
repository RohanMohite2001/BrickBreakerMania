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

    [SerializeField] private Transform camera;
    [SerializeField] private Vector3 positionStrength;

    [SerializeField] private Transform startingPos;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject bricksSpace;
    public Ball ball;
    private Vector3 incrementTextOriginalPos;

    [SerializeField] private Image live1, live2, live3;
    
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

    private void Update()
    {
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
                break;
        }
    }

    private void NewGame()
    {
        live1.gameObject.SetActive(true);
        live2.gameObject.SetActive(true);
        live3.gameObject.SetActive(true);
        lives = 3;
        score = 0;
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
    }

    public void CameraShake()
    {
        camera.DOShakePosition(.3f, positionStrength);
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
