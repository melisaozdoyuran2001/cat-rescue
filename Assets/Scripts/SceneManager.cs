using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void StartGame() {
         UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Instructions() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OpenStartScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    public void Level1ToLevel2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
    public void ReturnToStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
