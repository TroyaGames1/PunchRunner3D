using System;
using Dreamteck.Splines;
using PlayerBehaviors;
using PlayerState;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Playerin kendi içinde verilecek bağımlılıklar buradan yüklenecek
/// </summary>

namespace Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        [TabGroup("LocalSettings")]
        Settings _settings = null;
        
        [SerializeField]
        [TabGroup("OtherSettings")]
        private PlayerSettings _playerSettings;
        public override void InstallBindings()
        {
            Container.Bind<Player>().AsSingle()
                .WithArguments(_settings.Rigidbody,_settings.PlayerModel,_settings.Animator,_settings.SplineFollower,_settings.Slider);
        
            StateManagerInstall();
            InstallPlayerHandlers();
            InstallSettings();

        }

        private void InstallSettings()
        {
            Container.BindInstance(_playerSettings.HealthHandler);
        }

        private void InstallPlayerHandlers()
        {
            Container.Bind<PlayerObservables>().AsSingle();
            Container.BindInterfacesTo<PlayerColliderHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerRaycastHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMoveHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerAnimationHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerHealthHandler>().AsSingle();
        }
        void StateManagerInstall()
        {
            Container.BindInterfacesAndSelfTo<PlayerStateManager>().AsSingle();
        
            Container.Bind<IdleState>().AsSingle();
            Container.Bind<RunningState>().AsSingle();
            Container.Bind<DeadState>().AsSingle();
            Container.Bind<FinalState>().AsSingle();
            Container.Bind<FinishState>().AsSingle();
        }
    
        [Serializable]
        [HideLabel]
        public class Settings
        {
            public Rigidbody Rigidbody;
            public GameObject PlayerModel;
            public Animator Animator;
            public SplineFollower SplineFollower;
            public Slider Slider;
        }

        [HideLabel]
        [Serializable]
        public class PlayerSettings
        {
            [TabGroup("HealtSettings")]
            [HideLabel]
            public PlayerHealthHandler.SceneSettings HealthHandler;
        }
    }
}
