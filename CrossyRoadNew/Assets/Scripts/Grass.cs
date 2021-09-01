using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour {
    [SerializeField] private GameObject[] _prefabs = null;
    [SerializeField] private AnimationCurve _curve = null;

    private void Start() {
        InstantiateTrees();
    }

    private void InstantiateTrees() {
        int amountOfTrees = ((int) transform.localScale.x) / TerrainCreator.TERRAIN_SIDE;
        for (int i = 1; i < amountOfTrees; i++) {
            float percentage = i / (float) amountOfTrees;
            float odds = _curve.Evaluate(percentage);

            if (UnityEngine.Random.value > odds) {
                continue;
            }

            Vector3 pos = new Vector3(
                i * TerrainCreator.TERRAIN_SIDE - transform.localScale.x / 2,
                0,
                transform.position.z
            );

            if (pos.Equals(Vector3.zero) || pos.Equals(Vector3.forward)) {
                continue;
            }

            GameObject tree = Instantiate(_prefabs[UnityEngine.Random.Range(0, _prefabs.Length)]);
            tree.transform.position = pos;
            tree.transform.parent = this.transform;
        }
    }
}
