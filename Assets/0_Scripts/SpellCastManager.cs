using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Oculus.Voice;
using Meta.WitAi.Json;

public class SpellCastManager : MonoBehaviour
{
    [System.Serializable]
    public class SpellData
    {
        public BasicSpell spellPrefab;
        public bool spellMovement;
        public string spellName;
    }
    public List<SpellData> spellData;
    public MovementRecognizer movementRecognizer;
    [SerializeField] private InputActionProperty spellInput;
    [SerializeField] private BasicSpell basicSpell;
    [SerializeField] private Transform wandTransform;

    private void Update()
    {
        if (spellInput.action.WasPerformedThisFrame())
        {
            StartCasting();
        }
        else if (spellInput.action.WasReleasedThisFrame())
        {
            StopCasting();
        }
    }

    private void StartCasting()
    {
        movementRecognizer.StartMovement();
    }

    private void StopCasting()
    {
        string spellMovement = movementRecognizer.EndMovement();
        SpellData spellCast;
        if (spellMovement != null)
        {
            int spellCastIndex = spellData.FindIndex(x => x.spellName==spellMovement && x.spellMovement);
            if (spellCastIndex>=0)
            {
                spellCast= spellData[spellCastIndex];
            }

            else 
            {
                spellCast = spellData[0];
            }
        }

        else
        {
            spellCast = spellData[0];
        }

        BasicSpell spawnedSpell=Instantiate(spellCast.spellPrefab);
        spawnedSpell.Initialize(wandTransform);
    }
}
