using UnityEngine;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Canvas rootCanvas;
        public void UpdateHealthBar(float value)        
        {
            rootCanvas.enabled = value < 1 && value > 0;
            transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
        }
    }
}
