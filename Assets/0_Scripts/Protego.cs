using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protego : BasicSpell
{
    public override void Initialize(Transform wandTransform)
    {
        transform.position = wandTransform.position;
        transform.eulerAngles = new Vector3(0, wandTransform.eulerAngles.y, 0);
    }
}
