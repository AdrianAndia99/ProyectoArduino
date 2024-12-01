using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void changeS(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public void salirP()
    {
        Application.Quit();
    }
}