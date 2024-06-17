using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] Sprite locked;
    [SerializeField] Sprite opened;
    public Button[] buttons;

    private void Awake()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
            buttons[i].GetComponent<LevelButtons>().Locked();
        }
        for (int i = 0; i < GameManager.currentLevel; i++)
        {
            buttons[i].interactable = true;
            buttons[i].GetComponent<LevelButtons>().Unlocked();
        }
        if(GameManager.currentWonLevel < GameManager.currentLevel)
        {
            buttons[GameManager.currentWonLevel].GetComponent<LevelButtons>().DidNotWin();
        }
    }
    public void OpenLevel(int levelID)
    {
        string levelName = "Level" + levelID;
        SceneManager.LoadScene(levelName);
    }
}
