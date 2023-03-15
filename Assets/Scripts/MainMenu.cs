using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject buttonsAndTitle;
    public GameObject credits;
    public GameObject loadingScreen;
    public Slider progressBar;
    public Text percentage;

    private bool creds = false;

    public void PlayButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.lives = 3;
            GameManager.Instance.coins = 0;
            GameManager.Instance.world = 1;
            GameManager.Instance.stage = 1;
            GameManager.Instance.small = true;
            GameManager.Instance.big = false;
            GameManager.Instance.fire = false;
        }
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously() {
        buttonsAndTitle.SetActive(false);
        credits.SetActive(false);
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync("1-1");

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            progressBar.value = progress;
            percentage.text = progress * 100 + "%";

            yield return null;
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {
        creds = true;
        buttonsAndTitle.SetActive(false);
        credits.SetActive(true);
    }

    public void ExitCredits()
    {
        creds = false;
        buttonsAndTitle.SetActive(true);
        credits.SetActive(false);
    }

    private void Update()
    {
        if (creds && Input.GetKeyDown(KeyCode.Escape)) {
            ExitCredits();
        }
    }
}
