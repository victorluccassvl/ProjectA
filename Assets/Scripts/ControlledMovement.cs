using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControlledMovement : MonoBehaviour
{
    [SerializeField]
    private float _sidewayVirtualSpeed = 3f;
    [SerializeField]
    private float _forwardVirtualSpeed = 5f;
    [SerializeField]
    private float _backwardVirtualSpeed = 3f;
    [SerializeField]
    private float _runExtraVirtualSpeed = 4f;

    private Rigidbody _rigidbody = null;


    private float _moveSidewaysAxisInput;
    private float _moveAlignedAxisInput;
    private float _runAlignedAxisInput;


    private float _cumulatedSidewaysDelocation = 0f;
    private float _cumulatedAlignedDelocation = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInputs();
        AcumulateDelocation();
    }

    private void FixedUpdate()
    {
        OperatePhysicsMovement();
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

    private void OperatePhysicsMovement()
    {
        _rigidbody.MovePosition(transform.position + _cumulatedAlignedDelocation * transform.forward + _cumulatedSidewaysDelocation * transform.right);

        _cumulatedAlignedDelocation = 0f;
        _cumulatedSidewaysDelocation = 0f; 
    }
}