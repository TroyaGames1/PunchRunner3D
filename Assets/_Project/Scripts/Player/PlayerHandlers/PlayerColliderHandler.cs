using Events;
using PlayerState;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerColliderHandler : IInitializable
    {

        private readonly PlayerObservables _observables;
        private readonly SignalBus _signalBus;
        private readonly Player _player;
        private readonly PlayerStateManager _stateManager;
       

        private PlayerColliderHandler(PlayerObservables observables,
           SignalBus signalBus, Player player, PlayerStateManager stateManager)
        {
            _observables = observables;
            _signalBus = signalBus;
            _player = player;
            _stateManager = stateManager;
        }

        public void Initialize()
        {

            _observables.PlayerTriggerEnterObservable.Subscribe(x =>
            {
                _signalBus.Fire<SignalStartRaycasting>();
            }); 
            _observables.PlayerTriggerStayObservable.Subscribe(x =>
            {
                _signalBus.Fire<SignalStartRaycasting>();
            });

            _observables.PlayerTriggerEnterObservable.Where(x => x.gameObject.CompareTag("BoxMachine")&& _stateManager.CurrentState==PlayerStateManager.PlayerStates.RunningState)
                .Subscribe(x =>
                    {
                        _stateManager.ChangeState(PlayerStateManager.PlayerStates.FinalState);
                        _player.GO.transform.position = x.gameObject.transform.GetChild(0).position;
                    }
                );

        }

    }
}
 

