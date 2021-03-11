using System;
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
    private readonly PlayerStateManager _stateManager;
    private readonly PlayerAnimationHandler _animationHandler;
    private Tween _tween;
    private bool _alreadyHit;
    private RectTransform _hitCursor;

    public FinalState(SignalBus signalBus, PlayerObservables observables, Player player, UIManager uıManager, PlayerStateManager stateManager, PlayerAnimationHandler animationHandler)
    {
        _signalBus = signalBus;
        _observables = observables;
        _player = player;
        _uıManager = uıManager;
        _stateManager = stateManager;
        _animationHandler = animationHandler;
    }


    public void EnterState()
    {       
        _uıManager.finalUI.SetActive(true);
        _player.Slider.gameObject.SetActive(false);
        _player.SplineFollower.enabled = false;
        _player.RigidBody.constraints = RigidbodyConstraints.FreezeAll;
        
        _signalBus.AbstractFire(new SignalChangeSpeedMovementFactorAndAnimation("FinalIDLE", 0,0));

        _hitCursor = _uıManager.finalUI.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        _tween =_hitCursor
            .DOAnchorPos(new Vector2(519, 248), 0.7f).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);

        Observable.Timer(TimeSpan.FromSeconds(0.7)).Subscribe(x =>
        {
            _observables.InputObservable.Subscribe(y =>
            {
                if (_alreadyHit) return;
                
                _tween.Pause();
                _tween.Kill();
                _signalBus.AbstractFire(new SignalChangeSpeedMovementFactorAndAnimation("FinalFinish", 0,0));
                _animationHandler.SetFloat("Finish",0);
                _signalBus.Fire(new SignalPunch(_hitCursor.anchoredPosition.x));
                _alreadyHit = true;
                Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(z =>
                {
                    _uıManager.finalUI.SetActive(false);
                    _stateManager.ChangeState(PlayerStateManager.PlayerStates.FinishState);
                });

            });
        });

    }



    public void ExitState()
    {
        
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }
}
