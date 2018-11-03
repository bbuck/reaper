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

        [Header("Debug")]
        public Soul debugSoul;

        public Soul ActiveSoul { get; private set; }
        public PhysicsMotor2D Motor { get; private set; }

        #endregion properties

        #region private properties

        private bool ActivatingSoul { get; set; }

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

            ActivatingSoul = false;
        }

        private void Update()
        {
            HandleInput();
            HandleActiveSoul();
        }

        #endregion lifecycle methods

        #region private methods

        private void HandleInput()
        {
            Vector2 velocity = Vector2.zero;
            velocity.x = Input.GetAxis("Horizontal") * movementSpeed;

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

            if (ActivatingSoul)
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
            if (!ActivatingSoul)
            {
                ActiveSoul.Activated();
                ActivatingSoul = true;
            }
        }

        private void DeactivateSoul()
        {
            if (ActivatingSoul)
            {
                ActiveSoul.Deactivated();
                ActivatingSoul = false;
            }
        }

        #endregion private methods
    }
}