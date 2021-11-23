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
    [CreateAssetMenu (menuName = "RPG Generator/Player/Create Attribute")]
    public class Attributes : ScriptableObject
    {
        public string Description;
        public Sprite Thumbnail;
    }
}