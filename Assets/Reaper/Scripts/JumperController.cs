using UnityEngine;
using BCore;

namespace Reaper
{
    [RequireComponent(typeof(PhysicsMotor2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class JumperController : EnemyController
    {
        #region properties

        [Header("Jumper Stats")]
        public float walkSpeed;
        public float inAirMovement;
        public float jumpForce;
        public int biteDamageModifier;

        [Header("Movement")]
        public float airTime = 0.35f;

        [Header("Grounding Tests")]
        public float groundedCastRadius = 0.1f;
        public LayerMask groundedLayer;

        [Header("Vision")]
        public BoxCollider2D lineOfSight;
        public BoxCollider2D bitingDistance;

        #endregion properties

        #region private properties

        private PhysicsMotor2D _motor;
        private Transform _base;
        private bool _inAir = false;
        private float _remainingAirTime = 0f;
        private bool _seeThePlayer = false;

        #endregion private properties

        #region lifecycle methods

        protected override void Awake()
        {
            base.Awake();

            _motor = GetComponent<PhysicsMotor2D>();
            _base = transform.Find("Base");
        }

        private void Update()
        {
            CheckGrounded();
            Move();
        }

        #endregion lifecycle methods

        #region event handlers

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (bitingDistance.IsTouching(other))
                {
                    // handle biting player
                }

                if (lineOfSight.IsTouching(other))
                {
                    _seeThePlayer = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (!lineOfSight.IsTouching(other))
                {
                    _seeThePlayer = false;
                }
            }
        }

        #endregion event handlers

        #region private methods

        private void Move()
        {
            var velocity = Vector2.zero;

            //_seeThePlayer = false;

            if (!_inAir && _seeThePlayer)
            {
                velocity.y = jumpForce;
                _remainingAirTime = airTime;
            }
            else if (_remainingAirTime > 0)
            {
                velocity.y = jumpForce;
                _remainingAirTime -= Time.deltaTime;
            }

            if (_inAir)
            {
                velocity.x = inAirMovement * transform.right.x;
            }

            _motor.TargetVelocity = velocity;
        }

        private void CheckGrounded()
        {
            _inAir = !Physics2D.OverlapCircle(_base.position, groundedCastRadius, groundedLayer);
        }

        #endregion private methods
    }
}