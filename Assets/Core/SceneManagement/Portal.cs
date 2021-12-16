using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.SceneManagement
{    public class Portal : MonoBehaviour
    {
        enum DestinationId { A, B, C, D, E }

        [SerializeField] int sceneIndex = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationId portalId;
        [SerializeField] float fadeOutTime = 1;
        [SerializeField] float fadeInTime = 2;
        [SerializeField] float fadeWaitTime = 3.5f;

        bool hasBeenTriggered;

        SavingWrapper savingWrapper;

         void Start()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }

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
            SaveScene();

            yield return SceneManager.LoadSceneAsync(sceneIndex);

            LoadScene();
            UpdatePlayer(FindOtherPortal());

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            Destroy(this.gameObject);
        }

        Portal FindOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.portalId != portalId) continue;
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
        void SaveScene()
        {
            savingWrapper.Save();
        }
        void LoadScene()
        {
            savingWrapper.Load();
        }
    }  
}
