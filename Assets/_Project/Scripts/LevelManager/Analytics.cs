using UnityEngine;

using GameAnalyticsSDK;

using UnityEngine.SceneManagement;
using Zenject;

public class Analytics : MonoBehaviour
{
    [Inject] private SignalBus _signal;
   
    public void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("CurrentLevel",SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }

    void Awake()
    {  

        LoadAnalyticSignals();
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.targetFrameRate = 60;
        }
        GameAnalytics.Initialize();
    }
   

  
    private void LoadAnalyticSignals()
    {
        _signal.Subscribe<SignalGameStart>(StartSignal);
        _signal.Subscribe<SignalPlayerFailed>(FailSignal);
        _signal.Subscribe<SignalGameFinished>(FinishSignal);
    }

    private void StartSignal()
    {
        GameAnalytics.NewProgressionEvent (GAProgressionStatus.Start,
            "World_01", 
            PlayerPrefs.GetInt("CurrentLevel").ToString(),
            "Level_Progress");

    }
    private void FailSignal()
    {
       GameAnalytics.NewProgressionEvent (GAProgressionStatus.Fail,
           "World_01", 
           PlayerPrefs.GetInt("CurrentLevel").ToString(),
           "Level_Progress");  
       

    }
    private void FinishSignal()
    {
      GameAnalytics.NewProgressionEvent (GAProgressionStatus.Complete,
          "World_01", 
          PlayerPrefs.GetInt("CurrentLevel").ToString(),
          "Level_Progress");
    }
}