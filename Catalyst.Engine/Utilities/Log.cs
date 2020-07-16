using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Catalyst.Engine.Utilities
{
    public static class Log
    {

        private static LogWriter _out = new LogWriter();
        public delegate void WriteFunction(char value);
        public static void SetWriteFunction(WriteFunction function)
        {
            _out.WriteHandler = function;
        }

        #region Write
        public static void WriteLine()
        {
            _out.WriteLine();
        }

        public static void WriteLine(bool value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(char value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(char[] buffer)
        {
            _out.WriteLine(buffer);
        }

        public static void WriteLine(char[] buffer, int index, int count)
        {
            _out.WriteLine(buffer, index, count);
        }

        public static void WriteLine(decimal value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(double value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(float value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(int value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(uint value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(long value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(ulong value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(Object value)
        {
            _out.WriteLine(value);
        }

        public static void WriteLine(String value)
        {
            _out.WriteLine(value);
        }


        public static void WriteLine(String format, Object arg0)
        {
            _out.WriteLine(format, arg0);
        }

        public static void WriteLine(String format, Object arg0, Object arg1)
        {
            _out.WriteLine(format, arg0, arg1);
        }

        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2)
        {
            _out.WriteLine(format, arg0, arg1, arg2);
        }
        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2, Object arg3, __arglist)
        {
            Object[] objArgs;
            int argCount;

            ArgIterator args = new ArgIterator(__arglist);

            //+4 to account for the 4 hard-coded arguments at the beginning of the list.
            argCount = args.GetRemainingCount() + 4;

            objArgs = new Object[argCount];

            //Handle the hard-coded arguments
            objArgs[0] = arg0;
            objArgs[1] = arg1;
            objArgs[2] = arg2;
            objArgs[3] = arg3;

            //Walk all of the args in the variable part of the argument list.
            for (int i = 4; i < argCount; i++)
            {
                objArgs[i] = TypedReference.ToObject(args.GetNextArg());
            }

            _out.WriteLine(format, objArgs);
        }

        public static void WriteLine(String format, params Object[] arg)
        {
            if (arg == null)                       // avoid ArgumentNullException from String.Format
                _out.WriteLine(format, null, null); // faster than Out.WriteLine(format, (Object)arg);
            else
                _out.WriteLine(format, arg);
        }

        public static void Write(String format, Object arg0)
        {
            _out.Write(format, arg0);
        }

        public static void Write(String format, Object arg0, Object arg1)
        {
            _out.Write(format, arg0, arg1);
        }
        public static void Write(String format, Object arg0, Object arg1, Object arg2)
        {
            _out.Write(format, arg0, arg1, arg2);
        }

        public static void Write(String format, Object arg0, Object arg1, Object arg2, Object arg3, __arglist)
        {
            Object[] objArgs;
            int argCount;

            ArgIterator args = new ArgIterator(__arglist);

            //+4 to account for the 4 hard-coded arguments at the beginning of the list.
            argCount = args.GetRemainingCount() + 4;

            objArgs = new Object[argCount];

            //Handle the hard-coded arguments
            objArgs[0] = arg0;
            objArgs[1] = arg1;
            objArgs[2] = arg2;
            objArgs[3] = arg3;

            //Walk all of the args in the variable part of the argument list.
            for (int i = 4; i < argCount; i++)
            {
                objArgs[i] = TypedReference.ToObject(args.GetNextArg());
            }

            _out.Write(format, objArgs);
        }

        public static void Write(String format, params Object[] arg)
        {
            if (arg == null)                   // avoid ArgumentNullException from String.Format
                _out.Write(format, null, null); // faster than Out.Write(format, (Object)arg);
            else
                _out.Write(format, arg);
        }

        public static void Write(bool value)
        {
            _out.Write(value);
        }

        public static void Write(char value)
        {
            _out.Write(value);
        }

        public static void Write(char[] buffer)
        {
            _out.Write(buffer);
        }

        public static void Write(char[] buffer, int index, int count)
        {
            _out.Write(buffer, index, count);
        }

        public static void Write(double value)
        {
            _out.Write(value);
        }

        public static void Write(decimal value)
        {
            _out.Write(value);
        }

        public static void Write(float value)
        {
            _out.Write(value);
        }

        public static void Write(int value)
        {
            _out.Write(value);
        }

        public static void Write(uint value)
        {
            _out.Write(value);
        }

        public static void Write(long value)
        {
            _out.Write(value);
        }

        public static void Write(ulong value)
        {
            _out.Write(value);
        }

        public static void Write(Object value)
        {
            _out.Write(value);
        }
        public static void Write(String value)
        {
            _out.Write(value);
        }
        #endregion

        internal class LogWriter : TextWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
            private bool _hasHandler = false;
            private WriteFunction _handler;
            internal WriteFunction WriteHandler
            {
                set
                {
                    _hasHandler = true;
                    _handler = value;

                }
            }

            public override void Write(char value)
            {
                if (_hasHandler)
                    _handler(value);
                else
                    Console.Write(value);
            }
        }
    }
}
