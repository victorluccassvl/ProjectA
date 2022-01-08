using UnityEngine;

public class PhysicsProperties : MonoBehaviour
{
    [SerializeField]
    private float _defaultMass = 80f;

    private float _currentMass;
    public float CurrentMass {
        get => _currentMass;
    }

    public float MassRatio {
        get => _currentMass / _defaultMass;
    }

    private void Awake()
    {
        _currentMass = _defaultMass;
    }
}
