using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class MovingPlatformModel : PlatformModel
    {
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private float _speed;

        public Vector3 StartPosition { get => _startPosition; set => _startPosition = value; }
        public Vector3 EndPosition { get => _endPosition; set => _endPosition = value; }
        public float Speed { get => _speed; set => _speed = value; }

        public MovingPlatformModel(int score, Vector3 start, Vector3 end, float speed) : base(score)
        {
            _startPosition = start;
            _endPosition = end;
            _speed = speed;
        }
    }
}


