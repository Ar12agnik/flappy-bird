using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public int player_score;
    public Text scoreText;
    public GameObject gameoverscreen;
    public void addScore()
    {
        player_score++;
        scoreText.text = player_score.ToString();
    }
    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void gameover()
    {
        gameoverscreen.SetActive(true);
    }

}
