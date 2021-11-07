using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int playerLevel = 0;
    public int playerExperience;
    public int experienceToNextLevel;

    public List<GameObject> selectedPerks = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(gameControl == null)
        {
            gameControl = this;
        }
    }
}
