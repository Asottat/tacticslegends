using UnityEngine;

public class UILoad : MonoBehaviour
{
    //TODO: Colocar um load mais condizente com o layout, está muito moderno
    private float _interval = 0.1f;
    private float _nextRotation = 0;

    void Update()
    {
        if (_nextRotation <= Time.time)
        {
            _nextRotation = Time.time + _interval;
            transform.Rotate(Vector3.back, 60f);
        }
    }
}
