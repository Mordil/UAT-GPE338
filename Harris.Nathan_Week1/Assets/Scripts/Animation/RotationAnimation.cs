using UnityEngine;

/// <summary>
/// MonoBehaviour that rotates the game object on the Z axis by the rotation speed assigned.
/// </summary>
public class RotationAnimation : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 5.0f;
    [SerializeField]
    private Transform _myTransform;

    private void Awake()
    {
        // check to see if we have a quick reference already - if not, get one
        if (_myTransform == null)
        {
            _myTransform = transform;
        }
    }

    private void Update()
    {
        // smooth out framerate with the rotation speed and add it to the current Z rotation
        float zValue = _myTransform.localEulerAngles.z + Time.deltaTime * _rotationSpeed;
        // assign the new rotation
        _myTransform.rotation = Quaternion.Euler(0, 0, zValue);
    }
}
