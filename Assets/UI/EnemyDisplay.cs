using RPG.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyDisplay : MonoBehaviour
    {
        Health healthComp;
        Fighter fighterComp;


        void Start()
        {
            fighterComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        void Update()
        {
            healthComp = fighterComp.Target;
            if (!healthComp)
            {
                GetComponent<Text>().text = "N/A";
                return;
            }
            GetComponent<Text>().text = String.Format("{0:0}%", healthComp.GetHealthPercentage());
        }

    }
}
