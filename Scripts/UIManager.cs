using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image playerHpBar;
    public Image playerMpBar;
    public Image playerExpBar;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI playerHpText;
    public TextMeshProUGUI playerMpText;
    public GameObject gameOver;

    #region singleton
    private static UIManager instance = null;

    private void Awake()
    {
        if (instance == null)
        { 
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static UIManager Instance
    {
        get
        {
            if (instance == null) return null;

            return instance;
        }
    }
    #endregion


    public void UIPlayerHpChange(int maxHp, int currHp)
    {
        playerHpBar.fillAmount = (float)currHp / maxHp;
        playerHpText.text = $"{currHp} / {maxHp}";
    }

    public void UIPlayerMpChange(int maxMp, int currMp)
    {
        playerMpBar.fillAmount = (float)currMp / maxMp;
        playerMpText.text = $"{currMp} / {maxMp}";
    }
    
    public void UIPlayerLevelUp(byte level)
    {       
        playerLevelText.text = $"Player Level : {level}";
    }

    public void UIPlayerExpChange(int acquiredEXP, int requiredLevelUpEXP)
    {
        playerExpBar.fillAmount = (float)acquiredEXP / requiredLevelUpEXP;
    }

    public void UIEnemyCountChange(int count)
    {
        enemyCountText.text = $"Enemy Count : {count}";
    }
   
    public void ShowGameOverUI(bool setActive)
    {
        gameOver.SetActive(setActive);
    }
    
}
