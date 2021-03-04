using Events;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerColliderHandler : IInitializable
    {

        private readonly PlayerObservables _observables;
        private readonly SignalBus _signalBus;


        private PlayerColliderHandler(PlayerObservables observables,
           SignalBus signalBus)
        {
            _observables = observables;
            _signalBus = signalBus;
        }

        public void Initialize()
        {

            _observables.PlayerTriggerEnterObservable.Subscribe(x =>
            {
                _signalBus.Fire<SignalStartRaycasting>();
            });
            
        }

    }
}
 

