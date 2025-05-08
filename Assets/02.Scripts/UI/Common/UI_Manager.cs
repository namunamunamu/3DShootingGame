using System;
using System.Collections.Generic;
using UnityEngine;

public enum EPopupType
{
    UI_OptionPopup,
    UI_CreditPopup
}

public class UI_Manager : SingletonBehaviour<UI_Manager>
{
    [Header("UI")]
    public UI_PlayerStatus PlayerStatusPanel;
    public UI_WeaponPanel WeaponPanel;


    [Header("Popups")]
    public List<UI_Popup> Popups;

    public Stack<UI_Popup> PopupStack => _popupStack;
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(PopupStack.Count > 0)
            {
                ClosePopup();
            }
            else
            {
                GameManager.Instance.Pause();
            }
        }
    }


    public void Open(EPopupType popupType, Action closeCallback = null)
    {
        OpenPopup(popupType.ToString(), closeCallback);
    }


    private void OpenPopup(string popupName, Action closeCallback = null)
    {
        foreach(UI_Popup popup in Popups)
        {
            if(popup.gameObject.name == popupName)
            {
                popup.Open(closeCallback);
                _popupStack.Push(popup);
                return;
            }
        }

        Debug.LogWarning("유효하지 않은 팝업 실행");
    }


    public void ClosePopup()
    {
        if(_popupStack.Count <= 0)
        {
            Debug.LogWarning("유효하지 않은 팝업 종료");
            return;
        }

        UI_Popup popup = _popupStack.Pop();
        popup.Close();
    }
}
