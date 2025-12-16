using UnityEngine;

public class CamRig : MonoBehaviour
{
    private Transform _transform;
    public Transform follow;
    public float speed = 3f;

    void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        if (follow)
            _transform.position = Vector3.Lerp(_transform.position, follow.position, speed * Time.deltaTime);
    }
}
