using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _bNewGame;
    [SerializeField] Button _bLoadGame;
    [SerializeField] Button _bSettings;
    [SerializeField] Button _bQuit;

    void Awake()
    {
        _bNewGame.onClick.AddListener(OnNewGame);
        _bLoadGame.onClick.AddListener(OnLoadGame);
        _bSettings.onClick.AddListener(OnSettings);
        _bQuit.onClick.AddListener(OnQuit);
    }

    void OnNewGame()
    {
        SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
        Debug.Log("New Game");
    }
    void OnLoadGame()
    {
        Debug.Log("Load Game");
    }
    void OnSettings()
    {
        Debug.Log("Settings");
    }
    void OnQuit()
    {
        Debug.Log("Quit");
    }
}
