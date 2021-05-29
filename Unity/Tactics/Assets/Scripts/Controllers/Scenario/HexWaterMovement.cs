using UnityEngine;

public class HexWaterMovement : MonoBehaviour
{
    public Transform TransformToMove;

    float _maxY = 0.12f;
    float _maxYForLerp = 0.19f;
    float _speed = 1f;
    float _minSpeed = 0.25f;
    float _maxSpeed = 0.5f;
    Vector3 _targetPosition;
    float _direction = 1f;

    void Start()
    {
        if (Random.Range(0, 2) > 0)
            _direction *= -1;
        ReverseDirection();
    }

    void Update()
    {
        TransformToMove.localPosition = Vector3.Lerp(TransformToMove.localPosition, _targetPosition, _speed * Time.deltaTime);

        if ((_direction == 1f && TransformToMove.localPosition.y >= _maxY) || (_direction == -1f && TransformToMove.localPosition.y <= -_maxY))
        {
            ReverseDirection();
        }
    }

    void ReverseDirection()
    {
        _speed = Random.Range(_minSpeed, _maxSpeed);

        _direction *= -1;
        _targetPosition = new Vector3(TransformToMove.localPosition.x, _maxYForLerp * _direction, TransformToMove.localPosition.z);
    }
}
