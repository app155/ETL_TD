using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerWrapped : MonoBehaviour
{
    public static SceneManagerWrapped instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("SceneManager").AddComponent<SceneManagerWrapped>();
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }

    private static SceneManagerWrapped _instance;

    private List<ISceneListener> _listeners = new List<ISceneListener>();


    public void Register(ISceneListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnRegister(ISceneListener listener)
    {
        _listeners.Remove(listener);
    }

    public void LoadScene(int sceneIndex)
    {
        foreach (var listener in _listeners)
        {
            listener.OnBeforeSceneLoaded();
        }

        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1.0f;

        foreach (var listener in _listeners)
        {
            listener.OnAfterSceneLoaded();
        }
    }
}
