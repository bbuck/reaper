using UnityEngine;

namespace BCore
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Motor2D : MonoBehaviour
    {
        #region properties

        public Vector2 TargetVelocity { get; set; }
        public float TargetAngularVelocity { get; set; }

        protected new Rigidbody2D rigidbody;

        #endregion properties

        #region lifecycle methods

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            TargetVelocity = Vector2.zero;
            TargetAngularVelocity = 0f;
        }

        #endregion lifecycle methods
    }
}