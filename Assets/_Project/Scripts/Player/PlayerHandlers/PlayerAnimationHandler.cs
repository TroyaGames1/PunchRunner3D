
namespace PlayerBehaviors
{
    using Zenject;
    using Events;

    public class PlayerAnimationHandler: IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly Player _player;
        
        public PlayerAnimationHandler(SignalBus signalBus, Player player)
        {
            _signalBus = signalBus;
            _player = player;
        }
        
        public void Initialize()
        {
           _signalBus.Subscribe<ISignalChangeAnimation>(x=>ChangeAnimation(x.Animation));
        }

        private void ChangeAnimation(string anim)
        {
            _player.GetAnimator.Play(anim);
        }

        public void SetFloat(string anim, float speed)
        {
            _player.GetAnimator.SetFloat(anim,speed);
        }
    }
}

