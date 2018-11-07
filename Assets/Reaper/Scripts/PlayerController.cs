using UnityEngine;
using BCore;

namespace Reaper
{
    [RequireComponent(typeof(PhysicsMotor2D))]
    public class PlayerController : MonoBehaviour
    {
        #region static properties

        public static PlayerController Instance { get; private set; }

        #endregion static properties

        #region properties

        [Header("Movement")]
        public float movementSpeed = 5f;
        public Vector2 damageReflectionForce = new Vector2(5f, 3f);

        [Header("Misc")]
        public float damageDuration = 0.15f;
        public float recoveryDuration = 0.5f;

        [Header("Debug")]
        public Soul debugSoul;

        public Soul ActiveSoul { get; private set; }
        public PhysicsMotor2D Motor { get; private set; }

        #endregion properties

        #region private properties

        private bool _activatingSoul = false;
        private Vector2 _damageVelocity = Vector2.zero;
        private float _damageTimeRemaining = 0f;
        private float _recoveryDurationRemaining = 0f;

        #endregion private properties

        #region lifecycle methods

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("There appear to be more than one player objects in this scene, this is an error state.");
            }

            Instance = this;

            Motor = GetComponent<PhysicsMotor2D>();

            if (debugSoul != null)
            {
                ActiveSoul = debugSoul;
                ActiveSoul.Initialize(this);
            }

            _activatingSoul = false;
        }

        private void Update()
        {
            if (_recoveryDurationRemaining > 0f)
            {
                if (_damageTimeRemaining > 0f)
                {
                    _damageTimeRemaining -= Time.deltaTime;
                    if (_damageTimeRemaining < (damageDuration / 2f))
                    {
                        _damageVelocity.y = 0f;
                    }
                    Motor.TargetVelocity = _damageVelocity;
                }
                else
                {
                    Motor.TargetVelocity = Vector2.zero;
                }
                _recoveryDurationRemaining -= Time.deltaTime;

                return;
            }

            HandleInput();
            HandleActiveSoul();
        }

        #endregion lifecycle methods

        #region event methods

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                var enemy = collision.gameObject.GetComponent<EnemyController>();
                // TODO: Damage player from enemy data
                // Knock back player
                Vector2 velocity = damageReflectionForce;
                velocity.x *= transform.right.x * -1;

                _damageVelocity = velocity;
                _damageTimeRemaining = damageDuration;
                _recoveryDurationRemaining = recoveryDuration;
            }
        }

        #endregion event methods

        #region private methods

        private void HandleInput()
        {
            Vector2 velocity = Vector2.zero;
            velocity.x = Input.GetAxis("Horizontal") * movementSpeed;

            if (velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            else if (velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

            Motor.TargetVelocity = velocity;
        }

        private void HandleActiveSoul()
        {
            if (ActiveSoul == null)
            {
                return;
            }

            if (Input.GetButtonDown("ActivateSoul"))
            {
                if (ActiveSoul.ShouldActivate())
                {
                    ActivateSoul();
                }
            }

            if (_activatingSoul)
            {
                if (ActiveSoul.ShouldDeactivate())
                {
                    DeactivateSoul();
                }
                else
                {
                    ActiveSoul.Update();
                }
            }

            if (Input.GetButtonUp("ActivateSoul"))
            {
                DeactivateSoul();
            }
        }

        private void ActivateSoul()
        {
            if (!_activatingSoul)
            {
                ActiveSoul.Activated();
                _activatingSoul = true;
            }
        }

        private void DeactivateSoul()
        {
            if (_activatingSoul)
            {
                ActiveSoul.Deactivated();
                _activatingSoul = false;
            }
        }

        #endregion private methods
    }
}