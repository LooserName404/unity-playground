using System;
using System.Collections;
using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private AnimationCurve _speedCurve;

    private Vector3 _offset;
    private Vector3 _initialPosition;

    public void SetTarget(Transform t) {
        _target = t;
        SetOffset();
    }

    private void SetOffset() {
        _offset = transform.position - _target.position;
    }

    private void Awake() {
        _initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        SetOffset();
    }

    private void Start() {
        StartCoroutine(Move());
    }

    private IEnumerator Move() {
        while (true) {
            var pos = new Vector3(_target.position.x, 0, _target.position.z) + _offset;
            if (pos - transform.position != Vector3.zero) {
                var cameraPos = Camera.main.WorldToViewportPoint(transform.position - _offset);
                var playerPos = Camera.main.WorldToViewportPoint(_target.position);
                var point = Vector3.Distance(cameraPos, playerPos) / 10;
                var lerp = Vector3.Lerp(cameraPos, playerPos, point);
                var speed = _speedCurve.Evaluate(point);
                var dist = lerp * speed * Time.deltaTime;
                
                transform.position += dist;
            }

            yield return null;
        }
    }
}
