using DG.Tweening;
using Events;
using Miscs;
using PlayerBehaviors;
using PlayerState;
using UniRx;
using UnityEngine;
using Zenject;

public class FinalState : IState
{
    private readonly SignalBus _signalBus;
    private readonly PlayerObservables _observables;
    private readonly UIManager _uıManager;
    private readonly Player _player;
    private Tween _tween;
    private bool _alreadyHit;
    public FinalState(SignalBus signalBus, PlayerObservables observables, Player player, UIManager uıManager)
    {
        _signalBus = signalBus;
        _observables = observables;
        _player = player;
        _uıManager = uıManager;
    }


    public void EnterState()
    {
       _player.RigidBody.constraints = RigidbodyConstraints.FreezeAll;
       _player.SplineFollower.enabled = false;
       _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("FinalIDLE", 0));
       _uıManager.finalUI.gameObject.SetActive(true);
       _tween =_uıManager.finalUI.gameObject.transform.GetChild(0).GetComponent<RectTransform>()
            .DOAnchorPos(new Vector2(465, -478f), 1).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);

        _observables.InputObservable.Subscribe(x =>
        {
            if (_alreadyHit) return;
            _tween.Pause();
            _tween.Kill();
            _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("FINALPUNCH", 0));
            _signalBus.Fire(new SignalPunch());
            _alreadyHit = true;

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
