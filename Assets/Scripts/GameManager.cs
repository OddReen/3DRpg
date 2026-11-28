using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public GameObject destinationHighlight;
    [SerializeField] public GameObject hoverHighlight;
    [SerializeField] public GameObject rotationHighlight;

    [SerializeField] public List<Structure> structures;

    public GameObject player;
    public GameObject camera;

    [SerializeField, Range(-1.0f, 1.0f)] float cuttoutThreshold;

    private void Awake()
    {
        instance = this;

        structures.AddRange(FindObjectsByType<Structure>(FindObjectsSortMode.None));
    }

    private void Update()
    {
        for (int i = 0; i < structures.Count; i++)
        {
            Vector3 cameraForward = new Vector3(camera.transform.forward.x, 0.0f, camera.transform.forward.z).normalized;
            
            Vector3 playerToWall = ((structures[i].transform.position - player.transform.position)).normalized;

            structures[i].GetComponent<Structure>().SetVisual(Vector3.Dot(cameraForward, playerToWall) < cuttoutThreshold);
        }

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
