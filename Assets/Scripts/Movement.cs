using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
    private LayerMask _layersThatBlockMovement;
    [SerializeField]
    private float _distanceUnableToMoveTowards = 0.7f;
    [SerializeField]
    private float _toleranceAngleToBeConsideredVerticalWall = 0.5f;

    private Rigidbody _rigidbody = null;

    private float _moveSidewaysAxisInput;
    private float _moveAlignedAxisInput;
    private float _runAlignedAxisInput;
    private float _defaultMass;

    private float _cumulatedSidewaysDelocation = 0f;
    private float _cumulatedAlignedDelocation = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _defaultMass = _rigidbody.mass;
    }

    private void Update()
    {
        GetInputs();
        AcumulateDelocation();
    }

    private void FixedUpdate()
    {
        OperatePhysicsMovement(MovementPostValidation());
    }

    private void GetInputs()
    {
        _moveAlignedAxisInput  = Input.GetAxis("Move Aligned");
        _moveSidewaysAxisInput = Input.GetAxis("Move Sideways");
        _runAlignedAxisInput   = Input.GetAxis("Run Aligned");
    }

    private void AcumulateDelocation()
    {
        if (_moveAlignedAxisInput > 0f)
            _cumulatedAlignedDelocation += _moveAlignedAxisInput * (_forwardVirtualSpeed  + (_runExtraVirtualSpeed * _runAlignedAxisInput)) * Time.deltaTime;
        else                           
            _cumulatedAlignedDelocation += _moveAlignedAxisInput * _backwardVirtualSpeed * Time.deltaTime;

        _cumulatedSidewaysDelocation += _moveSidewaysAxisInput * _sidewayVirtualSpeed * Time.deltaTime;
    }

    private Vector3 MovementPostValidation()
    {
        Vector3 delocation = Vector3.zero;
        delocation += _cumulatedAlignedDelocation * transform.forward;
        delocation += _cumulatedSidewaysDelocation * transform.right;
        delocation *= _defaultMass / _rigidbody.mass;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, delocation, out hit, _distanceUnableToMoveTowards, _layersThatBlockMovement))
        {
            if (Vector3.Angle(Vector3.ProjectOnPlane(hit.normal, Vector3.up), hit.normal) < _toleranceAngleToBeConsideredVerticalWall)
                delocation = Vector3.zero;
        }

        return delocation;
    }

    private void OperatePhysicsMovement(Vector3 delocation)
    {
        _rigidbody.MovePosition(transform.position + delocation);

        _cumulatedAlignedDelocation = 0f;
        _cumulatedSidewaysDelocation = 0f;
    }
}