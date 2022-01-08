using UnityEngine;

public class Player : MonoBehaviour
{
    private AnimaEnergy _energy = null;
    private AegisShield _shield = null;
    private CandenceDeath _death = null;
    private SagacitasRadar _radar = null;
    private OccultoIndetectable _indetectable = null;

    private Movement _movement = null;
    private View _view = null;
    private Target _target = null;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        _energy       = GetComponentInChildren<AnimaEnergy>(true);
        _shield       = GetComponentInChildren<AegisShield>(true);
        _death        = GetComponentInChildren<CandenceDeath>(true);
        _radar        = GetComponentInChildren<SagacitasRadar>(true);
        _indetectable = GetComponentInChildren<OccultoIndetectable>(true);

        _movement = GetComponentInChildren<Movement>(true);
        _view     = GetComponentInChildren<View>(true);
        _target   = GetComponentInChildren<Target>(true);

        bool allComponentsSet = true;

        allComponentsSet &= _energy != null;
        allComponentsSet &= _shield != null;
        allComponentsSet &= _death != null;
        allComponentsSet &= _radar != null;
        allComponentsSet &= _indetectable != null;

        allComponentsSet &= _movement != null;
        allComponentsSet &= _view != null;
        allComponentsSet &= _target != null;

        this.enabled = allComponentsSet;

        if (!allComponentsSet)
        {
            print("The player has not initialized components.");
        }
    }
}
