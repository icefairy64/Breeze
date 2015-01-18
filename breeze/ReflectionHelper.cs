using System;
using System.Reflection;
using System.Collections.Generic;

namespace Breeze
{
    public class BuildParams : Dictionary<String, object>
    {
    }

    public class ReflectionException : Exception
    {
        public ReflectionException(string msg) : base(msg)
        {
        }
    }

    public static class ReflectionHelper
    {
        public static void SetField(Type type, object inst, string name, object value)
        {
            FieldInfo field = type.GetField(name);
            if (field == null)
                throw new ReflectionException(String.Format("Could not find field called '{0}' in type '{1}'", name, type.Name));
            field.SetValue(inst, Convert.ChangeType(value, field.FieldType));
        }

        public static void SetProperty(Type type, object inst, string name, object value)
        {
            PropertyInfo prop = type.GetProperty(name);
            if (prop == null)
                throw new ReflectionException(String.Format("Could not find property called '{0}' in type '{1}'", name, type.Name));
            prop.SetValue(inst, Convert.ChangeType(value, prop.PropertyType), null);
        }

        public static void SetAnything(Type type, object inst, string name, object value)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                field.SetValue(inst, Convert.ChangeType(value, field.FieldType));
                return;
            }

            PropertyInfo prop = type.GetProperty(name);
            if (prop != null)
            {
                prop.SetValue(inst, Convert.ChangeType(value, prop.PropertyType), null);
                return;
            }

            throw new ReflectionException(String.Format("Could not find field or property called '{0}' in type '{1}'", name, type.Name));
        }

        public static object GetField(Type type, object inst, string name)
        {
            FieldInfo field = type.GetField(name);
            if (field == null)
                throw new ReflectionException(String.Format("Could not find field called '{0}' in type '{1}'", name, type.Name));
            return field.GetValue(inst);
        }

        public static object GetProperty(Type type, object inst, string name)
        {
            PropertyInfo prop = type.GetProperty(name);
            if (prop == null)
                throw new ReflectionException(String.Format("Could not find property called '{0}' in type '{1}'", name, type.Name));
            return prop.GetValue(inst, null);
        }

        public static object GetAnything(Type type, object inst, string name)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                return field.GetValue(inst);
            }

            PropertyInfo prop = type.GetProperty(name);
            if (prop != null)
            {
                return prop.GetValue(inst, null);
            }

            throw new ReflectionException(String.Format("Could not find field or property called '{0}' in type '{1}'", name, type.Name));
        }
    }
}

