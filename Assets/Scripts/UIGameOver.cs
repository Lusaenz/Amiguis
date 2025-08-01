using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using TMPro;


public class UIGameOver : MonoBehaviour
{
    public int displayedPoints = 0;
    public TextMeshProUGUI pointsUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnGameStateUpdated.AddListener(GameStateUpadated);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateUpdated.RemoveListener(GameStateUpadated);
    }
    private void GameStateUpadated(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.GameOver)
        {
            displayedPoints = 0;
            StartCoroutine(DisplayedPointsCoroutine());
        }
    }
    IEnumerator DisplayedPointsCoroutine()
    {
        while (displayedPoints < GameManager.Instance.Points)
        {
            displayedPoints++;
            pointsUI.text = displayedPoints.ToString();
            yield return new WaitForFixedUpdate();
        }
        displayedPoints = GameManager.Instance.Points;
        pointsUI.text = displayedPoints.ToString();

        yield return null;
    }

    public void PlayAgainBtnClicked()
    {
        GameManager.Instance.RestartGame();

    }
    public void ExitGameBtnClicked()
    {
        GameManager.Instance.ExitGame();
        
    }

}
