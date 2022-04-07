using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EpicTortoiseStudios
{
    public class Flash_CoRoutine : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI flashTextComponent;
        [SerializeField]
        private string message;
        [SerializeField]
        private float timer;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(TextFlash());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator TextFlash()
        {
            while (true)
            {
                flashTextComponent.text = message;
                yield return new WaitForSeconds(timer);
                flashTextComponent.text = "";
                yield return new WaitForSeconds(timer);
            }
        }
    }
}
