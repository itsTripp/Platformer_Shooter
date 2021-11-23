using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public class PlayerStats : MonoBehaviour
    {

        [Header("Main Player Stats")]
        public string PlayerName;
        public int PlayerHP = 50; //baseline = 50, +5 each level

        [SerializeField]
        private int m_PlayerXP = 0;
        public int PlayerXP
        {
            get { return m_PlayerXP; }
            set
            {
                m_PlayerXP = value;

                //If we have subscribers, then tell them xp changed :)
                if (onXPChange != null)
                {
                    onXPChange();
                    ShowData("xp");
                }
            }
        }

        /* Set Listener for Player Level */
        [SerializeField]
        private int m_PlayerLevel = 1;
        public int PlayerLevel
        {
            get { return m_PlayerLevel; }
            set
            {
                m_PlayerLevel = value;

                //If we have subscribers, then tell them xp changed :)
                if (onLevelChange != null)
                {
                    onLevelChange();
                    ShowData("level");
                }
            }
        }

        public Text txtPlayerLevel;
        public Text txtPlayerXP;

        public void ShowData(string i)
        {
            if (i == "level")
                txtPlayerLevel.text = m_PlayerLevel.ToString();

            if (i == "xp")
                txtPlayerXP.text = m_PlayerXP.ToString();
        }

        public void Start()
        {
            ShowData("level");
            ShowData("xp");
        }

        [Header("Player Attributes")]
        public List<PlayerAttributes> Attributes = new List<PlayerAttributes>();

        [Header("Player Skills Enabled")]
        [SerializeField]
        private List<Skills> m_PlayerSkills = new List<Skills>();
        public List<Skills> PlayerSkills
        {
            get { return m_PlayerSkills; }
            set
            {
                m_PlayerSkills = value;

                //If we have subscribers, then tell them xp changed :)
                if (onSkillChange != null)
                    onSkillChange();
            }
        }

        //Delegates for listeners  
        public delegate void OnSkillChange();
        public event OnSkillChange onSkillChange;

        public delegate void OnXPChange();
        public event OnXPChange onXPChange;

        public delegate void OnLevelChange();
        public event OnLevelChange onLevelChange;


        //Just some temporary methods to help test
        public void UpdateLevel(int amount)
        {
            PlayerLevel += amount;
        }
        public void UpdateXP(int amount)
        {
            PlayerXP += amount;
        }


    }

}