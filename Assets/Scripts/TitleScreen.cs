using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    [SerializeField] private GameObject tutorialImage;
    [SerializeField] private GameObject creditsImage;
    public void PlayGame() {
        SceneManager.LoadSceneAsync("ThimbleKnight");
    }

    public void Tutorial() {
        tutorialImage.SetActive(true);
    }

    public void Credits() {
        creditsImage.SetActive(true);
    }
}
