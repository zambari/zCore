using UnityEngine;
//v.02 ANGLE FFS
//v.03 smoothtime3d
//v.04 dampedTargeter
//v.04 b smoothing constructor
//v.04 c detlacutfof smaller

/// <summary>
/// A collection of Mathf.SmoothDamp wrappers
/// </summary>


[System.Serializable]
public class Damper
{
    public float deltaCutoff = 0.001f;
    public float currentValue;
    [SerializeField] protected float _targetValue;
    public Damper(float smoothing, float deltaCutoffPoint)
    {
        deltaCutoff = deltaCutoffPoint;
        smoothTime = smoothing;
    }
    public Damper(float smoothing)
    {
        smoothTime = smoothing;
    }
    public Damper()
    {

    }
    public float targetValue
    {
        get { return _targetValue; }
        set
        {
            _targetValue = value;
            motionFinished = false;
        }
    }
    public float smoothTime = 0.1f;
    public float velocity;
    public void InitializeValue(float value)
    {
        targetValue = value;
        currentValue = targetValue;
        velocity = 0;
        motionFinished = true;
    }
    public float UpdatedValue()
    {
        //currentValue = Mathf.SmoothDamp(currentValue, targetValue, ref velocity, smoothTime);
        currentValue = Mathf.SmoothDamp(currentValue, targetValue, ref velocity, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime); //unscaled


        if (Mathf.Abs(targetValue - currentValue) < deltaCutoff) motionFinished = true;
        return currentValue;
    }
    public bool motionFinished { get; protected set; }
}



[System.Serializable]
public class DamperLerp : Damper
{
    new public float UpdatedValue()
    {
        currentValue = Mathf.Lerp(currentValue, targetValue, smoothTime * Time.deltaTime);
        if (Mathf.Abs(targetValue - currentValue) < deltaCutoff) motionFinished = true;
        return currentValue;
    }
}
[System.Serializable]
public class DamperAngle : Damper
{
    new public float UpdatedValue()
    {
        //currentValue = Mathf.SmoothDampAngle(currentValue, targetValue, ref velocity, smoothTime);
        currentValue = Mathf.SmoothDampAngle(currentValue, targetValue, ref velocity, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
        if (Mathf.Abs(targetValue - currentValue) < deltaCutoff) motionFinished = true;
        return currentValue;
    }

}

/// <summary>
/// An aggregate of 3 dampers, handles 3D motion
/// </summary>


[System.Serializable]
public class Damper3D
{
    public Damper xDamp;
    public Damper yDamp;
    public Damper zDamp;
    public Damper3D()
    {
        xDamp = new Damper();
        yDamp = new Damper();
        zDamp = new Damper();
    }
    public void InitializeValue(Vector3 value)
    {
        xDamp.InitializeValue(value.x);
        yDamp.InitializeValue(value.y);
        zDamp.InitializeValue(value.z);
    }

    public float smoothTime
    {
        get { return xDamp.smoothTime; }
        set
        {
            xDamp.smoothTime = value;
            yDamp.smoothTime = value;
            zDamp.smoothTime = value;
        }
    }

    public Vector3 smoothTime3D
    {
        get { return new Vector3(xDamp.smoothTime, yDamp.smoothTime, zDamp.smoothTime); }
        set
        {
            xDamp.smoothTime = value.x;
            yDamp.smoothTime = value.y;
            zDamp.smoothTime = value.z;
        }
    }

    public bool motionFinished
    {
        get
        {
            return xDamp.motionFinished && yDamp.motionFinished && zDamp.motionFinished;
        }
    }
    public Vector3 UpdatedValue()
    {
        return new Vector3(xDamp.UpdatedValue(), yDamp.UpdatedValue(), zDamp.UpdatedValue());
    }

    public Vector3 targetValue
    {
        get { return new Vector3(xDamp.targetValue, yDamp.targetValue, zDamp.targetValue); }
        set
        {
            xDamp.targetValue = value.x;
            yDamp.targetValue = value.y;
            zDamp.targetValue = value.z;
        }
    }
    public Vector3 currentValue
    {
        get { return new Vector3(xDamp.currentValue, yDamp.currentValue, zDamp.currentValue); }
        set
        {
            xDamp.currentValue = value.x;
            yDamp.currentValue = value.y;
            zDamp.currentValue = value.z;
        }
    }
    public Vector3 velocity
    {
        get { return new Vector3(xDamp.velocity, yDamp.velocity, zDamp.velocity); }
        set
        {
            xDamp.velocity = value.x;
            yDamp.velocity = value.y;
            zDamp.velocity = value.z;
        }
    }

    public float deltaCutoff
    {
        get { return xDamp.deltaCutoff; }
        set
        {
            xDamp.deltaCutoff = value;
            yDamp.deltaCutoff = value;
            zDamp.deltaCutoff = value;
        }
    }
}


/// <summary>
/// An aggregate of 2 dampers, handles 2D motion
/// </summary>


[System.Serializable]
public class Damper2D
{
    public Damper xDamp;
    public Damper yDamp;
    public Damper2D()
    {
        xDamp = new Damper();
        yDamp = new Damper();
    }
    public void InitializeValue(Vector2 value)
    {
        xDamp.InitializeValue(value.x);
        yDamp.InitializeValue(value.y);
    }

    public float smoothTime
    {
        get { return xDamp.smoothTime; }
        set
        {
            xDamp.smoothTime = value;
            yDamp.smoothTime = value;
        }
    }
    public Vector3 smoothTime2D
    {
        get { return new Vector2(xDamp.smoothTime, yDamp.smoothTime); }
        set
        {
            xDamp.smoothTime = value.x;
            yDamp.smoothTime = value.y;
        }
    }
    public bool motionFinished
    {
        get
        {
            return xDamp.motionFinished && yDamp.motionFinished;
        }
    }
    public Vector2 UpdatedValue()
    {
        return new Vector2(xDamp.UpdatedValue(), yDamp.UpdatedValue());
    }

    public Vector2 targetValue
    {
        get { return new Vector2(xDamp.targetValue, yDamp.targetValue); }
        set
        {
            xDamp.targetValue = value.x;
            yDamp.targetValue = value.y;
        }
    }
    public Vector2 currentValue
    {
        get { return new Vector2(xDamp.currentValue, yDamp.currentValue); }
        set
        {
            xDamp.currentValue = value.x;
            yDamp.currentValue = value.y;
        }
    }

    public float deltaCutoff
    {
        get { return xDamp.deltaCutoff; }
        set
        {
            xDamp.deltaCutoff = value;
            yDamp.deltaCutoff = value;

        }
    }
}
// a version that avoids a jump between 0 and 360 for rotation

/*
[System.Obsolete]
[System.Serializable]
public class Damper3DRotation : Damper3D
{
    new public Vector3 UpdatedValue()
    {
        return new Vector3(xDamp.UpdatedValue() % 360, yDamp.UpdatedValue() % 360, zDamp.UpdatedValue() % 360);
    }

    float checkRotation(float i)
    {
        if (i == 0) return 360;
        if (i < 180) i += 360;
        else
              if (i > 540) i -= 360;
        return i;
    }


    new public Vector3 targetValue
    {
        //get { return new Vector3(xDamp.targetValue%360,yDamp.targetValue%360,zDamp.targetValue%360); }
        get { return new Vector3(xDamp.targetValue, yDamp.targetValue, zDamp.targetValue); }
        set
        {
            value.x = checkRotation(value.x);
            value.y = checkRotation(value.y);
            value.z = checkRotation(value.z);
            xDamp.targetValue = value.x;
            yDamp.targetValue = value.y;
            zDamp.targetValue = value.z;
        }
    }
}*/
//old ver end
[System.Serializable]
public class Damper3DAngle
{
    public void InitializeValue(Vector3 value)
    {
        xDampA.InitializeValue(value.x);
        yDampA.InitializeValue(value.y);
        zDampA.InitializeValue(value.z);
    }

    public float smoothTime
    {
        get { return xDampA.smoothTime; }
        set
        {
            xDampA.smoothTime = value;
            yDampA.smoothTime = value;
            zDampA.smoothTime = value;
        }
    }
    public Vector3 smoothTime3D
    {
        get { return new Vector3(xDampA.smoothTime, yDampA.smoothTime, zDampA.smoothTime); }
        set
        {
            xDampA.smoothTime = value.x;
            yDampA.smoothTime = value.y;
            zDampA.smoothTime = value.z;
        }
    }

    public bool motionFinished
    {
        get
        {
            return xDampA.motionFinished && yDampA.motionFinished && zDampA.motionFinished;
        }
    }
    public Vector3 UpdatedValue()
    {
        return new Vector3(xDampA.UpdatedValue(), yDampA.UpdatedValue(), zDampA.UpdatedValue());
    }
    public Damper3DAngle()
    {
        xDampA = new DamperAngle();
        yDampA = new DamperAngle();
        zDampA = new DamperAngle();
    }

    DamperAngle xDampA;
    DamperAngle yDampA;
    DamperAngle zDampA;


    public Vector3 targetValue
    {
        //get { return new Vector3(xDamp.targetValue%360,yDamp.targetValue%360,zDamp.targetValue%360); }
        get { return new Vector3(xDampA.targetValue, yDampA.targetValue, zDampA.targetValue); }
        set
        {

            xDampA.targetValue = value.x;
            yDampA.targetValue = value.y;
            zDampA.targetValue = value.z;
        }
    }
}

[System.Serializable]
public class DampedTargetter
{
    public Damper damper = new Damper();
    public float smoothTime { get { return damper.smoothTime; } set { damper.smoothTime = value; } }
    public float baseValue = 1;
    [Range(0, 1)]
    public float randomAmount = 0.4f;
    public Vector2 timeRandomRange = new Vector2(0.2f, 2.6f);
    float GetTimeOfNextChange()
    {
        return Time.time + timeRandomRange.RandomFromRange();
    }
    float timeOfNextChange;
    public float targetValue { get { return damper.targetValue; } set { damper.targetValue = value; } }
    [Range(0, 1)]
    public float toNextChange;
    public float UpdatedValue()
    {
        if (Time.time >= timeOfNextChange)
        {
            targetValue = GetNextValue();
            timeOfNextChange = GetTimeOfNextChange();
        }
        toNextChange = (timeOfNextChange - Time.time) / timeRandomRange.y;
        return damper.UpdatedValue();
    }
    public float ZeroCenteredValue()
    {
        return damper.UpdatedValue() * 2 - 1;
    }
    float GetNextValue()
    {
        return baseValue.Randomize(randomAmount);
    }
}