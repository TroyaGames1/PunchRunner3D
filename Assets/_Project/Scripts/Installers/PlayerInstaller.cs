using System;
using PlayerBehaviors;
using PlayerState;
using UnityEngine;
using Zenject;

/// <summary>
/// Playerin kendi içinde verilecek bağımlılıklar buradan yüklenecek
/// </summary>

namespace Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        Settings _settings = null;
        public override void InstallBindings()
        {
            Container.Bind<Player>().AsSingle()
                .WithArguments(_settings.Rigidbody,_settings.Animator);
        
            StateManagerInstall();
        
        }

        void StateManagerInstall()
        {
            Container.BindInterfacesAndSelfTo<PlayerStateManager>().AsSingle();
        
            Container.Bind<IdleState>().AsSingle();
            Container.Bind<RunningState>().AsSingle();
            Container.Bind<DeadState>().AsSingle();
            Container.Bind<FinishState>().AsSingle();
        }
    
        [Serializable]
        public class Settings
        {
            public Rigidbody Rigidbody;
            public Animator Animator;
        }
    }
}
