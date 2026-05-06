using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] GameObject cursor;

    void Update()
    {
        Camera camera = Camera.main;
        cursor.transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
        
    }
}
