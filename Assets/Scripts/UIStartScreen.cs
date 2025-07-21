using UnityEngine;

public class UIStartScreen : MonoBehaviour
{
    public void StarBtnClicked()
    {
        GameManager.Instance.StartGame();
    }
}
