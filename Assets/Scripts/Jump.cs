using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    [SerializeField]
    private float _jumpForce = 50f;
    [SerializeField]
    private float _minimumDistanceFromGroundToAllowJumps = 0.5f;
    [SerializeField]
    private LayerMask _groundForJump;

    private Rigidbody _rigidbody = null;
    private Ray _ray;
    private bool _isTouchingGround = true;
    private bool _jumpInputGiven = false;

    private void Awake()
    {
        _ray.direction = Vector3.down;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _jumpInputGiven |= Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        _ray.origin = transform.position + Vector3.up * _minimumDistanceFromGroundToAllowJumps;
        _isTouchingGround = Physics.Raycast(_ray, out hit, 2 * _minimumDistanceFromGroundToAllowJumps, _groundForJump);

        if (_isTouchingGround && _jumpInputGiven && _rigidbody.velocity.y < 1f)
        {
            _rigidbody.AddForce(_jumpForce * Vector3.up, ForceMode.Impulse);
        }

        _jumpInputGiven = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.up * _minimumDistanceFromGroundToAllowJumps, transform.position + Vector3.down * _minimumDistanceFromGroundToAllowJumps);
    }
}
