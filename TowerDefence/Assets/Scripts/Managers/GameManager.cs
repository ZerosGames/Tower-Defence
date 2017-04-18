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

    public GameObject LoadingScreen;

    public enum GameState
    {
        Loading,
        Starting,
        Playing,
        Pasued,
        Win,
        Lose,
        Stopping
    }

    public GameState gameState = new GameState();

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
        gameState = GameState.Loading;
    }
	
	void Update ()
    {
        switch (gameState)
        {
            case GameState.Loading:
                LoadContent();
                break;
            case GameState.Starting:
                StartInit();
                break;
            case GameState.Playing:
                Playing();
                break;
            case GameState.Win:
                Win();
                break;
            case GameState.Lose:
                Lose();
                break;
            case GameState.Stopping:
                break;
            default:
                break;
        }
    }

    private void LoadContent()
    {
        gameState = GameState.Starting;
    }

    private void StartInit()
    {
        References.Refs.InitReferences();

        References.Refs.placeController.InitData();
        References.Refs.camController.InitData();
        References.Refs.playerData.InitData();

        ObjectPoolManager.instance.InitPools();

        //Generates Map\\
        References.Refs.world.InitData();
        References.Refs.mapGenerator.InitData();

        gameState = GameState.Playing;
    }

    private void Playing()
    {
        LoadingScreen.SetActive(false);

        if (lives <= 0)
        {
            gameState = GameState.Lose;
        }
    }

    private void Win()
    {
        gameState = GameState.Stopping;
    }

    private void Lose()
    {
        gameState = GameState.Stopping;
    }

    private void Stopping()
    {

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

    public void ChangeState(GameState _GameState)
    {
        gameState = _GameState;
    }
}
