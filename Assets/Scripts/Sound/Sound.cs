using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private Vector3 _Position;

    private string _Owner;

    private float _Intensity;

    private float _Range;

    // Constructor
    public Sound(Vector3 position, string owner, float intensity, float range)
    {
        _Position = position;
        _Owner = owner;
        _Intensity = intensity;
        _Range = range;
    }


    public Vector3 position
    {
        get
        {
            return _Position;
        }
        set
        {
            _Position = value;
        }
    }

    public float intensity
    {
        get
        {
            return _Intensity;
        }
        set
        {
            _Intensity = value;
        }
    }

    public float range
    {
        get
        {
            return _Range;
        }
        set
        {
            _Range = value;
        }
    }

    public string owner
    {
        get
        {
            return _Owner;
        }
        set
        {
            _Owner = value;
        }
    }
}
