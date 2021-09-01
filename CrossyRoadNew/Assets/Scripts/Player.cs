using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float _moveTime = 0.5f;
    [SerializeField] private float _jumpHeight = 1f;
    [SerializeField] private AnimationCurve _jumpCurve = null;

    private Coroutine _moveRoutine;

    public Action<Vector2Int> onMove;

    private void Update() {
        if (_moveRoutine != null) {
            return;
        }
        int h = GetAxis("Horizontal");
        int v = GetAxis("Vertical");

        if ((h == 0 && v == 0) || (h != 0 && v != 0)) {
            return;
        }

        Vector3 pos = transform.position + new Vector3Int(h, 0, v) * 2;
        if (!CanMove(pos)) {
            return;
        }
        if (onMove != null) {
            onMove(new Vector2Int(h, v));
        }
        _moveRoutine = StartCoroutine(MoveRoutine(pos));
    }

    private IEnumerator MoveRoutine(Vector3 endpos) {
        Vector3 startpos = transform.position;
        float count = 0;
        while (count <= _moveTime) {
            float t = count / _moveTime;
            transform.position = Vector3.Lerp(startpos, endpos, t) + Vector3.up * _jumpCurve.Evaluate(t) * _jumpHeight;
            yield return null;
            count += Time.deltaTime;
        }
        transform.position = endpos;
        _moveRoutine = null;
    }

    private bool CanMove(Vector3 pos) {
        Collider[] cs = Physics.OverlapSphere(pos, 0.5f);
        foreach (Collider c in cs) {
            if (c.CompareTag("Tree")) {
                return false;
            }
        }
        return true;
    }

    private int GetAxis(string which) {
        int i = 0;
        switch (which) {
            case "Horizontal":
            if (Input.GetKeyDown(KeyCode.D)) {
                i++;
            }
            if (Input.GetKeyDown(KeyCode.A)) {
                i--;
            }
            break;
            case "Vertical":
            if (Input.GetKeyDown(KeyCode.W)) {
                i++;
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                i--;
            }
            break;
        }
        return i;
    }
}
