using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] string questionScenes;
    [SerializeField] string finalScene;
    [SerializeField] string titleScene;

    public void LoadQuestionsScene() 
    {
        SceneManager.LoadScene(questionScenes);    
    }

    public void LoadFinalScene()
    {
        SceneManager.LoadScene(finalScene);
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(titleScene);
    }
}
