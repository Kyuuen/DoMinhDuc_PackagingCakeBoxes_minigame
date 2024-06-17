using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    [SerializeField] Image[] decorStars;
    [SerializeField] Sprite[] starSprites;

    [SerializeField] GameObject lockUI;
    [SerializeField] GameObject textUI;
    public void Locked()
    {
        foreach (Image item in decorStars)
        {
            item.sprite = starSprites[0];
        }
        lockUI.SetActive(true);
        textUI.SetActive(false);
    }
    public void Unlocked()
    {
        foreach (Image item in decorStars)
        {
            item.sprite = starSprites[1];
        }
        lockUI.SetActive(false);
        textUI.SetActive(true);
    }

    public void DidNotWin()
    {
        foreach (Image item in decorStars)
        {
            item.sprite = starSprites[0];
        }
        lockUI.SetActive(false);
        textUI.SetActive(true);
    }
}
