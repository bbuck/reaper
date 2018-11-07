using UnityEngine;

namespace Reaper
{
    public class EnemyController : MonoBehaviour
    {
        #region properties

        [Header("Base Stats")]
        public int health = 1;
        public int damage = 1;

        [Header("Soul")]
        public int soulLevel = 1;
        public SoulType soulType = SoulType.None;

        public int CurrentHealth { get; private set; }

        #endregion properties

        #region lifecycle methods

        protected virtual void Awake()
        {
            if (soulType == SoulType.None)
            {
                Debug.LogError(string.Format("There is no soul set for \"{0}\"!", transform.name));
            }

            CurrentHealth = health;
        }

        #endregion lifecycle methods
    }
}
