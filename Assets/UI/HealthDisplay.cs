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
            UpdateHealth();
        }

        void UpdateHealth()
        {
            GetComponent<Text>().text = healthComp.GetHealthPoints();
        }

    }
}

