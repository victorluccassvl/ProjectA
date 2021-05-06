using UnityEngine;

// Sagacitas is a radar, represented as a minimap on the HUD.
public class SagacitasRadar : MonoBehaviour
{
    private const float CLIP_DISTANCE = 100f;

    [SerializeField]
    private Transform _target = null;
    [SerializeField]
    private GameObject _radarPrefab = null;

    [SerializeField]
    private float _maxCameraViewSize = 25;
    [SerializeField]
    private float _minCameraViewSize = 3f;
    [SerializeField]
    private float _zoomFactor = 0.3f;

    private GameObject _radar = null;
    private Camera _camera = null;

    private void Awake()
    {
        InitializeRadar();
    }

    private void OnEnable()
    {
        _radar.SetActive(true);

    }

    private void Update()
    {
        UpdateRadarTransform();
    }

    private void OnDisable()
    {
        _radar.SetActive(false);
    }

    private void UpdateRadarTransform()
    {
        Vector3 newPosition = _target.position;
        newPosition.y = CLIP_DISTANCE;

        _radar.transform.position = newPosition;
    }

    private void InitializeRadar()
    {
        if (_radarPrefab != null)
        {
            _radar = Instantiate(_radarPrefab, transform);
            _camera = _radar.GetComponentInChildren<Camera>();
            _camera.orthographicSize = Mathf.Lerp(_minCameraViewSize, _maxCameraViewSize, _zoomFactor);
        }
    }
}
