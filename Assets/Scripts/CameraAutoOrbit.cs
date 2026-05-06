using UnityEngine;

public class CameraAutoOrbit : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float orbitSpeed = 20f;

    void Update()
    {
        if (target == null) return;
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}