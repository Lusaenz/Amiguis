using UnityEngine;

public class UIInGame : MonoBehaviour
{
  public void PauseButtonPressed()
    {
        GameManager.Instance.Pause();
    }
}
