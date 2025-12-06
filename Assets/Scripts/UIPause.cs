using UnityEngine;

public class UIPause : MonoBehaviour
{
    public void ContinueButtonPressed()
    {
        GameManager.Instance.ContunueGame();
    }
    public void ExitButtonPressed()
    {
        GameManager.Instance.ExitGame();
    }
}
