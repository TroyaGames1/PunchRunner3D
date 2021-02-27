using System;
using PlayerBehaviors;
using UnityEngine;
using Zenject;



namespace Installers 
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]

    /// <summary>
    /// Public olarak eklenecek prefablar,ayarlar vesaire buraya  yüklenecek
    /// Amaç tek yerden yönetmek ve birden fazla ayara sahip olmak
    /// </summary>
    ///
    ///
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{

    [SerializeField] private  PlayerSettings Player;
   
    ///EnemySettings Enemy;
   
    [Serializable]
    public class PlayerSettings
    {
        public PlayerColliderHandler.Settings ColliderSettings;
    }
    [Serializable]
    public class EnemySettings
    {
        //Enemmy Classındaki açılan settings classları buraya yüklenecek
    }
    
    public override void InstallBindings()
    {
        Container.BindInstance(Player.ColliderSettings).AsSingle();


    }
}}
