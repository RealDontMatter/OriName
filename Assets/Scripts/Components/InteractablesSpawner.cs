using SO;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Components
{
    class InteractablesSpawner : MonoBehaviour
    {

        [SerializeField] private HomeSettingsSO m_homeSettings;
        [SerializeField] private Transform m_parentTransform;


        public List<IInteractable> SpawnObjects()
        {
            List<IInteractable> interactables = new();
            foreach (var item in m_homeSettings.StartingInteractables)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    Vector3 minPos = m_homeSettings.InteractableMinimumPosition;
                    Vector3 maxPos = m_homeSettings.InteractableMaximumPosition;
                    float radius = m_homeSettings.InteractableMinimumRadius;

                    float x, z;
                    bool foundInteractable;
                    int attempts = 0;
                    do
                    {
                        x = Random.Range(minPos.x, maxPos.x);
                        z = Random.Range(minPos.z, maxPos.z);
                        foundInteractable = false;

                        Collider[] colliders = Physics.OverlapSphere(new Vector3(x, 0, z), radius);
                        foreach (Collider collider in colliders)
                        {
                            if (collider.GetComponent<IInteractable>() != null) foundInteractable = true;
                        }
                        if (++attempts > 100)
                        {
                            Debug.LogWarning("Not enough space to spawn all interactables.");
                            break;
                        }

                    } while (foundInteractable);

                    if (attempts > 100) break;

                    GameObject intObject = Instantiate(item.ToSpawn.Prefab, new Vector3(x, 0, z), Quaternion.identity, m_parentTransform);
                    var interactableComponent = intObject.GetComponent<IInteractable>();
                    interactables.Add(interactableComponent);
                }
            }
            return interactables;
        }
    }
}
