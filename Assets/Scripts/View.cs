using UnityEngine;

[RequireComponent(typeof(Camera))]
public class View : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;
    [SerializeField]
    private float _targetExpectedHeight = 2f;
    [SerializeField]
    private float _maxDistanceFromTarget = 3f;
    [SerializeField]
    private float _viewVirtualAngularSpeed = 200f;
    [SerializeField]
    private float _maxCameraAlignedMovementFreedom = 160f;
    [SerializeField]
    private LayerMask _layersThatRepelCamera;
    [SerializeField]
    private float _cameraClippingCorrection = 0.05f;

    private float _rotateCameraAlignedInput;
    private float _rotateCameraSidewaysInput;
    private Vector3 _targetRealPosition;


    private void OnEnable()
    {
        if (_target == null)
        {
            this.enabled = false;
            return;
        }

        UpdateCursorStatus(true);
    }

    private void Update()
    {
        GetRefinedInputs();
        UpdateCameraPosition();
        RotateCameraAroundTargetSideways();
        RotateCameraAroundTargetAligned();
        UpdateCameraLookAt();
    }

    private void OnDisable()
    {
        UpdateCursorStatus(false);
    }

    private void OnDrawGizmos()
    {
        DrawCameraTarget();
    }

    private void UpdateCameraLookAt()
    {
        transform.forward = (_targetRealPosition - transform.position).normalized;
    }

    private void RotateCameraAroundTargetAligned()
    {
        Quaternion rotationBackup = transform.rotation;
        Vector3 positionBackup = transform.position;

        transform.RotateAround(_targetRealPosition, transform.right, -_rotateCameraAlignedInput);

        // If rotation extrapolates defined values, revert modifications
        float currentAngle = Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(transform.forward, Vector3.up));
        if (currentAngle > _maxCameraAlignedMovementFreedom / 2f)
        {
            transform.rotation = rotationBackup;
            transform.position = positionBackup;
        }
    }

    private void RotateCameraAroundTargetSideways()
    {
        transform.RotateAround(_targetRealPosition, Vector3.up, _rotateCameraSidewaysInput);

        // May need to change in the future if the player interacts with slopes
        _target.forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
    }

    private void UpdateCameraPosition()
    {
        _targetRealPosition = _target.position + _targetExpectedHeight * Vector3.up;

        RaycastHit hit;
        bool hasHit = Physics.Raycast(_targetRealPosition, -transform.forward, out hit, _maxDistanceFromTarget, _layersThatRepelCamera);

        float distance = (hasHit)? Vector3.Distance(hit.point, _targetRealPosition) - _cameraClippingCorrection : _maxDistanceFromTarget;

        transform.position = _targetRealPosition + (-transform.forward) * distance;
    }

    private void GetRefinedInputs()
    {
        Vector2 input;
        input.x = Input.GetAxis("Rotate Camera Aligned");
        input.y = Input.GetAxis("Rotate Camera Sideways");

        input.Normalize();

        input *= _viewVirtualAngularSpeed * Time.deltaTime;

        _rotateCameraAlignedInput = input.x;
        _rotateCameraSidewaysInput = input.y;
    }

    private void DrawCameraTarget()
    {
        Vector3 realTarget = _target.position + _targetExpectedHeight * Vector3.up;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_target.position, realTarget);
    }

    private void UpdateCursorStatus(bool enable)
    {
        if (enable)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}