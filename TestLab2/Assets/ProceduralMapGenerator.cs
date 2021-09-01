using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMapGenerator {

    private ProceduralMapSettings _settings;

    private MonoBehaviour _mono;

    private Action _onContaminate;
    private int _contaminatedTerrains;

    public ProceduralMapGenerator(ProceduralMapSettings settings, MonoBehaviour mono) {
        _settings = settings;
        _mono = mono;
    }

    public Action<TerrainType[,]> onComplete;

    public IEnumerator Create(TerrainType[,] map) {
        if (_settings == null) {
            throw new NullReferenceException($"{this.GetType()} needs a ProceduralMapSettings to work.");
        }
        List<Vector2Int> spots = new List<Vector2Int>();
        yield return CreateTerrainRandomSpots(map, _settings, spots);
        yield return FillMapAccordingToRandomGrowthFactors(map, spots);

        onComplete?.Invoke(map);
    }

    private IEnumerator FillMapAccordingToRandomGrowthFactors(TerrainType[,] map, List<Vector2Int> spots) {
        _onContaminate += OnTerrainContaminated;
        _contaminatedTerrains = spots.Count;

        foreach (Vector2Int spot in spots) {
            int randomFrameDelay = UnityEngine.Random.Range(1, 4);
            _mono.StartCoroutine(GrowRoutine(map, spot, randomFrameDelay));
        }
        
        int matrixSize = map.GetLength(0) * map.GetLength(1);
        while (_contaminatedTerrains < matrixSize) {
            yield return null;
        }

        _onContaminate -= OnTerrainContaminated;
    }

    private void OnTerrainContaminated() {
        _contaminatedTerrains++;
    }

    private IEnumerator GrowRoutine(TerrainType[,] map, Vector2Int spot, int randomFrameDelay) {
        int radius = 1;
        do {
            
            _mono.StartCoroutine(ContaminateInACircle(map, spot, radius));
            radius++;
            for (int i = 0; i < randomFrameDelay; i++) {
                yield return null;
            }
        } while (radius < Mathf.Max(map.GetLength(0), map.GetLength(1)));
    }

    private void ContaminateSpot(TerrainType[,] map, TerrainType type, Vector2Int spot, Vector2Int center) {
        if (spot.x < 0 || spot.x >= map.GetLength(0) || spot.y < 0 || spot.y >= map.GetLength(1)) {
            return;
        }
        if (map[spot.x, spot.y] != TerrainType.None) {
            return;
        }
        
        map[spot.x, spot.y] = type;
        _onContaminate?.Invoke();

        if (spot.x > center.x) {
            ContaminateSpot(map, type, new Vector2Int(spot.x - 1, spot.y), center);
        } else if (spot.x < center.x) {
            ContaminateSpot(map, type, new Vector2Int(spot.x + 1, spot.y), center);
        }
        if (spot.y > center.y) {
            ContaminateSpot(map, type, new Vector2Int(spot.x, spot.y - 1), center);
        } else if (spot.y < center.y) {
            ContaminateSpot(map, type, new Vector2Int(spot.x, spot.y + 1), center);
        }
    }

    private IEnumerator CreateTerrainRandomSpots(TerrainType[,] map, ProceduralMapSettings settings, List<Vector2Int> spots) {
        foreach (KeyValuePair<TerrainType, int> kv in settings.terrainAmounts) {
            for (int i = 0; i < kv.Value; i++) {
                int x, y;
                do {
                    x = UnityEngine.Random.Range(0, settings.size.x);
                    y = UnityEngine.Random.Range(0, settings.size.y);
                } while (map[x, y] != TerrainType.None);
                map[x, y] = kv.Key;
                spots.Add(new Vector2Int(x, y));
            }
            yield return null;
        }
    }

    public IEnumerator ContaminateInACircle(TerrainType[,] map, Vector2Int spot, int radius) {
        TerrainType type = map[spot.x, spot.y];

        int d = (5 - radius * 4) / 4;
        int x = 0;
        int y = radius;
        while(UnityEngine.Random.Range(0, 2) != 1) {
            yield return new WaitForSeconds(0.25f);
        }

        do {
            // ensure index is in range before setting (depends on your map implementation)
            // in this case we check if the pixel location is within the bounds of the map before setting the pixel
            if (spot.x + x >= 0 && spot.x + x <= map.GetLength(0) - 1 && spot.y + y >= 0 && spot.y + y <= map.GetLength(1) - 1) ContaminateSpot(map, type, new Vector2Int(spot.x + x, spot.y + y), spot);
            if (spot.x + x >= 0 && spot.x + x <= map.GetLength(0) - 1 && spot.y - y >= 0 && spot.y - y <= map.GetLength(1) - 1) ContaminateSpot(map, type, new Vector2Int(spot.x + x, spot.y - y), spot);
            if (spot.x - x >= 0 && spot.x - x <= map.GetLength(0) - 1 && spot.y + y >= 0 && spot.y + y <= map.GetLength(1) - 1) ContaminateSpot(map, type, new Vector2Int(spot.x - x, spot.y + y), spot);
            if (spot.x - x >= 0 && spot.x - x <= map.GetLength(0) - 1 && spot.y - y >= 0 && spot.y - y <= map.GetLength(1) - 1) ContaminateSpot(map, type, new Vector2Int(spot.x - x, spot.y - y),spot);
            if (spot.x + y >= 0 && spot.x + y <= map.GetLength(0) - 1 && spot.y + x >= 0 && spot.y + x <= map.GetLength(1) - 1) ContaminateSpot(map, type, new Vector2Int(spot.x + y, spot.y + x), spot);
            if (spot.x + y >= 0 && spot.x + y <= map.GetLength(0) - 1 && spot.y - x >= 0 && spot.y - x <= map.GetLength(1) - 1) ContaminateSpot(map, type, new Vector2Int(spot.x + y, spot.y - x), spot);
            if (spot.x - y >= 0 && spot.x - y <= map.GetLength(0) - 1 && spot.y + x >= 0 && spot.y + x <= map.GetLength(1) - 1) ContaminateSpot(map, type, new Vector2Int(spot.x - y, spot.y + x), spot);
            if (spot.x - y >= 0 && spot.x - y <= map.GetLength(0) - 1 && spot.y - x >= 0 && spot.y - x <= map.GetLength(1) - 1) ContaminateSpot(map, type, new Vector2Int(spot.x - y, spot.y - x), spot);
            if (d < 0) {
                d += 2 * x + 1;
            } else {
                d += 2 * (x - y) + 1;
                y--;
            }
            x++;
        } while (x <= y);
    }
}