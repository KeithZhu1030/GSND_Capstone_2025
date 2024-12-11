using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.IK
{
    /// <summary>  Process the weight by checking the Look At Angle of the Animator / </summary>
    [System.Serializable]
    [AddTypeMenu("Animal/Weight Mode")]
    public class WeightAnimalMode : WeightProcessor
    {
        [Tooltip("Exclude these modes. Meaning if the Character is NOT on these modes then the weight is set to 1")]
        public bool exclude = false;
        [Tooltip("Modes to check if the animal is on. Weight will be set to 1")]
        public List<ModeID> Modes = new();

        [Tooltip("Exclude these abilities. Meaning if the Character is NOT on these abilities then the weight is set to 1")]
        public bool excludeAbilities = false;

        [Tooltip("Abilities to check if the animal is on. Weight will be set to 1")]
        public List<int> Abilities = new();


        private List<int> modes = new();
        private ICharacterAction character;

        private float modeWeight = 0;
        public override void OnEnable(IKSet set, Animator anim)
        {
            modes = Modes.ConvertAll(x => x.ID); //Convert to Ints

            if (anim.TryGetComponent(out character))
            {
                character.ModeStart += OnModeStart;
                character.ModeEnd += OnModeEnd;
            }
            else
            {
                Active = false;
                Debug.LogWarning("The Mode weight processor requires an Animal Controller. Disabling it");
            }
        }

        private void OnModeEnd(int mode, int ability)
        {
            Debug.Log($"mode {mode} ability {ability}");
            modeWeight = 0;
        }

        private void OnModeStart(int mode, int ability)
        {
            Debug.Log($"mode {mode} ability {ability}");

            modeWeight = modes.Contains(mode) ? 1 : 0;
            if (exclude) modeWeight = 1 - modeWeight;

            if (Abilities.Count > 0) //Check for abilities too
            {
                modeWeight *= (Abilities.Contains(ability) ? 1 : 0);
                if (excludeAbilities) modeWeight = 1 - modeWeight;
            }
        }

        public override void OnDisable(IKSet set, Animator anim)
        {
            if (character != null) character.ModeStart -= OnModeStart;
        }

        public override float Process(IKSet set, float weight) => weight * modeWeight;
    }
}
