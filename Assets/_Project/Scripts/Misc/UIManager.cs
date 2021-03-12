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
        public  GameObject inGameUI{ get; private set; }
        public  GameObject deadUI { get; private set; }
        public  GameObject finalUI { get; private set; }
        public  GameObject finishGameUI { get; private set; }
        
        
        void SetUIGetters()
        {
            preGameUI = _settings.PreGameUI;
            inGameUI = _settings.InGameUI;
            deadUI = _settings.DeadUI;
            finalUI = _settings.FinalUI;
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
                var nextLevelIndex = SceneManager.GetActiveScene().buildIndex+1;
                PlayerPrefs.SetInt("CurrentLevel",nextLevelIndex);
                SceneManager.LoadScene(nextLevelIndex);
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
            public GameObject FinalUI;
            [TabGroup("STATE UI")]
            public GameObject FinishUI;

            [TabGroup("BUTTONS")] public Button RestartButton;
            [TabGroup("BUTTONS")] public Button NextLevelButton;


        }

       
    }

}
