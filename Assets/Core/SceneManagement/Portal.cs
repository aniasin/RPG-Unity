using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


namespace RPG.SceneManagement
{    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneIndex;
        [SerializeField] Transform spawnPoint;
        [SerializeField] int portalIndex;

        bool hasBeenTriggered;

        void OnTriggerEnter(Collider other)
        {
            if (!hasBeenTriggered && other.gameObject.tag == "Player")
            {
                hasBeenTriggered = true;
                StartCoroutine(SceneTransition(sceneIndex));
            }
        }

        IEnumerator SceneTransition(int sceneIndex)
        {
            DontDestroyOnLoad(gameObject);

            Scene currentScene = SceneManager.GetActiveScene();
            yield return SceneManager.LoadSceneAsync(sceneIndex);            

            UpdatePlayer(FindOtherPortal());

            print("Loaded Scene!");
            Destroy(this.gameObject);
        }

        Portal FindOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.portalIndex == portalIndex)
                {
                    return portal;
                }                
            }
            return null;
        }
        void UpdatePlayer(Portal portal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.transform.position);
            player.transform.rotation = portal.spawnPoint.transform.rotation;
        }

    }  
}
