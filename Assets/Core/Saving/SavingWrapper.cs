using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float FadeInTime = 0.2f;
        const string defaultSaveFile = "save";

        void Awake()
        {
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(FadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                EraseSave();
            }
        }
        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void EraseSave()
        {
            GetComponent<SavingSystem>().Erase(defaultSaveFile);
        }

    }
}
