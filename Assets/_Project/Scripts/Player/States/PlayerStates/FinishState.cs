using System;
using Events;
using Miscs;
using PlayerBehaviors;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerState
{
    public class FinishState : IState
    {
        private readonly UIManager _uıManager;
        readonly SignalBus _signalBus;
        private PlayerAnimationHandler _animationHandler;
        private float vars = 0;
        public FinishState(UIManager uıManager, SignalBus signalBus, PlayerAnimationHandler animationHandler)
        {
            _uıManager = uıManager;
            _signalBus = signalBus;
            _animationHandler = animationHandler;
        }

        public void EnterState()
        {
            Observable.Timer(TimeSpan.FromSeconds(1))
                .Subscribe(x=>_uıManager.finishGameUI.SetActive(true));
            _signalBus.AbstractFire
                (new SignalChangeSpeedMovementFactorAndAnimation("FinalFinish", 0, 0));
        }

        public void ExitState()
        {
            _uıManager.finishGameUI.SetActive(false);
        }

        public void Update()
        {
            if (vars<1)
            {
                vars += 0.3f*Time.deltaTime;
            }
            
            _animationHandler.SetFloat("Finish",vars);
        }

        public void FixedUpdate()
        {
        }
    }

}
