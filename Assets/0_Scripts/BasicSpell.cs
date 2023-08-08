using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpell : MonoBehaviour
{
    public virtual void Initialize(Transform wandTransform)
    {
        transform.position = wandTransform.position;
        transform.rotation = wandTransform.rotation;
    }
}
