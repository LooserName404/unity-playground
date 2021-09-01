using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asphalt : Traffic {
    protected override float GetYPosition(GameObject prefab) {
        return transform.position.y + transform.localScale.y / 2 + prefab.transform.localScale.y / 2;
    }
}
