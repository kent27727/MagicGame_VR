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
        public bool useVoice;
        public string voiceName;
    }
    public List<SpellData> spellData;
    public MovementRecognizer movementRecognizer;
    public AppVoiceExperience voiceRecognizer;
    [SerializeField] private InputActionProperty spellInput;
    [SerializeField] private BasicSpell basicSpell;
    [SerializeField] private Transform wandTransform;
    private string saidSpell;


    private void Start()
    {
        voiceRecognizer.VoiceEvents.OnPartialResponse.AddListener(SetSaidSpell);
    }

    

    private void Update()
    {
        if (spellInput.action.WasPerformedThisFrame())
        {
            StartCasting();
        }
        else if (spellInput.action.WasReleasedThisFrame())
        {
            StartCoroutine(StopCasting());
        }
    }

    private void StartCasting()
    {
        movementRecognizer.StartMovement();
        voiceRecognizer.Activate();
        saidSpell = "";
    }

    private IEnumerator StopCasting()
    {
        string spellMovement = movementRecognizer.EndMovement();
        voiceRecognizer.Deactivate();
        yield return new WaitForSeconds(1f);

        SpellData spellCast;

        //movement spell check
        if (spellMovement != null)
        {
            int spellCastIndex = spellData.FindIndex(x => x.spellName==spellMovement && x.spellMovement);
            if (spellCastIndex>=0)
            {
                spellCast= spellData[spellCastIndex];
                print("olaaaa");
            }

            else 
            {
                spellCast = spellData[0];
            }
        }

        //voice spell check
        else if (saidSpell != null && saidSpell != "")
        {
            int spellCastIndex = spellData.FindIndex(x => x.voiceName == saidSpell && x.useVoice);
            if (spellCastIndex >= 0)
            {
                spellCast = spellData[spellCastIndex];
                print("olaaaa");
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

    private void SetSaidSpell(WitResponseNode response)
    {
        string intentName = response["intents"][0]["name"].Value.ToString();
        saidSpell = intentName;
    }
}
