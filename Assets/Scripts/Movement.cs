using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PhysicsProperties))]
public class Movement : MonoBehaviour
{
    [SerializeField]
    private float _sidewayVirtualSpeed = 3f;
    [SerializeField]
    private float _forwardVirtualSpeed = 5f;
    [SerializeField]
    private float _backwardVirtualSpeed = 3f;
    [SerializeField]
    private float _runExtraVirtualSpeed = 4f;
    [SerializeField]
    private float _jumpSpeed = 10f;

    [Space]
    [SerializeField]
    private Transform _groundChecker = null;
    [SerializeField]
    private float _groundDistance = 0.15f;
    [SerializeField]
    private LayerMask _groundMask;

    private CharacterController _characterController = null;
    private PhysicsProperties _physicsProperties = null;

    private float _verticalVelocity = 0f;

    private float _moveSidewaysAxisInput;
    private float _moveAlignedAxisInput;
    private float _runAlignedAxisInput;
    private bool _jumpInput;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _physicsProperties = GetComponent<PhysicsProperties>();
    }

    private void Update()
    {
        GetInputs();
        UpdateVerticalVelocity();
        Move(CalculateDelocation());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_groundChecker.position, _groundDistance);
    }

    private void GetInputs()
    {
        _moveAlignedAxisInput  = Input.GetAxis("Move Aligned");
        _moveSidewaysAxisInput = Input.GetAxis("Move Sideways");
        _runAlignedAxisInput   = Input.GetAxis("Run Aligned");
        _jumpInput             = Input.GetButtonDown("Jump");
    }

    private void UpdateVerticalVelocity()
    {
        bool isGrounded = Physics.CheckSphere(_groundChecker.position, _groundDistance, _groundMask);

        if (isGrounded && _verticalVelocity < 0f)
        {
            if (_jumpInput)
                _verticalVelocity = _jumpSpeed * _physicsProperties.MassRatio;
            else
                _verticalVelocity = 0f;
        }

        _verticalVelocity += Physics.gravity.y * Time.deltaTime;
    }

    private Vector3 CalculateDelocation()
    {
        Vector3 delocation = Vector3.zero;

        float alignedSpeed;
        float sidewaysSpeed;

        if (_moveAlignedAxisInput > 0f)
            alignedSpeed = _moveAlignedAxisInput * (_forwardVirtualSpeed + (_runExtraVirtualSpeed * _runAlignedAxisInput));
        else
            alignedSpeed = _moveAlignedAxisInput * _backwardVirtualSpeed;
        sidewaysSpeed = _moveSidewaysAxisInput * _sidewayVirtualSpeed;

        delocation += alignedSpeed * Time.deltaTime * transform.forward;
        delocation += sidewaysSpeed * Time.deltaTime * transform.right;
        delocation += _verticalVelocity * Time.deltaTime * Vector3.up;

        return delocation / _physicsProperties.MassRatio;
    }

    private void Move(Vector3 delocation)
    {
        if (delocation != Vector3.zero)
            _characterController.Move(delocation);
    }
}