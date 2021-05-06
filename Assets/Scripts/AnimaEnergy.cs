using UnityEngine;

// Anima defines the energy a player requires to keep itself alive and to use attacks
public class AnimaEnergy : MonoBehaviour
{
    public delegate void FullyDepletionDelegate();
    public event FullyDepletionDelegate OnFullyDepletion = null;

    [SerializeField]
    private float _startingAnima;

    private float _currentAnima;
    public float CurrentAnima
    {
        get
        {
            return _currentAnima;
        }
    }

    public void SubscribeToFullyDepletionEvent(FullyDepletionDelegate newDelegate)
    {
        OnFullyDepletion += newDelegate;
    }

    public void UnsubscribeToFullyDepletionEvent(FullyDepletionDelegate delegateToRemove)
    {
        OnFullyDepletion -= delegateToRemove;
    }

    public void DepleteAnima(float amount)
    {
        _currentAnima -= amount;

        if (CurrentAnima < 0) OnFullyDepletion?.Invoke();
    }
}
