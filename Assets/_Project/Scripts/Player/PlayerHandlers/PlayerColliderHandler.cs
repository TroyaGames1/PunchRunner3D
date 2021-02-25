using UniRx;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerColliderHandler: IInitializable
    {
        private PlayerObservables _observables;

        PlayerColliderHandler(PlayerObservables playerObservables)
        {
            _observables = playerObservables;
        }


        public void Initialize()
        {
            _observables.PlayerTriggerEnterObservable.Where(x => x.gameObject.CompareTag("Obstacles/"));
        }
    }

}

