using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private PipeManager pipeManager_;

    [SerializeField]
    private Transform pipeSpawnPoint_;

    [SerializeField]
    private GameObject cover_screen_;

    private void Start()
    {   
        // pipeManager_?.Init(ref pipeSpawnPoint_);
    }

    public void StartGame()
    {
        cover_screen_.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quiting game...");
        Application.Quit();
    }
}
