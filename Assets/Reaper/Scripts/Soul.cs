using UnityEngine;

namespace Reaper
{
    public abstract class Soul : ScriptableObject
    {
        #region public methods

        /// <summary>
        /// Setup the soul once it's become associated to a player character.
        /// </summary>
        /// <param name="player">
        /// The player controller this soul has been attached too.
        /// </param>
        public abstract void Initialize(PlayerController player);

        /// <summary>
        /// Determine if the soul can be activated based on custom tests. Defaults
        /// to returning true, in other words a soul can always be activated by
        /// default and it's up to implementations to override the tests.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if the soul can be activated,
        /// <c>false</c> if the soul cannot be activated.
        /// </returns>
        public virtual bool ShouldActivate()
        {
            return true;
        }

        /// <summary>
        /// Determine if the soul should no longer be considered as activated,
        /// which will end up having Deactivated called and Update to no longer
        /// be called. By default, this method always returns false, and it's
        /// up to the soul's implementation whether it should deactivate early.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if the soul should become deactivated (without button 
        /// interaction) <c>false</c> if it should remain activated.
        /// </returns>
        public virtual bool ShouldDeactivate()
        {
            return false;
        }

        /// <summary>
        /// Activated is called when the player presses the ActivateSoul button.
        /// This is a setup method to prepare for running the souls effects, and
        /// or if the soul does something simple like fire a projectile that can
        /// be done here.
        /// </summary>
        public abstract void Activated();

        /// <summary>
        /// While the soul is activated, this method will be called from the players
        /// update lifecycle method, so we can do any continuous updating here.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Deactivated is called when the user stops pressing the ActivateSoul
        /// ability, this can be used for teardown. Or simply do nothing.
        /// </summary>
        public abstract void Deactivated();

        #endregion public methods
    }
}