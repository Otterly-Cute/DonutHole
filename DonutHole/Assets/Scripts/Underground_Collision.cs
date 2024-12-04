using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Underground_Collision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!Game_Data.isGameOver)
        {
            string tag = other.tag;

            if (tag.Equals("Object"))
            {
                Level.Instance.objectsInScene--;
                UI_Manager.Instance.UpdateLevelProgress();

                Destroy(other.gameObject);

                // Check if win
                if (Level.Instance.objectsInScene == 0)
                {
                    // No more objects to collect
                    UI_Manager.Instance.ShowLevelCompletedUI();

                    Invoke("NextLevel", 2f);
                }
            }

            if (tag.Equals("Obstacle"))
            {
                Game_Data.isGameOver = true;

                //making the camera shake
                Camera.main.transform
                    .DOShakePosition(1f, .2f, 20, 90f)
                    .OnComplete(() => 
                    {
                        Level.Instance.RestartLevel();
                    });
            }
        }
    }

    void NextLevel()
    {
        Level.Instance.LoadNextLevel();
    }
}
