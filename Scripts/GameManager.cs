using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Title,
        PlayingGame,
        GameOver
    }

    public GameState gameState;
    private Player player;    
    public List<GameObject> enemyCount;
  
    #region singleton
    private static GameManager instance = null;

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

    public static GameManager Instance
    {
        get
        {
            if (instance == null) return null;

            return instance;
        }
    }
    #endregion

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();        
    }

    private void Update()
    {
        UIManager.Instance.UIEnemyCountChange(enemyCount.Count);        
    }

    public void PlayerExpUp(int enemyExp)
    {
        player.acquiredEXP += enemyExp;
        UIManager.Instance.UIPlayerExpChange(player.acquiredEXP, player.requiredLevelUpEXP);

        if(player.acquiredEXP >= player.requiredLevelUpEXP)
        {
            player.PlayerLevelUp();                       
        }
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        UIManager.Instance.ShowGameOverUI(true);        
    }    
}
