using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTrack : Traffic {
    [SerializeField] private MeshRenderer[] _meshes = null;
    [SerializeField] private Material _blackMat = null;
    [SerializeField] private Material _redMat = null;

    protected override IEnumerator WarningRoutine() {
        for (int i = 0; i < 6; i++) {
            foreach (MeshRenderer mr in _meshes) {
                mr.material = i % 2 == 0 ? _redMat : _blackMat;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    protected override float GetYPosition(GameObject prefab) {
        return transform.position.y + transform.localScale.y / 2 + prefab.transform.localScale.y / 2;
    }
}
