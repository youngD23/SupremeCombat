using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageAI : Damage
{
    internal override void Start() {
        base.Start();
        if (player.controlSetting != "cpu") { return; }

        player.gaurdValue = 1;
    }
    internal override void Update() {
        base.Update();
        if (player.controlSetting != "cpu") { return; }

        RecoverCheck();
    }
}
