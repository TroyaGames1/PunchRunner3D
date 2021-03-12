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
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            _currentLevel= PlayerPrefs.GetInt("CurrentLevel");
        }
         
        if (SceneManager.GetActiveScene().buildIndex==_currentLevel)
        {
            _signal.Fire<StartSignal>();
            return;
        }
        SceneManager.LoadScene(_currentLevel);
    }
}