using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] private UserData currentUserData;
    
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI userNameText;

    private static string path;
    private bool _login = false;
    private bool _dirty = false;
    private Coroutine _auto = null;
    private float intervalSec = 60f;

    public static GameManager Instance { get { return instance; } }

    // ----Lifecycle Methods====
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
#if UNITY_EDITOR
        path = Application.dataPath + "/Data/";
#else
        path = Application.persistentDataPath + "/Data/";
#endif
    }
    private void Start()
    {
        _login = false;
        _dirty = false;
    }
    private void OnApplicationQuit()
    {
        OnLogout();
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) SaveIfDirty();
    }


    // ====UserData Methods====
    public void UpdateCurrentUserData(UserData saveData)
    {
        currentUserData = saveData;
        _dirty = true;
        UIRefresh();
    }
    public void SaveUserData(string ID, UserData saveData)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path + ID + ".json", json);

        Debug.Log($"Save User Data {ID}.json in {path}");
    }
    public UserData LoadUserData(string ID, string password)
    {
        if (!File.Exists(path + ID + ".json"))
        {
            Debug.Log("No UserData exist.");
            return null;
        }
        else
        {
            string loadJson = File.ReadAllText(path + ID + ".json");
            UserData loadUserData = JsonUtility.FromJson<UserData>(loadJson);

            if (loadUserData.password != password)
            {
                Debug.Log("Password incorrect.");
                return null;
            }

            UpdateCurrentUserData(loadUserData);
            return loadUserData;
        }
    }
    public void SaveIfDirty()
    {
        if (!_login || currentUserData == null) return;
        if (!_dirty) return;

        SaveUserData(currentUserData.ID, currentUserData);
        _dirty = false;
    }
    IEnumerator SaveLoop()
    {
        var wait = new WaitForSecondsRealtime(intervalSec);
        while (true)
        {
            yield return wait;
            SaveIfDirty();
        }
    }
    public bool TryLogin(string ID, string password) 
    {
        if (LoadUserData(ID, password) != null)
        {
            _login = true;

            OnLogin();
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnLogin()
    {
        _login = true;

        if (_auto == null)
            _auto = StartCoroutine(SaveLoop());
    }
    private void OnLogout()
    {
        SaveIfDirty();
        _login = false;
        if (_auto != null)
        {
            StopCoroutine(_auto);
            _auto = null;
        }
    }
    public void SignUp(string name, string ID, string password)
    {
        UserData userData = new UserData(name, ID, password);
        UpdateCurrentUserData(userData);

        OnLogin();
    }
    private void UIRefresh()
    {
        balanceText.text = currentUserData.balance.ToString("N0");
        cashText.text = currentUserData.cash.ToString("N0");
        userNameText.text = currentUserData.name.ToString();
    }

    // ====Deposite and Withdraw====
    public bool TryDeposit(int num)
    {
        if (currentUserData.cash < num)
        {
            return false;
        }
        else
        {
            // change values
            UserData userData = currentUserData;
            userData.cash -= num;
            userData.balance += num;
            // apply
            UpdateCurrentUserData(userData);
            return true;
        }
    }
    public bool TryWithdraw(int num)
    {
        if (currentUserData.balance < num)
        {
            return false;
        }
        else
        {
            // change values
            UserData userData = currentUserData;
            userData.balance -= num;
            userData.cash += num;
            // apply
            UpdateCurrentUserData(userData);
            return true;
        }
    }


}