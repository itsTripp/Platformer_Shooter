using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl gameControl;

    [Header("Player Health")]
    public int playerMaxHealth = 5;
    public int playerCurrentHealth;

    [Header("Player Weapon Info")]
    public Weapon currentRightWeapon;
    public Weapon currentLeftWeapon;
    public int rightWeaponCurrentAmmo;
    public int leftWeaponCurrentAmmo;

    [Header("Player Experience")]
    [SerializeField]
    private int _playerXP = 0;
    public int playerXP
    {
        get { return _playerXP; }
        set
        {
            _playerXP = value;
            if (onXPChange != null)
            {
                onXPChange();
                ShowData("xp");
            }
        }
    }
    public int experienceToNextLevel;
    [SerializeField]
    private int _playerLvl = 0;
    public int playerLevel
    {
        get { return _playerLvl; }
        set
        {
            _playerLvl = value;
            if (onLevelChange != null)
            {
                onLevelChange();
                ShowData("level");
            }
        }
    }

    public Text txtPlayerLevel;
    public Text txtPlayerXP;
    public Text testText;


    [Header("Enabled Perks")]
    public List<GameObject> selectedPerks = new List<GameObject>();//Delete this if UI route works
    public List<Perks> m_PlayerPerks = new List<Perks>();
    public List<Perks> PlayerPerks
    {
        get { return m_PlayerPerks; }
        set
        {
            m_PlayerPerks = value;
            if(onPerkChange != null)
            {
                onPerkChange();
            }
        }
    }

    private void Start()
    {
        ShowData("level");
        ShowData("xp");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (gameControl == null)
        {
            gameControl = this;
        }
    }

    public void ShowData(string i)
    {
        if(i == "level")
        {
            txtPlayerLevel.text = _playerLvl.ToString();
        }
        if(i == "xp")
        {
           txtPlayerXP.text = playerXP.ToString();
        }
    }

    public delegate void OnPerkChange();
    public event OnPerkChange onPerkChange;

    public delegate void OnXPChange();
    public event OnXPChange onXPChange;

    public delegate void OnLevelChange();
    public event OnLevelChange onLevelChange;

    public void UpdateLevel(int amount)
    {
        playerLevel += amount;
        Debug.Log("Level is Firing");
    }
    public void UpdateXP(int amount)
    {
        playerXP += amount;
        Debug.Log("XP is Firing");
    }

    public void TestText(string text)
    {
        testText.text = text.ToString();
    }
}
