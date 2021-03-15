using System;
using Dreamteck.Splines;
using PlayerState;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerFacade : MonoBehaviour
    {

        Player _model;
        private PlayerStateManager _stateManager;
        private Settings _settings;

       

        [Inject]
        public void Construct(Player player,PlayerStateManager stateManager,Settings settings)
        {
            _model = player;
            _stateManager = stateManager;
            _settings = settings;
        }

        public bool IsDead => _model.IsDead;

        public Vector3 Position
        {
            get => _model.Position;
            set => _model.Position = value;
        }

        public Quaternion Rotation => _model.Rotation;

        public GameObject GO => _model.GO;

        public Animator Animator => _model.GetAnimator;


        public Rigidbody Rigidbody => _model.RigidBody;

        public SplineFollower SplineFollower => _model.SplineFollower;

        public Slider HPSlider => _model.Slider;
        
        void OnApplicationQuit()
        {
             _stateManager.CurrentStateAsIstate.ExitState();

        }

        private void Awake()
        {
            this.ObserveEveryValueChanged(x => SplineFollower.followSpeed)
                .Where(x => x == 0).Subscribe(x => _settings.WindParticle.SetActive(false));this.ObserveEveryValueChanged(x => SplineFollower.followSpeed)
                .Where(x => x != 0).Subscribe(x => _settings.WindParticle.SetActive(true));

        }

        [Serializable]
        public struct Settings
        {
            public GameObject WindParticle;
        }
  
    }
}
