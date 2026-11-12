using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem Instance;

    private Level level;
    private GrabableSphere[] hooks;

    private int hookCount;
    private int destroyedHooks;

    public event Action OnWin;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.buildIndex == 1)
        {
            Instantiate(level.arena);

            hooks = FindObjectsByType<GrabableSphere>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
            hookCount = hooks.Length;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }
        else
            level = null;
    }

    public void SetLevel(Level level)
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        this.level = level;
    }

    public Level GetLevel()
    {
        return level;
    }

    public void IncreaseDestroyedHookCount()
    {
        destroyedHooks++;

        if (destroyedHooks == hookCount)
        {
            // WIN SCREEN
            OnWin?.Invoke();
        }
    }

    public void DecreaseDestroyedHookCount()
    {
        destroyedHooks--;
    }
}
