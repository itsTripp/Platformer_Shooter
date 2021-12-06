using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace EpicTortoiseStudios
{
    public class Perk_Manager : MonoBehaviour
    {
        public static Perk_Manager perkManager;
        //public List<Perks> possiblePerks = new List<Perks>();
        public List<PerkSelect> perkSelect = new List<PerkSelect>();

        // Start is called before the first frame update
        void Start()
        {
            perkManager = this;
            //possiblePerks = Resources.LoadAll<Perks>("Perks").ToList();
            perkSelect = FindObjectsOfType<PerkSelect>().ToList();


            for (int i = 0; i < GameControl.gameControl.possiblePerks.Count; i++)
            {
                Perks temp = GameControl.gameControl.possiblePerks[i];
                int randomIndex = Random.Range(i, GameControl.gameControl.possiblePerks.Count);
                GameControl.gameControl.possiblePerks[i] = GameControl.gameControl.possiblePerks[randomIndex];
                GameControl.gameControl.possiblePerks[randomIndex] = temp;
            }

            for (int i = 0; i < perkSelect.Count; i++)
            {
                ///Perk 2 doesn't work with this route.
                perkSelect[i].perk = GameControl.gameControl.possiblePerks[i];
                perkSelect[i].perkIcon.sprite = GameControl.gameControl.possiblePerks[i].icon;
                perkSelect[i].perkName.text = GameControl.gameControl.possiblePerks[i].name;
                perkSelect[i].perkCost.text = GameControl.gameControl.possiblePerks[i].perkCost.ToString();
            }
        }

        public void NextLevel()
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
