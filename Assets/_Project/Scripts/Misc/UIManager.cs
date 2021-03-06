using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Miscs
{
    public class UIManager
    {
        readonly Settings _settings;
        UIManager(Settings settings)
        {
            _settings = settings;
            SetUIGetters();
            SetButtons();
        }

        public GameObject preGameUI { get; private set; }
        public  GameObject ınGameUI{ get; private set; }
        public  GameObject deadUI { get; private set; }
        public  GameObject finishGameUI { get; private set; }
        
        
        void SetUIGetters()
        {
            preGameUI = _settings.PreGameUI;
            ınGameUI = _settings.InGameUI;
            deadUI = _settings.DeadUI;
            finishGameUI = _settings.FinishUI;
        }
        void SetButtons()
        {
            _settings.RestartButton.OnClickAsObservable().Subscribe(x =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }).AddTo(_settings.RestartButton);
            _settings.NextLevelButton.OnClickAsObservable().Subscribe(x =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            }).AddTo(_settings.NextLevelButton);
        }
        
        [Serializable]
        public class Settings
        {
        
            [TabGroup("STATE UI")]
            public GameObject PreGameUI;
            [TabGroup("STATE UI")]
            public GameObject InGameUI;
            [TabGroup("STATE UI")]
            public GameObject DeadUI;
            [TabGroup("STATE UI")]
            public GameObject FinishUI;

            [TabGroup("BUTTONS")] public Button RestartButton;
            [TabGroup("BUTTONS")] public Button NextLevelButton;


        }

       
    }

}
