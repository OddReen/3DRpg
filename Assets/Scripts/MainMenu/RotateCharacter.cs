using UnityEngine;

public class RotateCharacter : MonoBehaviour
{
    [SerializeField] float speed;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + (Time.deltaTime * speed), 0);
    }
}