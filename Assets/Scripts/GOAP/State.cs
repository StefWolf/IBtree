using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    private List<(string, object)> _property;

    public State(List<(string, object)> property)
    {
        _property = property;
    }

    public void Set(string key, object value)
    {
        if (value is bool || value is int || value is float || value is double) {
            _property.Add((key,value));
        }
        else
        {
            throw new ArgumentException("Value must be a boolean or numeric type.");
        }
    }

    public object Get(string key) {
        if(key is string)
        {
            foreach ((string, object) s in _property)
            {
                if (s.Item1 == key)
                {
                    return s.Item2;
                }
            }
        }
        return null; 
    }

    public List<(string, object)> GetState() {
        return _property;
    }
}
