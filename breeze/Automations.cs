using System;
using System.Collections.Generic;

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

    public delegate void AutomationFinishHandler(object sender);
        
    public abstract class Automation<T>
    {
        public AutomationFinishHandler OnFinish;
        public bool Loop = false;
        public double Speed = 1.0;
        protected double Time = 0.0;
        protected List<object> Clients;
        protected List<T> Values;
        protected List<int> Pos;
        protected List<InterpolationMethod> InterpolationMethods;
        protected int CurrentPos = 0;
        public T CurrentValue;
        protected bool FActive;

        public bool Active
        {
            get { return FActive; }
            set
            {
                // (Un)subscribe from/to core's animation timer
                if (value && !FActive)
                    BreezeCore.OnAnimate += Update;
                if (!value && FActive)
                    BreezeCore.OnAnimate -= Update;
                FActive = value;
            }
        }

        public Automation(T startValue, bool active = true)
        {
            Clients = new List<object>();
            Values = new List<T>();
            Values.Add(startValue);
            Pos = new List<int>();
            InterpolationMethods = new List<InterpolationMethod>();
            Active = active;
        }

        public void AddPoint(int pos, T value, InterpolationMethod intMethod)
        {
            Pos.Add(pos);
            Values.Add(value);
            InterpolationMethods.Add(intMethod);
        }

        protected abstract void CalculateValue(double k);
        protected abstract void AutomateClient(object client); 

        public void Update(object sender, TimerEventArgs e)
        {
            Time += e.Interval * Speed;

            // Automating
            if (Time >= Pos[CurrentPos])
            {
                CurrentPos++;
                if (CurrentPos >= Pos.Count)
                {
                    if (Loop)
                    {
                        CurrentPos = 0;
                        Time -= Pos[Pos.Count - 1];
                    }
                    else
                    {
                        if (OnFinish != null)
                            OnFinish(this);
                        Active = false;
                    }
                }
            }

            // Calculating values and sending them to clients
            int prevPos;
            if (CurrentPos == 0)
                prevPos = 0;
            else
                prevPos = Pos[CurrentPos - 1];

            CalculateValue((Time - prevPos) / (Pos[CurrentPos] - prevPos));
            foreach (object client in Clients)
                AutomateClient(client);
        }

        ~Automation()
        {
            if (FActive)
                Active = false;
        }
    }
}

