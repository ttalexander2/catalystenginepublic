using System;
using Chroma.Engine.Utilities;

namespace Chroma.Engine
{

    #region Integer
    [Serializable]
    public class ImmediateInteger : Attribute
    {
        public int Min { get; private set; }
        public int Max { get; private set; }
        public bool HasRange { get; private set; }
        public ImmediateIntegerMode Mode { get; private set; }

        public ImmediateInteger(ImmediateIntegerMode mode, int min, int max)
        {
            this.Min = min;
            this.Max = max;
            this.Mode = mode;
            this.HasRange = true;
            if (mode == ImmediateIntegerMode.Percent)
            {
                throw new ImmediateAttributeException("Percent attribute type does not take a (min, max)!");
            }
        }

        public ImmediateInteger(int min, int max)
        {
            this.Min = min;
            this.Max = max;
            this.Mode = ImmediateIntegerMode.Default;
            this.HasRange = true;
        }

        public ImmediateInteger(ImmediateIntegerMode mode)
        {
            this.HasRange = false;
            this.Mode = mode;
            if(mode == ImmediateIntegerMode.Slider)
            {
                throw new ImmediateAttributeException("Slider attribute type must have a range (min, max)!");
            }
        }


        public ImmediateInteger()
        {
            this.HasRange = false;
            this.Mode = ImmediateIntegerMode.Default;
        }
    }

    [Serializable]
    public enum ImmediateIntegerMode
    {
        Default,
        Drag,
        Percent,
        Slider
    }

    #endregion

    #region Float
    [Serializable]
    public class ImmediateFloat : Attribute
    {
        public float Min { get; private set; }
        public float Max { get; private set; }
        public bool HasRange { get; private set; }
        public ImmediateFloatMode Mode { get; private set; }

        public ImmediateFloat(ImmediateFloatMode mode, float min, float max)
        {
            this.Min = min;
            this.Max = max;
            this.Mode = mode;
            this.HasRange = true;
            if (mode == ImmediateFloatMode.Angle)
            {
                throw new ImmediateAttributeException("Angle (min, max) is automatically in degrees (-360, 360)");
            }
        }

        public ImmediateFloat(float min, float max)
        {
            this.Min = min;
            this.Max = max;
            this.Mode = ImmediateFloatMode.Default;
            this.HasRange = true;
        }

        public ImmediateFloat(ImmediateFloatMode mode)
        {
            this.HasRange = false;
            this.Mode = mode;
            if (mode == ImmediateFloatMode.Slider)
            {
                throw new ImmediateAttributeException("Slider attribute type must have a range (min, max)");
            }
            if (mode == ImmediateFloatMode.SmallDrag)
            {
                throw new ImmediateAttributeException("Drag attribute type must have a range (min, max)");
            }
        }


        public ImmediateFloat()
        {
            this.HasRange = false;
            this.Mode = ImmediateFloatMode.Default;
        }
    }

    [Serializable]
    public enum ImmediateFloatMode
    {
        Default,
        Scientific,
        Drag,
        Slider,
        Angle,
        Small,
        SmallDrag

    }

    #endregion

    #region Double
    [Serializable]
    public class ImmediateDouble : Attribute
    {
        public double Min { get; private set; }
        public double Max { get; private set; }
        public bool HasRange { get; private set; }

        public ImmediateDouble(double min, double max)
        {
            this.Min = min;
            this.Max = max;
            this.HasRange = true;
        }

        public ImmediateDouble()
        {
            this.HasRange = false;
        }
    }

    #endregion

    #region Vector2
    [Serializable]
    public class ImmediateVector2 : Attribute
    {
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        public bool HasRange { get; private set; }

        public ImmediateVector2()
        {
            HasRange = false;
        }

        public ImmediateVector2(Vector2 min, Vector2 max)
        {
            HasRange = true;
            this.Min = min;
            this.Max = max;
        }
    }

    #endregion

    #region Vector3
    [Serializable]
    public class ImmediateVector3 : Attribute
    {
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }
        public bool HasRange { get; private set; }

        public ImmediateVector3()
        {
            HasRange = false;
        }

        public ImmediateVector3(Vector3 min, Vector3 max)
        {
            HasRange = true;
            this.Min = min;
            this.Max = max;
        }
    }
    #endregion

    #region Vector4
    [Serializable]
    public class ImmediateVector4 : Attribute
    {
        public Vector4 Min { get; private set; }
        public Vector4 Max { get; private set; }
        public bool HasRange { get; private set; }

        public ImmediateVector4()
        {
            HasRange = false;
        }

        public ImmediateVector4(Vector4 min, Vector4 max)
        {
            HasRange = true;
            this.Min = min;
            this.Max = max;
        }
    }
    #endregion

    #region Color
    [Serializable]
    public class ImmediateColor : Attribute
    {
        public ImmediateColorMode Mode { get; private set; }
        public ImmediateColor()
        {
            Mode = ImmediateColorMode.RGBA;
        }

        public ImmediateColor(ImmediateColorMode mode)
        {
            this.Mode = mode;
        }
    }

    public enum ImmediateColorMode
    {
        RGB,
        RGBA
    }


    #endregion

    #region Label
    [Serializable]
    public class ImmediateLabel : Attribute
    {
        public string Label { get; private set; }
        public ImmediateLabel()
        {
            Label = null;
        }

        public ImmediateLabel(string label_string)
        {
            Label = label_string;
        }
    }
    #endregion

    #region String

    [Serializable]
    public class ImmediateString : Attribute
    {
        public string Hint { get; private set; }
        public bool HasHint { get; private set; }

        public ImmediateString()
        {
            this.HasHint = false;
        }

        public ImmediateString(string hint)
        {
            this.Hint = hint;
            this.HasHint = true;
        }
    }

    #endregion

    #region Boolean

    [Serializable]
    public class ImmediateBoolean : Attribute
    {
        public ImmediateBoolean()
        {
        }
    }

    #endregion

    #region Enum
    [Serializable]
    public class ImmediateEnum : Attribute
    {
        public ImmediateEnum()
        {
        }
    }
    #endregion

    #region EntitySelector
    [Serializable]
    public class ImmediateEntitySelector : Attribute
    {
        public ImmediateEntitySelector()
        {
        }
    }
    #endregion

    #region Exception

    [Serializable]
    public class ImmediateAttributeException : Exception
    {
        public ImmediateAttributeException()
        {

        }

        public ImmediateAttributeException(string message) :
            base(String.Format("Immediate Attribute Exception: {0}", message))
        {

        }
    }

    #endregion
}
