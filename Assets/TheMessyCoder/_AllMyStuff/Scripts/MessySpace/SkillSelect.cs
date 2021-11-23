using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Messyspace
{
    public class SkillSelect : MonoBehaviour
    {

        //Get the Scriptable Object for Skill
        public Skills skill;
        //Get the UI components
        public Text skillName;
        public Image skillIcon;

        [SerializeField]
        private PlayerStats m_PlayerHandler;

        // Use this for initialization
        void Start()
        {
            m_PlayerHandler = this.GetComponentInParent<PlayerHandler>().Player;
            //listener for the XP change
            m_PlayerHandler.onXPChange += ReactToChange;
            //listener for the Level change
            m_PlayerHandler.onLevelChange += ReactToChange;
            //listener for the skill change
            m_PlayerHandler.onSkillChange += ReactToChange;

            if (skill)
                skill.SetValues(this.gameObject, m_PlayerHandler);

            EnableSkills();
        }

        public void EnableSkills()
        {
            //if the player has the skill already, then show it as enabled
            if (m_PlayerHandler && skill && skill.EnableSkill(m_PlayerHandler))
            {
                TurnOnSkillIcon();
            }
            //if the player has the skill already, then show it as enabled
            else if (m_PlayerHandler && skill && skill.CheckSkills(m_PlayerHandler))
            {
                this.GetComponent<Button>().interactable = true;
                this.transform.Find("IconParent").Find("Disabled").gameObject.SetActive(false);
            }
            else
            {
                TurnOffSkillIcon();
            }
        }

        private void OnEnable()
        {
            EnableSkills();
        }
        

        //Turn on the Skill Icon - stop it from being clickable and disable the UI elements that make it change colour
        private void TurnOnSkillIcon()
        {
            this.transform.Find("IconParent").Find("Available").gameObject.SetActive(false);
            this.transform.Find("IconParent").Find("Disabled").gameObject.SetActive(false);
        }

        //Turn off the Skill Icon so it cannot be used - stop it from being clickable and enable the UI elements that make it change colour
        private void TurnOffSkillIcon()
        {
            if (this.GetComponent<Button>())
            {
                this.transform.Find("IconParent").Find("Available").gameObject.SetActive(true);
                this.transform.Find("IconParent").Find("Disabled").gameObject.SetActive(true);
            }
        }

        //event for when listener is woken
        void ReactToChange()
        {
            StartCoroutine(SmallDelay(0.05f));
        }

        // skills are updating too quick for the listener so adding a delay Very Messy
        IEnumerator SmallDelay(float time)
        {
            yield return new WaitForSeconds(time);
            EnableSkills();
        }
    }
}
