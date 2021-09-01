using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController
{
    public Action<TerrainType[,]> onComplete;
    private ProceduralMapGenerator _generator;
    private ProceduralMapSettings _settings;

    private MapView _view;

    public MapController(ProceduralMapSettings settings, MapView view) {
        _settings = settings;
        _view = view;
        _generator = new ProceduralMapGenerator(settings, view as MonoBehaviour);
    }

    public IEnumerator Create(TerrainType[,] map) {
        _generator.onComplete += OnGeneratorComplete;
        yield return _generator.Create(map);
    }

    private void OnGeneratorComplete(TerrainType[,] map) {
        _generator.onComplete -= OnGeneratorComplete;
        onComplete?.Invoke(map);
    }
}
