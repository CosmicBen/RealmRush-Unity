using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    
    [SerializeField] private int startingBalance = 150;
    private int currentBalance = 0;
    public int CurrentBalance { get { return currentBalance; } }

    [SerializeField] TextMeshProUGUI displayBalance;

    private void Awake()
    {
        SetBalance(startingBalance);
    }

    private void SetBalance(int value)
    {
        currentBalance = value;
        displayBalance.text = $"Gold: {currentBalance}";
    }

    public void Deposit(int amount)
    {
        SetBalance(currentBalance + Mathf.Abs(amount));
    }

    public void Withdraw(int amount)
    {
        SetBalance(currentBalance - Mathf.Abs(amount));

        if (currentBalance < 0)
        {
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
