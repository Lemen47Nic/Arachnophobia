using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{

    public Text text;

    void Awake()
    {
        text.text = "Level Reached: " + FindObjectOfType<GlobalVariables>().Level;
    }

    public void PlayGame()
    {
        FindObjectOfType<GlobalVariables>().Level = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
