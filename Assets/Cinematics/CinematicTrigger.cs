using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool hasBeenTriggered;
        private void OnTriggerEnter(Collider other)
        {
            if (!hasBeenTriggered && other.gameObject.tag == "Player")
            {
                hasBeenTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
