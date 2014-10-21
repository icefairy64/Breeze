using System;
using System.Collections.Generic;
using SDL2;

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

    public delegate void AutomationFinishHandler(BaseAutomation sender);

    public abstract class BaseAutomation
    {
        public AutomationFinishHandler OnFinish;
    }
        
    public abstract class Automation<T, C> : BaseAutomation
    {
        public bool Loop = false;
        public double Speed = 1.0;
        protected double Time = 0.0;
        protected List<C> Clients;
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

        public Automation(T startValue, bool active = false)
        {
            Clients = new List<C>();
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
        protected abstract void AutomateClient(C client); 

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
                        return;
                    }
                }
            }

            // Calculating current value and sending it to clients
            int prevPos;
            if (CurrentPos == 0)
                prevPos = 0;
            else
                prevPos = Pos[CurrentPos - 1];

            CalculateValue((Time - prevPos) / (Pos[CurrentPos] - prevPos));
            foreach (C client in Clients)
                AutomateClient(client);
        }

        public void AttachClient(C client)
        {
            Clients.Add(client);
        }

        public void DetachClient(C client)
        {
            Clients.Remove(client);
        }

        ~Automation()
        {
            if (FActive)
                Active = false;
        }
    }
}

namespace Breeze.Graphics
{
    public class AlphaAutomation : Automation<byte, Drawable>
    {
        public AlphaAutomation(byte startVal, bool active = false) : base(startVal, active)
        {
        }

        protected override void CalculateValue(double k)
        {
            CurrentValue = (byte)(Values[CurrentPos] + (Values[CurrentPos + 1] - Values[CurrentPos]) * k);
        }

        protected override void AutomateClient(Drawable client)
        {
            client.Alpha = CurrentValue;
        }
    }
}