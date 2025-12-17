using UnityEngine;

public class CamRig : MonoBehaviour
{
    private Transform _transform;
    public Transform target;
    public float speed = 3f;
    public Vector3 targetOffset = Vector3.zero;

    // 1. 턴이나 타일 선택을 기준오로 카메라의 기준점을 다시 설정합니다.
    // 2. 마우스 좌클릭을 통해서 각도를 변경합니다.
    // 3. 마우스 우클릭을 통해서 카메라 위치를 변경합니다.
    // 4. 마우스 휠을 통해서 카메라의 거리를 변경합니다.

    void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        if (target)
            _transform.position = Vector3.Lerp(_transform.position, target.position, speed * Time.deltaTime);
    }
}
