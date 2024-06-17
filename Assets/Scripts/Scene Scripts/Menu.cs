using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject guildUI;
    public void Play()
    {
        SceneManager.LoadScene("Level Select");
    }
    public void Guild()
    {
        guildUI.SetActive(true);
    }

    public void Close()
    {
        guildUI.SetActive(false);
    }
}
