using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform FollowTarget;
    [SerializeField] private Vector3 PositionOffset;
    [SerializeField] private Vector3 RotationOffset;

    void Update()
    {
        transform.position = FollowTarget.position;
        transform.position += PositionOffset;
        transform.rotation = Quaternion.Euler(RotationOffset);
    }
}
