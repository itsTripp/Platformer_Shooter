using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EpicTortoiseStudios
{
    public class PerkSelect : MonoBehaviour
    {
        public Perks perk;
        public List<Perks> perkSelectList = new List<Perks>();
        public Text perkName;
        public Image perkIcon;
        public Text perkCost;

        // Start is called before the first frame update
        void Start()
        {
            
            GameControl.gameControl.onLevelChange += ReactToChange;
            GameControl.gameControl.onPerkChange += ReactToChange;

            perkSelectList = Resources.LoadAll<Perks>("Perks").ToList();
            
            if (perk)
            {
                perk.SetValues(gameObject, GameControl.gameControl);
            }
            EnablePerks();
        }
        public void EnablePerks()
        {
            //If the player has the perk already, then show it as enabled
            if (perk && perk.EnablePerk(GameControl.gameControl))
            {
                TurnOnPerkIcon();
            }
            //If the player has the perk already, then show it as enabled
            else if (perk && perk.CheckPerks(GameControl.gameControl))
            {
                this.GetComponent<Button>().interactable = true;
                this.transform.Find("Icon_Parent").Find("Disabled").gameObject.SetActive(false);
            }
            else
            {
                TurnOffPerkIcon();
            }
        }
        private void OnEnable()
        {
            EnablePerks();
        }

        //Turn on the Perk Icon - stop it from being clickable and disable the UI elements that make
        //it change color.
        private void TurnOnPerkIcon()
        {
            this.transform.Find("Icon_Parent").Find("Available").gameObject.SetActive(false);
            this.transform.Find("Icon_Parent").Find("Disabled").gameObject.SetActive(false);
        }

        //Turn off the Perk Icon so it cannot be used - stop it from being clickable and enable the 
        //UI elements that make it change color.
        private void TurnOffPerkIcon()
        {
            if (this.GetComponent<Button>())
            {
                this.transform.Find("Icon_Parent").Find("Available").gameObject.SetActive(true);
                this.transform.Find("Icon_Parent").Find("Disabled").gameObject.SetActive(true);
            }
        }

        //event for when listener is woken
        void ReactToChange()
        {
            StartCoroutine(SmallDelay(0.05f));
        }

        //skills are updating too quick for the listener so adding a delay
        IEnumerator SmallDelay(float time)
        {
            yield return new WaitForSeconds(time);
            EnablePerks();
        }
    }
}
