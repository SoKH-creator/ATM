using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string name;
    public int cash;
    public int balance;
    public string ID;
    public string password;

    public UserData(string userName, int cash, int balance, string ID, string password)
    {
        this.name = userName;
        this.cash = cash;
        this.balance = balance;
        this.ID = ID;
        this.password = password;
    }
    public UserData(string userName, string ID, string password)
    {
        this.name = userName;
        this.ID = ID;
        this.password = password;

        this.cash = 115000;
        this.balance = 85000;
    }
}
