using UnityEngine;

namespace Reaper
{
    [CreateAssetMenu(fileName = "New Jump Soul", menuName = "Souls/Jump")]
    public class JumpSoul : Soul
    {
        #region properties

        [Header("Jump Values")]
        public float jumpForce = 5f;
        public float maxJumpTime = 0.35f;

        [Header("Grounded Check")]
        public float overlapRadius = 0.1f;
        public LayerMask groundLayer;

        private PlayerController Player { get; set; }
        private Transform Base { get; set; }

        private float jumpTimeRemaining = 0f;

        #endregion properties

        #region override methods

        public override void Initialize(PlayerController player)
        {
            Player = player;
            Base = player.transform.Find("Base");
        }

        public override bool ShouldActivate()
        {
            bool result = Physics2D.OverlapCircle(Base.position, overlapRadius, groundLayer);
            Debug.Log(result);
            return result;
        }

        public override void Activated()
        {
            SetJumpVelocity();
            jumpTimeRemaining = maxJumpTime;
        }

        public override void Update()
        {
            if (jumpTimeRemaining > 0)
            {
                SetJumpVelocity();
                jumpTimeRemaining -= Time.deltaTime;
            }
        }

        public override bool ShouldDeactivate()
        {
            return jumpTimeRemaining < 0;
        }

        public override void Deactivated()
        {
            // empty
        }

        #endregion override methods

        #region private methods

        private void SetJumpVelocity()
        {
            Vector2 velocity = Player.Motor.TargetVelocity;
            velocity.y = jumpForce;
            Player.Motor.TargetVelocity = velocity;
        }

        #endregion private methods
    }
}