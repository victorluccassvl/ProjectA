using UnityEngine;

public class ControlledView : MonoBehaviour
{
    [SerializeField]
    private float _viewSpeed = 5f;

    [SerializeField]
    private Transform _target = null;

    private Vector2 _cumulatedRotationInput = Vector2.zero;

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _cumulatedRotationInput.x += Input.GetAxis("Rotate Camera Horizontal");
        _cumulatedRotationInput.y += -Input.GetAxis("Rotate Camera Frontal");
    }

    private void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        
        _cumulatedRotationInput.Normalize();

        transform.Rotate(transform.up, _cumulatedRotationInput.x * 5f);
        Camera.main.transform.RotateAround(_target.position, Camera.main.transform.right, _cumulatedRotationInput.y * 1f);

        _cumulatedRotationInput = Vector2.zero;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}