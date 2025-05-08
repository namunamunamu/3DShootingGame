
public class UI_OptionPopup : UI_Popup
{
    public void OnClickContinueButton()
    {
        GameManager.Instance.Continue();
        UI_Manager.Instance.ClosePopup();
    }

    public void OnClickRetryButton()
    {
        GameManager.Instance.Restart();
    }

    public void OnClickQuitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnClickCreditButton()
    {
        UI_Manager.Instance.Open(EPopupType.UI_CreditPopup);
    }
}
