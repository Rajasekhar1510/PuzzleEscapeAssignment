using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int maxHealth = 3;
    [HideInInspector] public int currentHealth;

    private bool gameEnded = false;

    [Header("UI References")]
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public Transform healthContainer;
    public GameObject heartPrefab;

    private List<GameObject> healthIcons = new List<GameObject>();

    private SnakeController[] allSnakes;
    private bool winCheckStarted = false;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (gameWinPanel != null)
            gameWinPanel.SetActive(false);

        GenerateHearts();

        allSnakes = FindObjectsByType<SnakeController>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        if (gameEnded)
            return;

        if (!winCheckStarted)
        {
            CheckForWin();
        }
    }


    void CheckForWin()
    {
        winCheckStarted = true;
        StartCoroutine(DelayedWinCheck());
    }

    private System.Collections.IEnumerator DelayedWinCheck()
    {
        yield return new WaitForSeconds(3f);  

        bool allMoving = true;

        foreach (SnakeController snake in allSnakes)
        {
            if (!snake.isMoving)
            {
                allMoving = false;
                break;
            }
        }

        if (allMoving)
        {
            GameWin();
        }
        else
        {
            winCheckStarted = false;
        }
    }


   
    void GameWin()
    {
        gameEnded = true;

        if (gameWinPanel != null)
        {
            gameWinPanel.SetActive(true);
        }
    }

    void GenerateHearts()
    {
        foreach (Transform child in healthContainer) Destroy(child.gameObject);
        healthIcons.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, healthContainer);
            healthIcons.Add(newHeart);
        }
    }

    public void TakeDamage()
    {
        if (gameEnded || currentHealth <= 0) return;

        currentHealth--;
        if (currentHealth < healthIcons.Count)
            healthIcons[currentHealth].SetActive(false);

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }


 
    void GameOver()
    {
        gameEnded = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
