using UnityEngine;

namespace BCore
{
    public class PhysicsMotor2D : Motor2D
    {
        #region lifecycle methods

        protected virtual void FixedUpdate()
        {
            Vector2 velocity = TargetVelocity;

            // If we have no target velocity in the y axis, we respect the current
            // velocity of the reigidbody in the y axis.
            if (Mathf.Approximately(velocity.y, 0f))
            {
                velocity.y = rigidbody.velocity.y;
            }

            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = TargetAngularVelocity;
        }

        #endregion lifecycle methods
    }
}
