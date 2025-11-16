using UnityEngine;

public class HideObjectsOnGameState : MonoBehaviour
{
    public GameObject targe;
    public GameManager.GameState hideOnstate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hideOnstate == GameManager.Instance.gameState)
        {
            targe.SetActive(false);  
        }
        GameManager.Instance.OnGameStateUpdated.AddListener(GamesStateUpdated);
    }
    private void GamesStateUpdated(GameManager.GameState newState)
    {
        targe.SetActive(hideOnstate != newState);  
    }

    
}
