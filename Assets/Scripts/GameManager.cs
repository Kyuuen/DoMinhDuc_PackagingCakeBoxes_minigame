using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int gridSize = 3; 
    public GameObject cakePrefab; 
    public GameObject boxPrefab; 
    public GameObject gridFramePrefab;
    public GameObject candyPrefab;
    public Transform gridParent;
    public Vector2Int[] candiesPos;
    public Text timeText;

    [SerializeField] Vector2Int cakePosition;
    [SerializeField] Vector2Int boxPosition;
    [SerializeField] GameObject wonUI;
    [SerializeField] GameObject loseUI;
    [SerializeField] GameObject gridPos;
    private GameObject cake;
    private GameObject box;
    private bool gameWon = false;
    private bool gameOver = false;
    private float countDown;

    public static int currentLevel = 1;
    public static int currentWonLevel = 0;


    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 currentSwipe;

    public float minSwipeLength = 1f;

    void Start()
    {
        gridPos.transform.localPosition -= new Vector3(gridSize - 1, gridSize - 1, 0);
        InitializeGame();

        countDown = 45f;
        countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);
        timeText.text = string.Format("{00:00}", countDown);

    }

    void InitializeGame()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject gridFrame = Instantiate(gridFramePrefab, gridParent);
                gridFrame.transform.localPosition = new Vector3(x * 2, y * 2, 0);
            }
        }

        foreach (Vector2Int candyPos in candiesPos)
        {
            GameObject candy = Instantiate(candyPrefab, gridParent);
            candy.transform.localPosition = new Vector3(candyPos.x * 2, candyPos.y * 2, 0); ;
        }

        cake = Instantiate(cakePrefab, gridParent);
        box = Instantiate(boxPrefab, gridParent);

        UpdateGrid();

        
    }

    void Update()
    {
        if(countDown <= 0 && !gameOver)
        {
            gameOver = true;
            loseUI.SetActive(true);
            return;
        }

        if(gameOver || gameWon)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    currentSwipe = endTouchPosition - startTouchPosition;

                    if (currentSwipe.magnitude > minSwipeLength)
                    {
                        currentSwipe.Normalize();

                        if (Mathf.Abs(currentSwipe.x) > Mathf.Abs(currentSwipe.y))
                        {
                            if (currentSwipe.x > 0)
                            {
                                MoveCake(Vector2Int.right);
                                MoveBox(Vector2Int.right);
                            }
                            else
                            {
                                MoveCake(Vector2Int.left);
                                MoveBox(Vector2Int.left);
                            }
                        }
                        else
                        {
                            if (currentSwipe.y > 0)
                            {
                                MoveCake(Vector2Int.up);
                                MoveBox(Vector2Int.up);
                            }
                            else
                            {
                                MoveCake(Vector2Int.down);
                                MoveBox(Vector2Int.down);
                            }
                        }
                    }
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCake(Vector2Int.up);
            MoveBox(Vector2Int.up);
        }
           
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCake(Vector2Int.down);
            MoveBox(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCake(Vector2Int.left);
            MoveBox(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCake(Vector2Int.right);
            MoveBox(Vector2Int.right);
        }

        countDown -= Time.deltaTime;
        countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);
        timeText.text = string.Format("{00:00}", countDown);
    }

    void MoveCake(Vector2Int direction)
    {
        while (true)
        {
            Vector2Int cakeTargetPosition = cakePosition + direction;
            foreach (Vector2Int candyPos in candiesPos)
            {
                if (candyPos == cakeTargetPosition) return;
            }

            if (cakeTargetPosition.x < 0 || cakeTargetPosition.x >= gridSize || cakeTargetPosition.y < 0 || cakeTargetPosition.y >= gridSize)
                return;

            if (cakeTargetPosition == boxPosition)
            {
                if (direction == Vector2Int.down && cakePosition.x == boxPosition.x && cakePosition.y == boxPosition.y + 1)
                {
                    cakePosition = boxPosition;
                    LevelWin();
                    UpdateGrid();
                    gameWon = true;
                    currentWonLevel = currentLevel;
                    currentLevel++;
                    return;
                }
                else return;
            }
            cakePosition = cakeTargetPosition;
            UpdateGrid();
        }
    }

    void MoveBox(Vector2Int direction)
    {
        while (true)
        {
            Vector2Int boxTargetPosition = boxPosition + direction;
            foreach (Vector2Int candyPos in candiesPos)
            {
                if (candyPos == boxTargetPosition) return;
            }
            if (boxTargetPosition.x < 0 || boxTargetPosition.x >= gridSize || boxTargetPosition.y < 0 || boxTargetPosition.y >= gridSize)
                return;

            if (boxTargetPosition == cakePosition)
            {
                if (direction == Vector2Int.up && cakePosition.x == boxPosition.x && cakePosition.y == boxPosition.y + 1)
                {
                    boxPosition = cakePosition;
                    LevelWin();
                    UpdateGrid();
                    gameWon = true;
                    currentWonLevel = currentLevel;
                    currentLevel++;
                    return;

                }
                else return;
            }
            boxPosition = boxTargetPosition;
            UpdateGrid();
        }
    }

    void LevelWin()
    {
        gameWon = true;
        Destroy(cake);
        wonUI.SetActive(true);
    }

    void UpdateGrid()
    {
        cake.transform.localPosition = new Vector3(cakePosition.x * 2, cakePosition.y * 2, 0);
        box.transform.localPosition = new Vector3(boxPosition.x * 2, boxPosition.y * 2, 0);
    }

    public void Reset()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void Next()
    {
        string levelName = "Level" + currentLevel;
        SceneManager.LoadScene(levelName);
    }
}