using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public GameObject hoverHighlight;
    public GameObject rotationHighlight;

    public GameObject camera;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (Time.timeScale <= 0.1f)
            {
                Time.timeScale = 1.0f;
            }
            else
            {
                Time.timeScale = 0.0f;
            }
        }
        else if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Time.timeScale = 1.0f;
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Time.timeScale = 3.0f;
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Time.timeScale = 6.0f;
        }
        else if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            Time.timeScale = .25f;
        }
    }
}
