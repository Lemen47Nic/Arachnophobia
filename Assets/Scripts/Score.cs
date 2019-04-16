using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public Transform player;

    public Text scoreText;

    GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isEnded())
            scoreText.text = player.position.z.ToString("0");
    }
}
