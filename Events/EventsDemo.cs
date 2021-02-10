using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventsDemo
{
    class program
    {
        public enum SignalColour{Red, Green}; // Enum to represent the signal state.

        public delegate void LightChangeHandler(SignalColour newColour); // Delegate used to handle signal change, takes argument of new signal colour, returns void.

        public class TrainSignal
        {
/*Event:*/  public event LightChangeHandler OnSignalChange; //This event is invoked when a Signal Change occurs  AKA: [PUBLISHER]

            public SignalColour signalColour {private set; get; } = SignalColour.Red; //Member var , stores the current Signal, set to Red as default

            public void SetSignal(SignalColour newSignalColor)
            {
                if(newSignalColor != signalColour) //Clarifies signal has actually changed, commits if true
                {
                    signalColour = newSignalColor;

                    Console.WriteLine("****************************");
                    Console.WriteLine("Signal now changing to " + (signalColour == SignalColour.Red ? "Red" : "Green") + "...");

                    OnSignalChange?.Invoke(signalColour);
                }
            }
        }

        public class WarningSiren
        {
            public void RegisterToSignal(TrainSignal signal)
            {
                signal.OnSignalChange += new LightChangeHandler(RespondToSignal);
            }

            private void RespondToSignal(SignalColour signal)
            {
                if(signal == SignalColour.Red)
                {
                    Console.Beep(10000, 2000);
                }
                else
                {
                    Console.Beep(5000, 2000);
                }
            }
        }


        public class Train                                                                                  //Note: This class can only be accessed via the train 'signal', all its own methods are private 
        {
            private void StartMoving() { Console.WriteLine("Train has started moving..."); }
            private void StopMoving() { Console.WriteLine("Train is coming to a stop...");}
        

            // This function registers the private method RespondToSignal(...)
            // to the OnSignalChange event on the given train signal object.
            public void RegistertoSignal(TrainSignal signal)
            {
                signal.OnSignalChange += new LightChangeHandler(RespondToSignal);
            }


            // This function handles the response to the train signal changing.
            // If it is green the train will start moving, otherwise it will
            // stop.
            private void RespondToSignal(SignalColour colour)
            {
                if(colour == SignalColour.Red)
                {
                    StopMoving();
                }
                else
                {
                    StartMoving();
                }
            } 
        }

        public class LevelCrossing
        {
            private void LowerBarriers() { Console.WriteLine("Barriers coming down!!"); }
            private void RaiseBarriers() { Console.WriteLine("Barriers raising up!!");}


            // This function registers the private method RespondToSignal(...)
            // to the OnSignalChange event on the given train signal object.
            public void RegistertoSignal(TrainSignal signal)
            {
              signal.OnSignalChange += new LightChangeHandler(RespondToSignal);
            }


            // This function handles the response to the train signal changing.
            // If it is green the barrier will drop, otherwise it will rise.
            private void RespondToSignal(SignalColour colour)
            {
              if(colour == SignalColour.Red)
              {
                RaiseBarriers();
              }
              else
              {
                LowerBarriers();
              }
            } 
        }


        // The main entry point of the demo.
        static void Main()
        {

            // Create an instance of the signal and a train and level
            // crossing.

            TrainSignal signal = new TrainSignal();
            Train train = new Train();
            LevelCrossing crossing = new LevelCrossing();
            WarningSiren siren = new WarningSiren();


            // Pass in the signal so that each class may register a 
            // delegate to the OnSignalChange event. Note the first
            // event registered will be executed first.

            crossing.RegistertoSignal(signal);
            train.RegistertoSignal(signal);
            siren.RegisterToSignal(signal);



            // Change the signal colour to trigger the OnSignalChange
            // event.

            signal.SetSignal(SignalColour.Green);
            signal.SetSignal(SignalColour.Red);
            signal.SetSignal(SignalColour.Green);


            /******* CONSOLE WINDOW *********

            ****************************
            Signal now changing to Green...
            Barriers coming down!!
            Choo Choo!! Off I go...
            ****************************
            Signal now changing to Red...
            Barriers raising up!!
            EEEKK!!! Applying the brakes...
            ****************************
            Signal now changing to Green...
            Barriers coming down!!
            Choo Choo!! Off I go...
            
            ***** CONSOLE WINDOW END ******/
        }
    }
}
