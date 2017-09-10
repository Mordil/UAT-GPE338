using UnityEngine;

public class RotationAnimation : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 5.0f;
    [SerializeField]
    private Transform _myTransform;

    private void Awake()
    {
        if (_myTransform == null)
        {
            _myTransform = transform;
        }
    }

    private void Update()
    {
        float zValue = _myTransform.localEulerAngles.z + Time.deltaTime * _rotationSpeed;
        _myTransform.rotation = Quaternion.Euler(0, 0, zValue);
    }
}
