using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health healthComp;

        void Start()
        {
            healthComp = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            healthComp.onHealthChanged += UpdateHealth;
        }

        void UpdateHealth()
        {
            GetComponent<Text>().text = String.Format("{0:0}%", healthComp.GetHealthPercentage());
        }

    }
}

