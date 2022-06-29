using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour,
    IPlayerSkill_ReflectiveDamage,
    IPlayerSkill_DeathAura,
    IPlayerSkill_ManaBomb
{
    private int maxHp = 100;
    private int currHp = 100;
    private int maxMp = 50;
    private int currMp = 50;
    public int acquiredEXP = 0;
    public int requiredLevelUpEXP = 100;
    public byte playerLevel = 1;
    private Animator playerAnimator;   
    int reflectiveDamage_DAM = 2;      
    int deathAura_DAM = 1;
    public ParticleSystem particle_reflectiveDamage;
    public ParticleSystem particle_manaBomb;
    public ParticleSystem particle_deathAura;

    enum playerState
    {
        NormalMode,
        Die,
    }

    playerState _playerState;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        _playerState = playerState.NormalMode;
        UIManager.Instance.UIPlayerHpChange(maxHp, currHp);
        UIManager.Instance.UIPlayerMpChange(maxMp, currMp);

        StartCoroutine("IPlayerSkill_DeathAura.DeathAura");
        InvokeRepeating("PlayerRegenerateHpMp", 1f, 1f);        
    }

    void PlayerRegenerateHpMp()
    {
        if (currHp < maxHp) currHp += Mathf.RoundToInt(maxHp * 0.01f);
        if (currMp < maxMp) currMp += Mathf.RoundToInt(maxMp * 0.01f);        
        UIManager.Instance.UIPlayerHpChange(maxHp, currHp);
        UIManager.Instance.UIPlayerMpChange(maxMp, currMp);
    }

    public void OnDamage(int dam, Enemy enemyWhoAttacked)
    {
        if (_playerState == playerState.Die) return;

        currHp -= dam;
        transform.LookAt(enemyWhoAttacked.transform);
        int manaBombChance = Random.Range(0, 10);
        if (manaBombChance <= 2 && currMp >= maxMp * 0.1f ) ManaBomb(Mathf.RoundToInt(currMp * 0.1f));
        ReflectiveDamage(enemyWhoAttacked);
        UIManager.Instance.UIPlayerHpChange(maxHp, currHp);
        if (currHp <= 0) Die();
    }

    public void ReflectiveDamage(Enemy enemyWhoAttacked)
    {
        enemyWhoAttacked.OnDamage(reflectiveDamage_DAM);
        particle_reflectiveDamage.Play();
    }

    public void Die()
    {
        _playerState = playerState.Die;
        StopCoroutine("IPlayerSkill_DeathAura.DeathAura");
        playerAnimator.SetTrigger("Die");
        GameManager.Instance.GameOver();
        Invoke("GoTitleScene", 4f);        
    }

    public void PlayerLevelUp()
    {
        playerLevel++;
        
        maxHp = Mathf.RoundToInt(maxHp * 1.1f);
        currHp = maxHp;
        maxMp = Mathf.RoundToInt(maxMp * 1.1f);
        currMp = maxMp;

        acquiredEXP -= requiredLevelUpEXP;
        requiredLevelUpEXP = Mathf.RoundToInt(requiredLevelUpEXP * 1.1f);

        UIManager.Instance.UIPlayerLevelUp(playerLevel);
        UIManager.Instance.UIPlayerHpChange(maxHp, currHp);
        UIManager.Instance.UIPlayerMpChange(maxMp, currMp);
    }

    void GoTitleScene()
    {
        SceneManager.LoadScene("Title");
    }
    
    public void ManaBomb(int usedMana)
    {
        particle_manaBomb.Play();
        Collider[] cols = Physics.OverlapSphere(transform.position, 5f, 1 << 6);
        foreach (var col in cols)
        {
            col.GetComponent<Enemy>().OnDamage(usedMana);            
        }
        currMp -= usedMana;
        UIManager.Instance.UIPlayerMpChange(maxMp, currMp);
    }

    IEnumerator IPlayerSkill_DeathAura.DeathAura()
    {
        while(true)
        {           
            yield return new WaitForSeconds(1.5f);
        
            Collider[] cols = Physics.OverlapSphere(transform.position, 10f, 1 << 6);
            foreach (var col in cols)
            {
            col.GetComponent<Enemy>().OnDamage(deathAura_DAM);
            }
        }
    }
}
