using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCreator : MonoBehaviour
{
    public const int TERRAIN_SIDE = 2;
    public GameObject[] prefabs;

    private int _zIndex = -3;

    private Queue<GameObject> _queue;
    private GameView _view;
    private int limit;

    private void Awake() {
        _view = FindObjectOfType<GameView>();
        _queue = new Queue<GameObject>();
        limit = 9;
        _view.onScoreIncreased += OnScoreIncreased;
        CreateTerrains(10);
        CreateTerrains(10);

        //quando chegar no 9, deleta os 10 primeiros
        //e cria 10 na frente.
        //quando chegar no 18, deleta os 10 primeiros
        //e cria 10 na frente.
    }

    private void OnScoreIncreased(int score) {
        if (score >= limit) {
            limit += 10;
            RemoveTerrains(10);
            CreateTerrains(10);
        }
    }

    public void CreateTerrains(int amount) {       
        for (int i = 0; i < amount; i++) {
            int r = 0;
            if (_zIndex > 0) {
                r = UnityEngine.Random.Range(0, prefabs.Length);
            }
            GameObject terrain = Instantiate(prefabs[r]);
            terrain.name = string.Format("{0} - {1}", _zIndex, prefabs[r].name);
            terrain.transform.position = new Vector3(0, terrain.transform.position.y, _zIndex * TERRAIN_SIDE);
            _zIndex++;
            _queue.Enqueue(terrain);
        }
    }

    public void RemoveTerrains(int amount) {
        for (int i = 0; i < amount; i++) {
            Destroy(_queue.Dequeue());
        }
    }
}
