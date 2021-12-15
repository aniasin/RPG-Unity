using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


namespace RPG.SceneManagement
{    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneIndex = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationId portalId;
        [SerializeField] float fadeOutTime = 1;
        [SerializeField] float fadeInTime = 2;
        [SerializeField] float fadeWaitTime = 3.5f;


        enum DestinationId { A, B, C, D, E }
        bool hasBeenTriggered;

        void OnTriggerEnter(Collider other)
        {
            if (sceneIndex < 0)
            {
                Debug.LogError("Scene hasn't been assigned in " + gameObject.name);
                return;
            }
            if (!hasBeenTriggered && other.gameObject.tag == "Player")
            {
                hasBeenTriggered = true;
                StartCoroutine(SceneTransition(sceneIndex));
            }
        }

        IEnumerator SceneTransition(int sceneIndex)
        {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            yield return SceneManager.LoadSceneAsync(sceneIndex);

            UpdatePlayer(FindOtherPortal());

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            Destroy(this.gameObject);
        }

        Portal FindOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this || portal.portalId != portalId) continue;
                return portal;             
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
