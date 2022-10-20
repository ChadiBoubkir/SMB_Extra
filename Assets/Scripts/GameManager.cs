using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world;
    public int stage;
    public int lives;
    public int coins;
    public Canvas canvas;
    public Text livesText;
    public Text coinsText;
    public Text levelText;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        livesText.text = " Lives:\nX" + lives.ToString();
        coinsText.text = " Coins:\nX" + coins.ToString();
        levelText.text = " Level:\n" + world.ToString() + "-" + stage.ToString();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        NewGame();
    }

    public void NewGame()
    {
        lives = 3;
        coins = 0;

        LoadLevel(1, 1);
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel()
    {
        if (world <= 8 && world > 0)
        {
            if (stage < 4)
            {
                LoadLevel(world, stage + 1);
            }
            else
            {
                LoadLevel(world + 1, 1);
            }
        }
    }

    public void ResetLevel(float delay)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            Invoke(nameof(ResetLevel), delay);
        }
    }

    public void ResetLevel()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            lives--;

            if (lives > 0)
            {
                LoadLevel(world, stage);
            }
            else
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            Invoke(nameof(NewGame), 3f);
        }
    }

    public void AddCoin()
    {
        coins++;

        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
    }
}