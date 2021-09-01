using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour {
    [System.Serializable]
    public class TerrainColor {
        public TerrainType type;
        public Color color;
    }
    [System.Serializable]
    public class TerrainAmount {
        public TerrainType type;
        public int amount;
    }

    [SerializeField] private SpriteRenderer _prefab;
    [SerializeField] private TerrainColor[] _colors;
    [Header("Map Settings")]
    [SerializeField] private Vector2Int size;
    [SerializeField] private TerrainAmount[] _amounts;

    private SpriteRenderer[,] _spriteMap;
    private TerrainType[,] _terrainMap;

    private Dictionary<TerrainType, Color> _dicColors;
    private MapController _controller;

    private ProceduralMapSettings _settings;

    private void Awake() {
        InitializeDictionary();
        _settings = CreateMapSettings();
        _terrainMap = new TerrainType[_settings.size.x, _settings.size.y];
        CreateMap(_terrainMap);
    }

    private void Start() {
        _controller = new MapController(_settings, this);
        _controller.onComplete += OnMapGenerationComplete;
        StartCoroutine(_controller.Create(_terrainMap));
    }

    private void Update() {
        UpdateMap(_terrainMap);
    }

    private void OnMapGenerationComplete(TerrainType[,] map) {
        _controller.onComplete -= OnMapGenerationComplete;
        UpdateMap(map);
    }

    private ProceduralMapSettings CreateMapSettings() {
        ProceduralMapSettings settings = new ProceduralMapSettings();
        settings.size = size;
        settings.terrainAmounts = new Dictionary<TerrainType, int>();
        foreach(TerrainAmount ta in _amounts) {
            settings.terrainAmounts.Add(ta.type, ta.amount);
        }
        return settings;
    }

    private void InitializeDictionary() {
        _dicColors = new Dictionary<TerrainType, Color>();
        foreach (TerrainColor tc in _colors) {
            _dicColors.Add(tc.type, tc.color);
        }
    }

    public void UpdateMap(TerrainType[,] map) {
        for (int i = 0; i < map.GetLength(0); i++) {
            for (int j = 0; j < map.GetLength(1); j++) {
                _spriteMap[i,j].color = _dicColors[map[i, j]];
            }
        }
    }

    public void CreateMap(TerrainType[,] map) {
        _spriteMap = new SpriteRenderer[map.GetLength(0), map.GetLength(1)];
        for (int i = 0; i < map.GetLength(0); i++) {
            for (int j = 0; j < map.GetLength(1); j++) {
                SpriteRenderer spr = Instantiate(_prefab);
                spr.transform.position = new Vector3(i, j, 0);
                spr.transform.parent = transform;
                _spriteMap[i, j] = spr;
            }
        }
    }
}
