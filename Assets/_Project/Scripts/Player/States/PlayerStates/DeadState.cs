using Events;
using Miscs;
using PlayerBehaviors;
using UnityEngine;
using Zenject;

namespace PlayerState
{
    public class DeadState : IState
    {
        private readonly UIManager _uıManager;
        private readonly SignalBus _signalBus;

        public DeadState(UIManager uıManager, SignalBus signalBus)
        {
            _uıManager = uıManager;
            _signalBus = signalBus;
        }

        public void EnterState()
        { 
            _signalBus.AbstractFire(new SignalChangeSpeedMovementFactorAndAnimation("DEAD", 0,0)); 
            _signalBus.Fire(new SignalPlayerFailed()); 
            _uıManager.deadUI.SetActive(true);
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
}

