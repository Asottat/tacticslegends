using UnityEngine;

public class TempPresetRotation : MonoBehaviour
{
    public Vector3 RotationPivot = Vector3.left;

    void Update()
    {
        if (!WSCustumeTest.FreezeRotation)
            transform.Rotate(RotationPivot, 30 * Time.deltaTime);
    }
}
