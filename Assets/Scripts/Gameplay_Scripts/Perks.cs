using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Create Perk")]
public class Perks : ScriptableObject
{
    public int perkID;
    public string description;
    public Sprite icon;
    public int perkCost;

    //public Method to set the values in the Perks UI
    public void SetValues(GameObject PerkDisplayObject, GameControl gameControl)
    {
        if (PerkDisplayObject)
        {
            PerkDisplay PD = PerkDisplayObject.GetComponent<PerkDisplay>();
            PerkSelect PS = PerkDisplayObject.GetComponent<PerkSelect>();

            if(PS)
            {
                PS.perkName.text = name;
                if (PS.perkIcon)
                {
                    PS.perkIcon.sprite = icon;
                }
                if(PS.perkCost)
                {
                    PS.perkCost.text = perkCost.ToString();
                }
            }

            if(PD)
            {
                PD.perkName.text = name;
                if(PD.perkDescription)
                {
                    PD.perkDescription.text = description;
                }
                if(PD.perkCost)
                {
                    PD.perkCost.text = perkCost.ToString();
                }
            }
        }
    }

    //Check if the player is able to get the perk.
    public bool CheckPerks(GameControl gameControl)
    {
        //check if the player is the right level.
        if (gameControl.playerLevel < perkCost)
        {
            return false;
        }
        //otherwise they can enable this perk
        return true;
    }

    //Check if the player already has the perk
    public bool EnablePerk(GameControl gameControl)
    {
        //go through all the perks that the player currently has
        List<Perks>.Enumerator perks = gameControl.PlayerPerks.GetEnumerator();
        while (perks.MoveNext())
        {
            var CurrPerk = perks.Current;
            if (CurrPerk.name == this.name)
            {
                return true;
            }
        }
        return false;
    }

    //get new perk
    public bool GetPerk(GameControl gameControl)
    {
        if(gameControl.playerLevel > perkCost)
        {
            //reduce the perk cost from the player's level
            gameControl.playerLevel -= perkCost;
            //add to the list of perks.
            gameControl.PlayerPerks.Add(this);
            return true;
        }
        return false;
    }
}
