using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Target : MonoBehaviour
{
    [SerializeField]
    private LayerMask _targetableLayers;
    [SerializeField]
    private float _maxTargetDistance = 100f;

    private Camera _camera = null;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public bool TryGetTargetHit(out RaycastHit targetHit)
    {
        Vector3 screenPosition = new Vector3(_camera.pixelWidth - 1, _camera.pixelHeight - 1, 0f) / 2;
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        return Physics.Raycast(ray, out targetHit, _maxTargetDistance, _targetableLayers);
    }
}
