using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    // Use singletons to access this class from other classes easily
    #region Singleton class: Level 

    public static Level Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    public int objectsInScene;
    public int totalObjects;

    [SerializeField] Transform objectsParent;

    void Start()
    {
        CountObjects();
    }

    void CountObjects ()
    {
        totalObjects = objectsParent.childCount;
        objectsInScene = totalObjects;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
