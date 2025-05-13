using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private PipeManager pipeManager_;

    [SerializeField]
    private Transform pipeSpawnPoint_;

    private void Start()
    {   
        // pipeManager_?.Init(ref pipeSpawnPoint_);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("Quiting game...");
        Application.Quit();
    }
}
