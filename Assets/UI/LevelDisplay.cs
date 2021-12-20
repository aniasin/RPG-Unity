using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Statistics
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats statsComp;


        void Awake()
        {
            statsComp = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", statsComp.GetLevel());
        }
    }
}
