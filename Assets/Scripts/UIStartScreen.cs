using UnityEngine;

public class UIStartScreen : MonoBehaviour
{
    public void StarBtnClicked()
    {
        GameManager.Instance.LevelSelection();
    }
    public void OptionsBtnClicked()
    {
        GameManager.Instance.Options();
    }
}
