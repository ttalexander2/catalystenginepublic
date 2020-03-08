using System;
using Catalyst.Engine.Utilities;

namespace Catalyst.Engine
{

    #region Integer
    [Serializable]
    public class GuiInteger : Attribute
    {
        public int Min { get; private set; }
        public int Max { get; private set; }
        public bool HasRange { get; private set; }
        public GuiIntegerMode Mode { get; private set; }

        public GuiInteger(GuiIntegerMode mode, int min, int max)
        {
            this.Min = min;
            this.Max = max;
            this.Mode = mode;
            this.HasRange = true;
            if (mode == GuiIntegerMode.Percent)
            {
                throw new GuiAttributeException("Percent attribute type does not take a (min, max)!");
            }
        }

        public GuiInteger(int min, int max)
        {
            this.Min = min;
            this.Max = max;
            this.Mode = GuiIntegerMode.Default;
            this.HasRange = true;
        }

        public GuiInteger(GuiIntegerMode mode)
        {
            this.HasRange = false;
            this.Mode = mode;
            if(mode == GuiIntegerMode.Slider)
            {
                throw new GuiAttributeException("Slider attribute type must have a range (min, max)!");
            }
        }


        public GuiInteger()
        {
            this.HasRange = false;
            this.Mode = GuiIntegerMode.Default;
        }
    }

    [Serializable]
    public enum GuiIntegerMode
    {
        Default,
        Drag,
        Percent,
        Slider
    }

    #endregion

    #region Float
    [Serializable]
    public class GuiFloat : Attribute
    {
        public float Min { get; private set; }
        public float Max { get; private set; }
        public bool HasRange { get; private set; }
        public GuiFloatMode Mode { get; private set; }

        public GuiFloat(GuiFloatMode mode, float min, float max)
        {
            this.Min = min;
            this.Max = max;
            this.Mode = mode;
            this.HasRange = true;
            if (mode == GuiFloatMode.Angle)
            {
                throw new GuiAttributeException("Angle (min, max) is automatically in degrees (-360, 360)");
            }
        }

        public GuiFloat(float min, float max)
        {
            this.Min = min;
            this.Max = max;
            this.Mode = GuiFloatMode.Default;
            this.HasRange = true;
        }

        public GuiFloat(GuiFloatMode mode)
        {
            this.HasRange = false;
            this.Mode = mode;
            if (mode == GuiFloatMode.Slider)
            {
                throw new GuiAttributeException("Slider attribute type must have a range (min, max)");
            }
            if (mode == GuiFloatMode.SmallDrag)
            {
                throw new GuiAttributeException("Drag attribute type must have a range (min, max)");
            }
        }


        public GuiFloat()
        {
            this.HasRange = false;
            this.Mode = GuiFloatMode.Default;
        }
    }

    [Serializable]
    public enum GuiFloatMode
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
    public class GuiDouble : Attribute
    {
        public double Min { get; private set; }
        public double Max { get; private set; }
        public bool HasRange { get; private set; }

        public GuiDouble(double min, double max)
        {
            this.Min = min;
            this.Max = max;
            this.HasRange = true;
        }

        public GuiDouble()
        {
            this.HasRange = false;
        }
    }

    #endregion

    #region Vector2
    [Serializable]
    public class GuiVector2 : Attribute
    {
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        public bool HasRange { get; private set; }

        public GuiVector2()
        {
            HasRange = false;
        }

        public GuiVector2(Vector2 min, Vector2 max)
        {
            HasRange = true;
            this.Min = min;
            this.Max = max;
        }
    }

    #endregion

    #region Vector3
    [Serializable]
    public class GuiVector3 : Attribute
    {
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }
        public bool HasRange { get; private set; }

        public GuiVector3()
        {
            HasRange = false;
        }

        public GuiVector3(Vector3 min, Vector3 max)
        {
            HasRange = true;
            this.Min = min;
            this.Max = max;
        }
    }
    #endregion

    #region Vector4
    [Serializable]
    public class GuiVector4 : Attribute
    {
        public Vector4 Min { get; private set; }
        public Vector4 Max { get; private set; }
        public bool HasRange { get; private set; }

        public GuiVector4()
        {
            HasRange = false;
        }

        public GuiVector4(Vector4 min, Vector4 max)
        {
            HasRange = true;
            this.Min = min;
            this.Max = max;
        }
    }
    #endregion

    #region Color
    [Serializable]
    public class GuiColor : Attribute
    {
        public GuiColorMode Mode { get; private set; }
        public GuiColor()
        {
            Mode = GuiColorMode.RGBA;
        }

        public GuiColor(GuiColorMode mode)
        {
            this.Mode = mode;
        }
    }

    public enum GuiColorMode
    {
        RGB,
        RGBA
    }


    #endregion

    #region Label
    [Serializable]
    public class GuiLabel : Attribute
    {
        public string Label { get; private set; }
        public GuiLabel()
        {
            Label = null;
        }

        public GuiLabel(string label_string)
        {
            Label = label_string;
        }
    }
    #endregion

    #region String

    [Serializable]
    public class GuiString : Attribute
    {
        public string Hint { get; private set; }
        public bool HasHint { get; private set; }

        public GuiString()
        {
            this.HasHint = false;
        }

        public GuiString(string hint)
        {
            this.Hint = hint;
            this.HasHint = true;
        }
    }

    #endregion

    #region Boolean

    [Serializable]
    public class GuiBoolean : Attribute
    {
        public GuiBoolean()
        {
        }
    }

    #endregion

    #region Enum
    [Serializable]
    public class GuiEnum : Attribute
    {
        public GuiEnum()
        {
        }
    }
    #endregion

    #region EntitySelector
    [Serializable]
    public class GuiEntitySelector : Attribute
    {
        public GuiEntitySelector()
        {
        }
    }
    #endregion

    #region Exception

    [Serializable]
    public class GuiAttributeException : Exception
    {
        public GuiAttributeException()
        {

        }

        public GuiAttributeException(string message) :
            base(String.Format("Immediate Attribute Exception: {0}", message))
        {

        }
    }

    #endregion
}
