using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour, ISavable
    {
        [SerializeField] float experiencePoints = 0;
        public float Xp { get { return experiencePoints; } }

        public void GainExperience(float xp)
        {
            experiencePoints += xp;
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

