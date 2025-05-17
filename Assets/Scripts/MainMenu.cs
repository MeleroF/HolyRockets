using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private PipeManager pipeManager_;

    [SerializeField]
    private Transform pipeSpawnPoint_;

    [SerializeField]
    private GameObject cover_screen_;

    [SerializeField]
    private GameObject high_score_window_;

    [SerializeField]
    private TextMeshProUGUI high_score_text_;

    private int high_score_;

    private int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    private void SetHighScoreWindow()
    {
        high_score_ = GetHighScore();

        if (high_score_ == 0)
        {
            high_score_window_.SetActive(false);
        }
        else
        {
            high_score_text_.text = "¡" + high_score_.ToString() + "!";
            high_score_window_.SetActive(true);
        }
    }

    private void Start()
    {
        SetHighScoreWindow();
    }

    public void StartGame()
    {
        cover_screen_.SetActive(true);
        AudioManager.instance_.PlaySFX(0);
    }

    public void Quit()
    {
        Debug.Log("Quiting game...");
        Application.Quit();
    }
}
