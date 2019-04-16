using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public float restartDelay = 2f;

    public Text scoreText;
    public GameObject winGamePanel;
    public GameObject loseGamePanel;
    GlobalVariables globals;
    Player player;

    float winAfterSeconds;

    void Start()
    {
        globals = FindObjectOfType<GlobalVariables>();
        player = FindObjectOfType<Player>();
        winAfterSeconds = globals.Level * 20;
        if (winAfterSeconds > 60)
            winAfterSeconds = 60;
        Invoke("WinGame", winAfterSeconds);
    }

    void Update()
    {
        scoreText.text = player.totalBullets + "/" + player.maxBullets + "\n" + "LV: " + globals.Level;
    }

    public void WinGame()
    {
        if (!globals.Ended)
        {
            StopGame();

            winGamePanel.SetActive(true);

            Invoke("Restart", restartDelay);
        }
    }

    public void EndGame()
    {
        if (!globals.Ended)
        {
            StopGame();

            loseGamePanel.SetActive(true);

            Invoke("ToMenu", restartDelay);
        }
    }

    void StopGame()
    {
        globals.Ended = true;
        globals.ZeroCharacters();
        SharedKnowledge.SharedInstance.ResetInstance();
        player.enabled = false;
    }

    void Restart()
    {
        globals.Level++;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Menu");
    }

    public void ImHere() { FindObjectOfType<GlobalVariables>().ImHere(); }
    public bool isEnded() { return globals.Ended; }
}
