using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using TMPro;
public class UIPoints : MonoBehaviour
{
    int displayedPoints = 0;
    public TextMeshProUGUI pointsLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnPointsUpdated.AddListener(UpdatePoints);

    }
    void UpdatePoints()
    {
        StartCoroutine(UpdatePointsCoroutine());
    }
    IEnumerator UpdatePointsCoroutine()
    {
        while (displayedPoints < GameManager.Instance.Points)
        {
            displayedPoints++;
            pointsLabel.text = displayedPoints.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
    
}
