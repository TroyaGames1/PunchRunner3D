using System;
using Events;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerColliderHandler: IInitializable
    {
        private readonly Player _player;

        private readonly PlayerObservables _observables;
        private readonly PlayerRaycastHandler _raycastHandler;
       

        private PlayerColliderHandler(Player player, PlayerObservables observables, PlayerRaycastHandler raycastHandler)
        {
            _player = player;
            _observables = observables;
            _raycastHandler = raycastHandler;
        }

        public void Initialize()
        {
            
            _observables.PlayerTriggerStayObservable.Subscribe(x =>
            {
               _raycastHandler.CheckRayCasts();
            });
        }

       

     
      
    }

}

