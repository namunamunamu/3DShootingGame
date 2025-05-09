using System;
using System.Security.Cryptography;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class UI_InputField
{
    public TextMeshProUGUI resultText;
    public TMP_InputField IDInputField;
    public TMP_InputField PWInputField;
    public TMP_InputField PWConfirmInputField;
    public Button ConfirmButton;
}

public class UI_LoginScene : MonoBehaviour
{
    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject RegisterPanel;


    [Header("로그인")] public UI_InputField LoginInputFields;


    [Header("회원가입")] public UI_InputField RegisterInputFields;

    private const string PREFIX = "ID_";
    private const string SALT = "qwer1234";


    private void Start()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
        LoginCheck();
    }

    public void OnClickResterButton()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    public void OnClickBackButton()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }

    public void Register()
    {
        string id = RegisterInputFields.IDInputField.text;
        if(string.IsNullOrEmpty(id))
        {
            RegisterInputFields.resultText.text = "아이디를 입력해주세요.";
            RegisterInputFields.resultText.rectTransform.DOShakeScale(0.2f);
            return;
        }

        // 비밀번호 입력을 확인
        string pw = RegisterInputFields.PWInputField.text;
        if(string.IsNullOrEmpty(pw))
        {
            RegisterInputFields.resultText.text = "비밀번호를 입력해주세요.";
            RegisterInputFields.resultText.rectTransform.DOShakeScale(0.2f);
            return;
        }

        // 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인
        string pwConfirm = RegisterInputFields.PWConfirmInputField.text;
        if(string.IsNullOrEmpty(pwConfirm) || pwConfirm != pw)
        {
            RegisterInputFields.resultText.text = "비밀번호와 비밀번호 확인이 같지 않습니다.";
            RegisterInputFields.resultText.rectTransform.DOShakeScale(0.2f);
            return;
        }

        // PlayerPrefs를 이용해 아이디와 비밀번호 저장
        PlayerPrefs.SetString(PREFIX + id, Encryption(pw + SALT));

        OnClickBackButton();
        RegisterInputFields.IDInputField.text = id;
    }

    public string Encryption(string text)
    {
        // 해시 암호와 알고리즘 인스턴스를 생성한다.
        SHA256 sha256 = SHA256.Create();

        // 운영체제 혹은 프로그램밍 언어별로 string을 표현한느 방식이 다르므로
        // UTF8 버전 바이트로 배열을 바꿔야한다.
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = string.Empty;
        foreach(byte b in hash)
        {
            // 바이트를 다시 string으로 바꿔서 이어붙이기
            resultText += b.ToString("X2");
        }

        return resultText;
    }

    public void Login()
    {
        // 아이디 입력을 확인
        string id = LoginInputFields.IDInputField.text;
        if(string.IsNullOrEmpty(id))
        {
            LoginInputFields.resultText.text = "아이디를 입력해주세요.";
            LoginInputFields.resultText.rectTransform.DOShakeScale(0.2f);
            return;
        }

        // 비밀번호 입력을 확인
        string pw = LoginInputFields.PWInputField.text;
        if(string.IsNullOrEmpty(pw))
        {
            LoginInputFields.resultText.text = "비밀번호를 입력해주세요.";
            LoginInputFields.resultText.rectTransform.DOShakeScale(0.2f);
            return;
        }

        // PlayerPrefs.Get을 이용하여 아이디와 비밀번호가 맞는지 확인
        if(!PlayerPrefs.HasKey(PREFIX + id) || PlayerPrefs.GetString(PREFIX + id) != Encryption(pw + SALT))
        {
            LoginInputFields.resultText.text = "아이디 혹은 패스워드가 틀렸습니다!";
            LoginInputFields.resultText.rectTransform.DOShakeScale(0.2f);
            return;
        };

        // 맞다면 로그인
        Debug.Log("로그인 성공");
        SceneManager.LoadScene(1);
    }

    public void LoginCheck()
    {
        string id = LoginInputFields.IDInputField.text;
        string pw = LoginInputFields.PWInputField.text;

        LoginInputFields.ConfirmButton.enabled = !string.IsNullOrEmpty(id)&&!string.IsNullOrEmpty(pw);
    }
}
