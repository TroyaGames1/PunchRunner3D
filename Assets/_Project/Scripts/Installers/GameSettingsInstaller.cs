using System;
using Miscs;
using PlayerBehaviors;
using Sirenix.OdinInspector;
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
    /// 
    [InlineEditor()] 
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    
    [TabGroup("PlayerSettings")]
    [HideLabel]
    [SerializeField] 
    private  PlayerSettings Player;
    
    
    //[TabGroup("EnemySettings")]
    //[HideLabel]
    //[SerializeField] 
    //private  EnemySettings enemySettings;
   
    ///EnemySettings Enemy;
   
    [Serializable]
    public class PlayerSettings
    {
       
        [TabGroup("RayCast Settings")]
        [HideLabel]
        public PlayerRaycastHandler.Settings RaycastSettings;
        
        [TabGroup("MoveHandler Settings")]
        [HideLabel]
        public PlayerMoveHandler.Settings MoveHandlerSettings;

    }
    
    [Serializable]
    public class EnemySettings
    {
        //Enemmy Classındaki açılan settings classları buraya yüklenecek
    }
    
    public override void InstallBindings()
    {
      
        Container.BindInstance(Player.RaycastSettings).AsSingle();
        Container.BindInstance(Player.MoveHandlerSettings).AsSingle();


    }
}}
