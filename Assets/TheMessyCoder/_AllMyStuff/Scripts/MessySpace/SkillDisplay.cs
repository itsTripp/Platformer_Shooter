using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/*****************************************************
// Author: The Messy Coder
// Date: January 2018
// Video Tutorial: https://youtu.be/6d7pmRE0T6c
// Please support the channel, facebook and on twitter
// YouTube:  www.YouTube.com/TheMessyCoder
// Facebook: www.Facebook.com/TheMessyCoder
// Twitter:  www.Twitter.com/TheMessyCoder
*****************************************************/

namespace Messyspace
{
    public class SkillDisplay : MonoBehaviour
    {
        //Get the Scriptable Object for Skill
        public Skills skill;
        //Get the UI components
        public Text skillName;
        public Text skillDescription;
        public Image skillIcon;
        public Text skillLevel;
        public Text skillXPNeeded;
        public Text skillAttribute;
        public Text skillAttrAmount;

        [SerializeField]
        private PlayerStats m_PlayerHandler;

        public List<Image> RequiredSkillsList = new List<Image>();

        // Use this for initialization
        void Start()
        {
            m_PlayerHandler = this.GetComponentInParent<PlayerHandler>().Player;
        }

        //Method to be used when you click the Skill icon
        public void GetSkill()
        {
            if (skill !=null && !skill.EnableSkill(m_PlayerHandler))
            {
                if(skill.CheckSkills(m_PlayerHandler) && skill.HasRequiredSkills(m_PlayerHandler))
                {
                    skill.GetSkill(m_PlayerHandler);
                }
            }
        }

        public void ShowRequiredList(Skills skillz)
        {
            ClearLabels(skillz);
            int i = 0;
            int x = skillz.RequiredSkills.Count;

            List<Image>.Enumerator ReqImages = RequiredSkillsList.GetEnumerator();
            while (ReqImages.MoveNext())
            {
                if (i >= x)
                    return;

                Skills CurrentSkill = skillz.RequiredSkills[i];

                ReqImages.Current.GetComponent<Image>().overrideSprite = CurrentSkill.Icon;

                i++;
            }
        }

        public void ClearLabels(Skills skillz)
        {
            List<Image>.Enumerator ReqImages = RequiredSkillsList.GetEnumerator();
            while (ReqImages.MoveNext())
            {
                //clears the icon for the displayer
                ReqImages.Current.GetComponent<Image>().overrideSprite = null;
            }
        }

        public void BtnShowValues()
        {
            //get the current button pressed by using unityengine.eventsystem
            SkillSelect button = EventSystem.current.currentSelectedGameObject.GetComponent<SkillSelect>();
            skill = button.skill;
            ShowRequiredList(skill);
            //show skill values on start
            if (skill)
                skill.SetValues(this.gameObject, m_PlayerHandler);
        }


    }
}

