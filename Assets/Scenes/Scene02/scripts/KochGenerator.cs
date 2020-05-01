using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochGenerator : MonoBehaviour
{
    protected enum _axis
    {
        XAxis,
        YAxis,
        ZAxis
    }
    [SerializeField]
    protected _axis axis = new _axis();

    protected enum _initiator
    {
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octagon
    }

    public struct LineSegment
    {
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }
        public Vector3 Direction { get; set; }
        public float Length { get; set; }
    }

    [SerializeField]
    protected _initiator initiator = new _initiator();

    [SerializeField]
    protected AnimationCurve _generator;
    protected Keyframe[] _keys;

    protected int _generationCount;

    protected int _initiatorPointerAmount;

    private Vector3[] _initiatorPoint;
    private Vector3 _rotateVector;
    private Vector3 _rotateAxis;
    private float _initialRotation;
    [SerializeField]
    protected float _initiatorSize;

    protected Vector3[] _position;
    protected Vector3[] _targetPosition;
    private List<LineSegment> _lineSegment;

    private void Awake()
    {
        GetInitiatorPoints();
        _position = new Vector3[_initiatorPointerAmount + 1];
        _targetPosition = new Vector3[_initiatorPointerAmount + 1];
        _lineSegment = new List<LineSegment>();
        _keys = _generator.keys;

        _rotateVector = Quaternion.AngleAxis(_initialRotation, _rotateAxis) * _rotateVector;

        for (int i = 0; i < _initiatorPointerAmount; i++)
        {
            _position[i] = _rotateVector * _initiatorSize;
            _rotateVector = Quaternion.AngleAxis(360 / _initiatorPointerAmount, _rotateAxis) * _rotateVector;
        }
        _position[_initiatorPointerAmount] = _position[0];
        _targetPosition = _position;
    }

    protected void KochGenerate(Vector3[] positions, bool outwards, float generatorMultplier)
    {
        //create line segment
        _lineSegment.Clear();
        for (int i = 0; i < positions.Length - 1; i++)
        {
            LineSegment line = new LineSegment();
            line.StartPosition = positions[i];
            if (i == positions.Length - 1)
            {
                line.EndPosition = positions[0];
            }
            else
            {
                line.EndPosition = positions[i + 1];
            }
            line.Direction = (line.EndPosition - line.StartPosition).normalized;
            line.Length = Vector3.Distance(line.EndPosition, line.StartPosition);
            _lineSegment.Add(line);
        }
        //add the line segment points to a point array
        List<Vector3> newPos = new List<Vector3>();
        List<Vector3> targetPos = new List<Vector3>();

        for(int i = 0; i < _lineSegment.Count; i++)
        {
            newPos.Add(_lineSegment[i].StartPosition);
            targetPos.Add(_lineSegment[i].StartPosition);

            for(int j = 1; j < _keys.Length - 1; j++)
            {
                float moveAmount = _lineSegment[i].Length * _keys[j].time;
                float heightAmount = (_lineSegment[i].Length * _keys[j].value) * generatorMultplier;
                Vector3 movePos = _lineSegment[i].StartPosition + (_lineSegment[i].Direction * moveAmount);
                Vector3 Dir;
                if (outwards)
                {
                    Dir = Quaternion.AngleAxis(-90, _rotateAxis) * _lineSegment[i].Direction;
                }
                else
                {
                    Dir = Quaternion.AngleAxis(90, _rotateAxis) * _lineSegment[i].Direction;
                }
                newPos.Add(movePos);
                targetPos.Add(movePos + (Dir * heightAmount));
            }
        }
        newPos.Add(_lineSegment[0].StartPosition);
        targetPos.Add(_lineSegment[0].StartPosition);
        _position = new Vector3[newPos.Count];
        _targetPosition = new Vector3[targetPos.Count];
        _position = newPos.ToArray();
        _targetPosition = targetPos.ToArray();

        _generationCount++;
    }

    private void OnDrawGizmos()
    {
        GetInitiatorPoints();
        _initiatorPoint = new Vector3[_initiatorPointerAmount];

        _rotateVector = Quaternion.AngleAxis(_initialRotation, _rotateAxis) * _rotateVector;
        //_rotateVector = new Vector3(0, 0, 1);
        //_rotateAxis = new Vector3(0, 1, 0);

        for(int i = 0; i < _initiatorPointerAmount; i++)
        {
            _initiatorPoint[i] = _rotateVector * _initiatorSize;
            _rotateVector = Quaternion.AngleAxis(360 / _initiatorPointerAmount, _rotateAxis) * _rotateVector;
        }

        for (int i = 0; i < _initiatorPointerAmount; i++)
        {
            Gizmos.color = Color.white;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            if (i < _initiatorPointerAmount - 1)
            {
                Gizmos.DrawLine(_initiatorPoint[i], _initiatorPoint[i + 1]);
            } else
            {
                Gizmos.DrawLine(_initiatorPoint[i], _initiatorPoint[0]);
            }
        }
    }

    private void GetInitiatorPoints()
    {
        switch (initiator)
        {
            case _initiator.Triangle:
                _initiatorPointerAmount = 3;
                _initialRotation = 0;
                break;
            case _initiator.Square:
                _initiatorPointerAmount = 4;
                _initialRotation = 45;
                break;
            case _initiator.Pentagon:
                _initiatorPointerAmount = 5;
                _initialRotation = 36;
                break;
            case _initiator.Hexagon:
                _initiatorPointerAmount = 6;
                _initialRotation = 30;
                break;
            case _initiator.Heptagon:
                _initiatorPointerAmount = 7;
                _initialRotation = 25.71428f;
                break;
            case _initiator.Octagon:
                _initiatorPointerAmount = 8;
                _initialRotation = 22.5f;
                break;
            default:
                _initiatorPointerAmount = 3;
                _initialRotation = 0;
                break;
        };

        switch (axis)
        {
            case _axis.XAxis:
                _rotateVector = new Vector3(1, 0, 0);
                _rotateAxis = new Vector3(0, 0, 1);
                break;

            case _axis.YAxis:
                _rotateVector = new Vector3(0, 1, 0);
                _rotateAxis = new Vector3(1, 0, 0);
                break;

            case _axis.ZAxis:
                _rotateVector = new Vector3(0, 0, 1);
                _rotateAxis = new Vector3(0, 1, 0);
                break;

            default:
                _rotateVector = new Vector3(0, 1, 0);
                _rotateAxis = new Vector3(1, 0, 0);
                break;

        };
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
