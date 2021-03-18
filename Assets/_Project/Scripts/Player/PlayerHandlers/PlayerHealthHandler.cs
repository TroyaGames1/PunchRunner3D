using System;
using Events;
using PlayerState;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerHealthHandler: IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly Player _player;
        private readonly PlayerStateManager _stateManager;
        private ProjectSettings _projectSettings;
        private readonly SceneSettings _sceneSettings;

        public PlayerHealthHandler(SignalBus signalBus, Player player, ProjectSettings projectSettings, PlayerStateManager stateManager, SceneSettings sceneSettings)
        {
            _signalBus = signalBus;
            _player = player;
            _projectSettings = projectSettings;
            _stateManager = stateManager;
            _sceneSettings = sceneSettings;
        }

        public void Initialize()
        {
           _signalBus.Subscribe<SignalPlayerHit>(x=>ChangeHP(x.Value));
           
           LoadStartSettings();
           
        }

        void LoadStartSettings()
        {
            _player.Slider.value = _projectSettings.PlayerHP;
      
        }
        void ChangeHP(float value)
        {
            _player.Slider.value += value;
            _projectSettings.PlayerHP += value;
            _projectSettings.PlayerHP = Mathf.Clamp(_projectSettings.PlayerHP, 0, 1);

            
            foreach (var settingsHand in _sceneSettings.PlayerHands)
            {
               
                settingsHand.gameObject.transform.localScale += new Vector3(value, value, value);
                if (settingsHand.gameObject.transform.localScale.x>2.2f)
                {
                    settingsHand.gameObject.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);

                }
            }
            CheckHP();
        }

        void CheckHP()
        {
            if (_projectSettings.PlayerHP<=0.05f)
            {
               _stateManager.ChangeState(PlayerStateManager.PlayerStates.DeadState);
            }
        }
        
        [Serializable]
        public struct ProjectSettings
        {
            public float PlayerHP;
        }

        [Serializable]
        public struct SceneSettings
        {
            public GameObject[] PlayerHands;
        }
    }

}

