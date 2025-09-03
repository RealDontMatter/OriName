using SO;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Components
{
    class InteractablesSpawner : MonoBehaviour
    {

        private HomeSettingsSO m_homeSettings;
        private GameObject m_player;
        [SerializeField] private Transform m_interactablesParentTransform;

        public void Initialize(HomeSettingsSO homeSettingsSO, GameObject player)
        {
            m_homeSettings = homeSettingsSO;
            m_player = player;

            SpawnObjects();
        }

        private void SpawnObjects()
        {
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

                    GameObject intObject = Instantiate(item.ToSpawn.Prefab, new Vector3(x, 0, z), Quaternion.identity, m_interactablesParentTransform);
                    IInteractable intComponent = intObject.GetComponent<IInteractable>();
                    intComponent.Initialize(m_player);
                }
            }
        }
    }
}
