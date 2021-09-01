using System.Collections;
using UnityEngine;

public abstract class Traffic : MonoBehaviour
{
    [Header("Limits")]
    [SerializeField] protected Vector2 _minMaxSpeed = new Vector2(3f, 7f);
    [SerializeField] protected Vector2 _minMaxDelay = new Vector2(0f, 0f);
    [SerializeField] protected Vector2 _minMaxFrequency = new Vector2(1.5f, 3f);


    protected float _speed;
    protected Direction _direction;
    protected float _frequency;
    protected float _delay;

    [SerializeField] protected GameObject[] prefabs;

    protected enum Direction {
        RIGHT = 0,
        LEFT = 1
    }

    protected void Awake() {
        _speed = Random.Range(_minMaxSpeed.x, _minMaxSpeed.y);
        _delay = Random.Range(_minMaxDelay.x, _minMaxDelay.y);
        _frequency = Random.Range(_minMaxFrequency.x, _minMaxFrequency.y);
        _direction = (Direction) System.Math.Round(Random.value);
        _speed = _direction.Equals(Direction.RIGHT) ? _speed : -_speed;
    }

    protected IEnumerator Start() {
        while (_delay > 0) {
            _delay -= Time.deltaTime;
            yield return null;
        }

        while (true) {
            yield return WarningRoutine();
            Spawn();
            _frequency = Random.Range(_minMaxFrequency.x, _minMaxFrequency.y);
            yield return new WaitForSeconds(_frequency);
        }
    }

    protected virtual IEnumerator WarningRoutine() {
        yield break;
    }

    protected void Spawn() {
        float xSize = transform.localScale.x / 2;

        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        float multiplier = _direction.Equals(Direction.RIGHT) ? -1 : 1;
        var pos = new Vector3(
            multiplier * (xSize + prefab.transform.localScale.x /2),
            GetYPosition(prefab),
            transform.position.z
        );
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        TrafficObject to = go.GetComponent<TrafficObject>();
        to.SetSpeed(_speed);
        to.SetLimit(_speed > 0 ? xSize : -xSize);
        go.transform.parent = this.transform;
    }

    protected abstract float GetYPosition(GameObject prefab);
}
