using System;
using Miscs;
using PlayerBehaviors;
using UniRx;
using UnityEngine;


namespace PlayerState
{
    public class IdleState : IState,IDisposable
    { 
        readonly UIManager _uıManager;
        private readonly PlayerObservables _observables;
        private readonly PlayerStateManager _stateManager;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        IdleState(UIManager _uı,PlayerObservables observables,PlayerStateManager stateManager)
        {
            _uıManager = _uı;
            _observables=observables;
            _stateManager = stateManager;

        }
        
        public void EnterState()
        {

            Physics.reuseCollisionCallbacks = true;

            _uıManager.preGameUI.SetActive(true);

            _observables.InputObservable
                .Where(x => x.Length > 0 && 
                            _stateManager.CurrentState!=PlayerStateManager.PlayerStates.DeadState
                            && _stateManager.CurrentState!=PlayerStateManager.PlayerStates.FinishState&&
                            _stateManager.CurrentState!=PlayerStateManager.PlayerStates.FinalState)
                .Subscribe(x => _stateManager.ChangeState(PlayerStateManager.PlayerStates.RunningState))
                .AddTo(_disposable);

        }

        public void ExitState()
        {
            _uıManager.preGameUI.SetActive(false);
        }

   

        public void FixedUpdate()
        {
        }

        public void Update()
        {
         
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }

}
