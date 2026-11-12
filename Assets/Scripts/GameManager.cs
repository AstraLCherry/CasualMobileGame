using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Platform
    {
        PC,
        Android
    }

    public static GameManager Instance;

    [SerializeField] private Platform platform;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Platform GetPlatform()
    {
        return platform;
    }
}
