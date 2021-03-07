using Events;
using PlayerBehaviors;
using PlayerState;
using UniRx;
using UnityEngine;
using Zenject;

public class FinalState : IState
{
    private readonly SignalBus _signalBus;
    private readonly PlayerObservables _observables;
    private Player _player;
    private bool _alreadyHit;
    public FinalState(SignalBus signalBus, PlayerObservables observables, Player player)
    {
        _signalBus = signalBus;
        _observables = observables;
        _player = player;
    }


    public void EnterState()
    {
       //_player.RigidBody.constraints = RigidbodyConstraints.FreezeAll;
       //_player.SplineFollower.enabled = false;
        _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("FinalIDLE", 0));

        _observables.InputObservable.Subscribe(x =>
        {
            if (!_alreadyHit)
            {
                _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("FINALPUNCH", 0));
                _signalBus.Fire(new SignalPunch());
                _alreadyHit = true;
            }
           
        });

    }

    public void ExitState()
    {
        Debug.Log("FınalExitr");
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }
}
