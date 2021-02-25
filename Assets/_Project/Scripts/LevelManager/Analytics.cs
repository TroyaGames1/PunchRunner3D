#if STORE_BUILD
using System;
using UnityEngine;
using System.Collections;
using Facebook.Unity;
using GameAnalyticsSDK;
using Zenject;

public class Analytics : MonoBehaviour
{
   [Inject] private SignalBus _signal;


    void Awake()
    {  

        LoadAnalyticSignals();
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.targetFrameRate = 60;
        }
        FB.Init(FBInitCallback);
        GameAnalytics.Initialize();
    }
    private void FBInitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
    }
    public void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
        }
    }

    
    private void LoadAnalyticSignals()
    {
        _signal.Subscribe<StartSignal>(StartSignal);
        _signal.Subscribe<FailSignal>(FailSignal);
        _signal.Subscribe<CompleteSignal>(FinishSignal);
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
#endif