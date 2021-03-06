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
            _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("DEAD", 0));
            _uıManager.deadUI.SetActive(true);
        }

        public void ExitState()
        {
            Debug.Log("PlayerDeadStateExit");
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
    }
}

