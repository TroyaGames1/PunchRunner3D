using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelLoader: IInitializable
{
    private int _currentLevel;
    private readonly SignalBus _signal;

    LevelLoader(SignalBus signal)
    {
        _signal = signal;
    }


    public void Initialize()
    {
        
        _currentLevel= PlayerPrefs.GetInt("CurrentLevel");
        if (SceneManager.GetActiveScene().buildIndex==_currentLevel)
        {
         //   _signal.Fire<StartSignal>(); //TODO
            return;
        }
        SceneManager.LoadScene(_currentLevel);
    }
}
