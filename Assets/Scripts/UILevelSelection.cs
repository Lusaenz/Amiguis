using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UILevelSelection : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject levelListContainer;
    public int LevelAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < LevelAmount; i++)
        {
            var newObject = Instantiate(levelButtonPrefab, levelListContainer.transform);
            newObject.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + i.ToString();
            newObject.GetComponent<Button>().onClick.AddListener(LevelButtonClicked);
        }
        
    }
    private void LevelButtonClicked()
    {
        GameManager.Instance.StartGame();
    }

     public void BackButtonPressed()
    {
        GameManager.Instance.Idle();
    }
}
