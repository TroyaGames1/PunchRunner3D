using Events;
using UnityEngine;
using Zenject;

public class TraumaInducer : MonoBehaviour 
{
 
    public float MaximumStress = 0.6f;
    public float Range = 45;
    private StressReceiver _target;
    [Inject] private SignalBus _signalBus;

    private void Awake()
    {
        _target = FindObjectOfType<StressReceiver>();
        _signalBus.Subscribe<SignalDoShake>(Shake);
    }

    private void Shake()
    {
         
        float distance = Vector3.Distance(transform.position, _target.transform.position);
        float distance01 = Mathf.Clamp01(distance / Range);
        float stress = (1 - Mathf.Pow(distance01, 2)) * MaximumStress;
        _target.InduceStress(stress);
    }
}