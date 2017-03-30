using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;
    private int lives = 30;

    [SerializeField]
    private int mapHeight;
    [SerializeField]
    private int mapWidth;

    private bool paused = false;

    void Awake()
    {
        if (gameManager != null)
        {
            Debug.LogError("More Than One gameManager in Scene");
            return;
        }

        gameManager = this;
    }

    void Start ()
    {

    }
	
	void Update ()
    {
        if(lives <= 0)
        {
            //Load GameOver Screen
        }
	}

    public void DecreaseLives()
    {
        lives--;

    }

    public void SetLives(int _lives)
    {
        lives = _lives;
    }

    public int GetLives()
    {
        return lives;
    }

    public string GetLivesText()
    {
        return lives.ToString();
    }

    public void SetPause(bool _paused)
    {
        paused = _paused;
    }

    public bool GetPause()
    {
        return paused;
    }

    public int GetMapHeight()
    {
        return mapHeight;
    }

    public int GetMapWidth()
    {
        return mapWidth;
    }
}
