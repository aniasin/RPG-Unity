using RPG.Control;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ObstacleCulling : MonoBehaviour
    {
        GameObject player;
        Transform obstruction;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, 
                (player.gameObject.transform.position + Vector3.up * 1) - transform.position, out hit, 10))
            {
                if (hit.collider.transform.gameObject.tag != "Player")
                {
                    if (obstruction)
                    {
                        obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                                                UnityEngine.Rendering.ShadowCastingMode.On;
                    }
                    obstruction = hit.transform;
                    obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                        UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }                    
            }
        }
    }
}
