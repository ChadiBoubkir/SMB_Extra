using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
            GameManager.Instance.lives = 3;
            GameManager.Instance.coins = 0;
        }else
        {
            SceneManager.LoadScene("1-1");
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
