using RayFire;
using TMPro;
using UniRx;
using UnityEngine;

public class ObstacleFacade : MonoBehaviour
{
    public ReactiveProperty<int> health;
    public float _hitTime = 0.5f;
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private LayerMask _layerMask;
    private HitState _hitState;
    private RayfireRigid _rayfireRigid;
    
    
    private enum HitState
    {
        AWAKE,
        HIT
    }
    private void Awake()
    {
        health.SubscribeToText(_textMesh).AddTo(this);
        _rayfireRigid = GetComponent<RayfireRigid>();
    }


    private void FixedUpdate()
    {
        CheckRayCastAndTakeDamage();
    }

    private void CheckRayCastAndTakeDamage()
    {
        if (CheckRayCast())
        {
            if (_hitTime > 0)
            {
                _hitTime -= Time.deltaTime;
            }
            else if (_hitTime <= 0)
            {
                TakeDamage();
                _hitTime = 0.5f;
            }
        }
        else if (!CheckRayCast())
        {
            _hitTime =  0.5f; //ScriptableObject'e aktarılabilir
        }
    }

    void TakeDamage()
    {
        health.Value -= 1;

        if (health.Value<=0)
        {
            _rayfireRigid.Demolish();
        }
    }

    private bool CheckRayCast()=>_hitState == HitState.HIT & Physics.Raycast(transform.position,
        transform.forward,1 , _layerMask);
   


    private void OnTriggerEnter(Collider other)
    {
        _hitState = HitState.HIT;
    }

    
    


    
}