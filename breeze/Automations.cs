using System;

namespace Breeze
{
    public enum InterpolationMethod
    {
        Linear
    }

    public static class AutomationHelper
    {
        public static double GetValue(InterpolationMethod method, double a, double b, double k)
        {
            double rk;
            switch (method)
            {
                case InterpolationMethod.Linear:
                    rk = k;
                    break;
                default: 
                    rk = k;
                    break;
            }

            return a + (b - a) * rk;
        }
    }

    public abstract class Automation
    {
        public Automation()
        {
        }
    }
}

