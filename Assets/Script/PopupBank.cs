using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{
    public GameObject defaultUI;
    public GameObject userInfoUI;
    public GameObject depositeUI;
    public GameObject withdrawUI;
    public GameObject remitUI;
    public GameObject loginUI;
    public GameObject signUpUI;
    public GameObject lowMoneyErrorUI;
    public GameObject incorrectInfoErrorUI;
    public GameObject noIDErrorUI;

    public TMP_InputField deposite_InputField;
    public TMP_InputField withdraw_InputField;
    public TMP_InputField remit_ID_InputField;
    public TMP_InputField remit_Amount_InputField;
    public TMP_InputField login_ID_InputField;
    public TMP_InputField login_PS_InputField;
    public TMP_InputField signUp_ID_InputField;
    public TMP_InputField signUp_Name_InputField;
    public TMP_InputField signUp_PS_InputField;
    public TMP_InputField signUp_PSConfirm_InputField;

    public TextMeshProUGUI signUpErrorText;

    private int depositeCustomNum;
    private int withdrawCustomNum;

    // ====ATM UI Set====
    public void SetDefaultUI()
    {
        depositeUI.SetActive(false);
        withdrawUI.SetActive(false);
        defaultUI.SetActive(true);
    }
    public void SetDepositeUI()
    {
        defaultUI.SetActive(false);
        depositeUI.SetActive(true);
    }
    public void SetWithdrawUI()
    {
        defaultUI.SetActive(false);
        withdrawUI.SetActive(true);
    }
    public void SetRemitUI()
    {
        defaultUI.SetActive(false);
        remitUI.SetActive(true);
    }

    // ====Deposit and Withdraw====
    public void DepositBtn(int num)
    {
        if (GameManager.Instance.TryDeposit(num)) return;
        else ActivateLowMoneyError();
    }
    public void WithdrawBtn(int num)
    {
        if (GameManager.Instance.TryWithdraw(num)) return;
        else ActivateLowMoneyError();
    }
    public void GetDepositeCustom()
    {
        if (!Int32.TryParse(deposite_InputField.text, out int result))
            return;

        depositeCustomNum = result;
    }
    public void GetWithdrawCustom()
    {
        if (!Int32.TryParse(withdraw_InputField.text, out int result))
            return;

        withdrawCustomNum = result;
    }
    public void DepositeCustom() => DepositBtn(depositeCustomNum);
    public void WithdrawCustom() => WithdrawBtn(withdrawCustomNum);

    // ====Remit====
    public void RemitBtn()
    {
        if (RemitError())
        {
            ActivateIncorrectInfoErrorUI();
            return;
        }

        switch(GameManager.Instance.TryRemit(
            remit_ID_InputField.text, remit_Amount_InputField.text))
        {
            case 1:
                ActivateNoIDError();
                return;
            case 2:
                ActivateLowMoneyError();
                return;
            case 3:                
                return;
        }
    }
    private bool RemitError()
    {
        if (remit_ID_InputField.text == "") return true;
        else if (remit_Amount_InputField.text == "") return true;
        else return false;
    }

    // ====Login and Sign up====
    public void LoginBtn()
    {
        if (!GameManager.Instance.TryLogin(
            login_ID_InputField.text, login_PS_InputField.text))
        {
            ActivateIncorrectInfoErrorUI();
            return;
        }
            
        loginUI.SetActive(false);
        defaultUI.SetActive(true);
        userInfoUI.SetActive(true);
    }
    public void SignUpBtn()
    {
        if (SignUpError())
        {
            ActivateIncorrectInfoErrorUI();
            return;
        }

        GameManager.Instance.SignUp(
            signUp_Name_InputField.text, signUp_ID_InputField.text, signUp_PS_InputField.text);

        // set UI
        DeactivateSignUpUI();
        loginUI.SetActive(false);
        defaultUI.SetActive(true);
        userInfoUI.SetActive(true);
    }
    private bool SignUpError()
    {
        if (signUp_ID_InputField.text == "")
        {
            signUpErrorText.text = "ID를 입력해주세요.";
            return true;
        }
        else if (signUp_Name_InputField.text == "")
        {
            signUpErrorText.text = "이름을 입력해주세요.";
            return true;
        }
        else if (signUp_PS_InputField.text == "")
        {
            signUpErrorText.text = "비밀번호를 입력해주세요.";
            return true;
        }
        else if (signUp_PS_InputField.text != signUp_PSConfirm_InputField.text)
        {
            signUpErrorText.text = "비밀번호가 일치하지 않습니다.";
            return true;
        }

        signUpErrorText.text = "";
        return false;
    }
    public void ActivateSignUpUI()
    {
        signUpUI.SetActive(true);
    }
    public void DeactivateSignUpUI()
    {
        signUpUI.SetActive(false);
    }

    // ====Error UI=====
    public void ActivateLowMoneyError()
    {
        lowMoneyErrorUI.SetActive(true);
    }
    public void DeactivateLowMoneyError()
    {
        lowMoneyErrorUI.SetActive(false);
    }
    public void ActivateIncorrectInfoErrorUI()
    {
        incorrectInfoErrorUI.SetActive(true);
    }
    public void DeactivateIncorrectInfoErrorUI()
    {
        incorrectInfoErrorUI.SetActive(false);
    }
    public void ActivateNoIDError()
    {
        noIDErrorUI.SetActive(true);
    }
    public void DeactivateNoIDError()
    {
        noIDErrorUI.SetActive(false);
    }
}
