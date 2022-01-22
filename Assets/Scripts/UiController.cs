using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{

    public static UiController instance;
    public TMP_Text gamePlayScoreText, endPanelScoreText;
    public GameObject gamePlayPanel, endGamePanel, startPanel;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }


   
    void Start()
    {
        StartScreenActive();
    }


    public void SetGPScoreText()
	{
        gamePlayScoreText.text = GameManager.instance.score.ToString();
	}

    public void SetEndPanelScore()
	{
        endPanelScoreText.text ="SCORE : " + GameManager.instance.score.ToString();
	}

    public void StartScreenActive()
	{
        startPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
        endGamePanel.SetActive(false);
	}

    public void EndPanelActive()
	{
        endGamePanel.SetActive(true);
        startPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        SetEndPanelScore();
	}

    public void GamePanelActive()
	{
        gamePlayPanel.SetActive(true);
        startPanel.SetActive(false);
        endGamePanel.SetActive(false);
    }
}
