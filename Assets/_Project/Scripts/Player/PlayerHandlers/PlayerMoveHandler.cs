using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{ 
    public class PlayerMoveHandler: IInitializable, ITickable
    {
        private readonly Player _player;
        private readonly PlayerObservables _observables;
        private Vector3 targetPos;

        PlayerMoveHandler(Player player,PlayerObservables observables)
        {
            _player = player;
            _observables = observables;
        }


        public void Initialize()
        {
            _observables.InputObservable.Subscribe(CheckInputs);
        }
        
        public void Tick()
        {
            _player.Position = Vector3.Lerp(_player.Position, targetPos, 3*Time.deltaTime);
        }

        void CheckInputs(Touch[] touches)
        {
            var touch = touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    break;
                case TouchPhase.Moved:
                    
                    targetPos= new Vector3(_player.Position.x,
                        _player.Position.y , _player.Position.z + (touch.deltaPosition.x * 0.025f));
                    break;
                
            }
        }

        
    }
}

