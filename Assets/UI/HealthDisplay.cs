using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health healthComp;


        void Awake()
        {
            healthComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}%", healthComp.GetHealthPercentage());
        }

    }
}

