using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : Traffic {
    protected override float GetYPosition(GameObject prefab) {
        return transform.position.y + transform.localScale.y / 2;
    }
}
