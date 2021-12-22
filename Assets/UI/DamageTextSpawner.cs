using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab;
        [SerializeField] float timeToDestroy = 5f;
        [SerializeField] Color textColor;

        public void SpawnText(float value)
        {
            DamageText instanceText = Instantiate(damageTextPrefab, transform);
            instanceText.GetComponentInChildren<Text>().color = textColor;
            instanceText.GetComponentInChildren<Text>().text = string.Format("{0:0}", value);

            Destroy(instanceText, timeToDestroy);
        }
    }

}