using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Statistics
{
    public class XpDisplay : MonoBehaviour
    {
        Experience xpComp;


        void Awake()
        {
            xpComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }

        void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", xpComp.Xp);
        }

    }
}
