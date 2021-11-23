using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [System.Serializable]
    public class PlayerAttributes
    {
        public Attributes attribute;
        public int amount;

        public PlayerAttributes(Attributes attribute, int amount)
        {
            this.attribute = attribute;
            this.amount = amount;
        }
    }
}