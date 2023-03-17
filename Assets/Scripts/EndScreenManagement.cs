using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManagement : MonoBehaviour
{
    public void TryAgain() {
        SceneManager.LoadSceneAsync("ThimbleKnight");
    }

    public void Exit() {
        Application.Quit();
    }
}
