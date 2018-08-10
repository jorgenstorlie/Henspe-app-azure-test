﻿using System; using System.Threading; using System.Threading.Tasks;  namespace Henspe.Core.Util {     public class TimerUtil     {         public delegate void OnTimerCompletedCallback();         private static OnTimerCompletedCallback onTimerCompletedCallback = null;          public TimerUtil()         {         }          static private void TimeEvent()         {             onTimerCompletedCallback();             onTimerCompletedCallback = null;         }          static public void StopTimer()         {             onTimerCompletedCallback = null;         }          static public bool Wait(double seconds, OnTimerCompletedCallback inputOnTimerCompletedCallback)         {             if (onTimerCompletedCallback != null)                 return false;              onTimerCompletedCallback = inputOnTimerCompletedCallback;             double milliseconds = seconds * 1000;             int millisecondsInt = (int)milliseconds;             PCLTimer _pageTimer = new PCLTimer(new Action(TimeEvent), millisecondsInt, -1);              return true;         }     }      public delegate void TimerCallback(object state);      public sealed class MyTimer : CancellationTokenSource, IDisposable     {         public MyTimer(TimerCallback callback, object state, int dueTime, int period)         {             Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>             {                 var tuple = (Tuple<TimerCallback, object>)s;                  while (true)                 {                     if (IsCancellationRequested)                         break;                      Task.Run(() => tuple.Item1(tuple.Item2));                     await Task.Delay(period);                 }              }, Tuple.Create(callback, state), CancellationToken.None,                 TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,                 TaskScheduler.Default);         }          protected override void Dispose(bool disposing)         {             if (disposing)                 Cancel();              base.Dispose(disposing);         }     }      public class PCLTimer     {         private MyTimer _timer;          private Action _action;          public PCLTimer(Action action, int dueTime, int period)         {             _action = action;              _timer = new MyTimer(PCLTimerCallback, null, dueTime, period);         }          private void PCLTimerCallback(object state)         {             _action.Invoke();         }     } } 