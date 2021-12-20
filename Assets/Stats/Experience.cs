using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Statistics
{
    public class Experience : MonoBehaviour, ISavable
    {
        [SerializeField] float experiencePoints = 0;
        public float Xp { get { return experiencePoints; } }

        public event Action OnXpAwarded;


        public void GainExperience(float xp)
        {
            experiencePoints += xp;
            OnXpAwarded();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}

