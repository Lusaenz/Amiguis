using UnityEngine;

public class HideObjectsOnGameState : MonoBehaviour
{
    public GameObject targe;
    public GameManager.GameState showOnstate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targe.SetActive(showOnstate == GameManager.Instance.gameState);
        GameManager.Instance.OnGameStateUpdated.AddListener(GamesStateUpdated);
    }
    private void GamesStateUpdated(GameManager.GameState newState)
    {
        targe.SetActive(showOnstate == GameManager.Instance.gameState);
    }

    
}
