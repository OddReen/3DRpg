using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour, IPointerEnterHandler
{
    [Header("Buttons")]
    [SerializeField] Button _bNewGame;
    [SerializeField] Button _bLoadGame;
    [SerializeField] Button _bSettings;
    [SerializeField] Button _bQuit;

    [Header("Audio Clips")]
    [SerializeField] AudioClip _onHoverAudio;
    [SerializeField] AudioClip _onClickAudio;

    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _bNewGame.onClick.AddListener(OnNewGame);
        _bLoadGame.onClick.AddListener(OnLoadGame);
        _bSettings.onClick.AddListener(OnSettings);
        _bQuit.onClick.AddListener(OnQuit);
    }

    void OnNewGame()
    {
        SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
        _audioSource.PlayOneShot(_onClickAudio);
    }
    void OnLoadGame()
    {
        _audioSource.PlayOneShot(_onClickAudio);
    }
    void OnSettings()
    {
        _audioSource.PlayOneShot(_onClickAudio);
    }
    void OnQuit()
    {
        _audioSource.PlayOneShot(_onClickAudio);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSource.PlayOneShot(_onHoverAudio);
    }
}
