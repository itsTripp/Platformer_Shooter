using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EpicTortoiseStudios
{
    public class PerkDisplay : MonoBehaviour
    {
        public Perks perk;
        public Text perkName;
        public Text perkDescription;
        public Image perkIcon;
        public Text perkCost;

        // Start is called before the first frame update
        void Start()
        {

        }

        //Method to be used when you click the perk icon
        public void GetPerk()
        {
            if (perk != null && !perk.EnablePerk(GameControl.gameControl))
            {
                if (perk.CheckPerks(GameControl.gameControl))
                {
                    perk.GetPerk(GameControl.gameControl);
                }
            }
        }

        public void ButtonShowValues()
        {
            //get the current button pressed by using unityengine.eventsystem
            PerkSelect button = EventSystem.current.currentSelectedGameObject.GetComponent<PerkSelect>();
            perk = button.perk;
            //show perk values on start
            if (perk)
            {
                perk.SetValues(this.gameObject, GameControl.gameControl);
            }
        }
    }
}
