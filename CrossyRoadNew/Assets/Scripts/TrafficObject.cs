using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrafficObject : MonoBehaviour
{
    protected float _speed;
    protected float _limit;

    public void SetSpeed(float speed) {
        if (speed > 0) {
            _speed = speed;
        } else {
            transform.Rotate(new Vector3(0, 180, 0));
            _speed = speed;
        }
    }

    protected void Update() {
        transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;

        if (_limit > 0 && transform.position.x - transform.parent.localScale.x * transform.localScale.x / 2 >= _limit ||
            _limit < 0 && transform.position.x + transform.parent.localScale.x * transform.localScale.x / 2 <= _limit) {
            Destroy(this.gameObject);
        }
    }

    public void SetLimit(float limit) {
        _limit = limit;
    }
}
