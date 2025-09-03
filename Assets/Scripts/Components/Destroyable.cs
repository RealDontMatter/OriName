using UnityEngine;
using Utility;

namespace Components
{
    public class Destroyable : Interactable
    {
        [SerializeField] private Models.DestroyableData m_destroyableData;
        private Models.Destroyable m_destroyable => (Models.Destroyable)data;
        private bool m_isInteracting;
        private float m_clock;

        public bool IsInteracting => m_isInteracting;
        public float Progress => m_clock / m_destroyable.Data.HitDuration;

        public override void Interact()
        {
            m_isInteracting = true;
            m_clock = 0;

            if (m_destroyable.HitPoints <= 0)
            {
                m_isInteracting = false;
                CollectReward();
                return;
            }
        }
        public override bool CanInteract() => m_isInteracting == false;

        public override void Initialize(GameObject player)
        {
            base.Initialize(player);

            data = m_destroyableData.CreateDestroyable();
        }

        void Update()
        {
            if (!m_isInteracting) return;
            
            m_clock += Time.deltaTime;
            
            if (m_clock >= m_destroyable.Data.HitDuration)
            {
                m_clock = 0;
                m_destroyable.HitPoints--;
                m_isInteracting = false;
            }
            if (m_destroyable.HitPoints <= 0) CollectReward();
        }

        void CollectReward()
        {
            PlayerController playerCon = player.GetComponent<PlayerController>();
            playerCon.Inventory.AddItems(m_destroyable.Data.ItemsToDrop.DeepClone());
            Destroy(gameObject);
        }
    }
}
