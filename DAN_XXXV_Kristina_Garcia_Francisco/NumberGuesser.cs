using System;
using System.Collections.Generic;
using System.Threading;

namespace DAN_XXXV_Kristina_Garcia_Francisco
{
    class NumberGuesser
    {
        private int noParticipants = 0;
        private int randomNumber = 0;
        private int guessedNumber = 0;
        private List<Thread> allParticipants = new List<Thread>();
        private Random rng = new Random();
        private static readonly object l = new object();

        public void EnterValues()
        {
            lock (l)
            {
                Validations val = new Validations();

                Console.Write("Enter number of participants: ");
                noParticipants = val.ValidPositiveNumber();

                Console.Write("Enter the guess number: ");
                randomNumber = val.RandomNumber();

                // Creates threads for the input values
                Thread readValues = new Thread(CreateParticipants)
                {
                    Name = "Thread_Generator"
                };
                readValues.Start();

                // Unlock the lock for Thread_Generator thread until it generates all threads
                Monitor.Wait(l, Timeout.Infinite);

                Announcer();

                // Signal the Thread_Generator that this thread is done with work
                Monitor.Pulse(l);
            }
        }

        public void CreateParticipants()
        {
            lock (l)
            {
                for (int i = 1; i < noParticipants + 1; i++)
                {
                    try
                    {
                        Thread t = new Thread(GuessTheNumber)
                        {
                            Name = "Participant_" + i
                        };
                        allParticipants.Add(t);
                    }
                    catch (OutOfMemoryException)
                    {
                        Console.WriteLine("The entered value is too big.");
                        Console.ReadKey();
                    }                   
                }

                // Signal the getValues thread that Thread_Generator is done with generating the threads.
                Monitor.Pulse(l);
                // Wait for the lock to be open again
                Monitor.Wait(l, Timeout.Infinite);
                // Signalize the generated threads that the lock is avaliable again 
                Monitor.Pulse(l);
                foreach (var item in allParticipants)
                {
                    item.Start();
                }
            }
        }

        public void Announcer()
        {
            Console.WriteLine("User entered the number of participants, {0}, " +
                "and the number to be guessed, {1}.\n", noParticipants, randomNumber);
        }

        public void GuessTheNumber()
        {
            while (guessedNumber != randomNumber)
            {
                lock (l)
                {
                    if (guessedNumber == randomNumber)
                    {
                        break;
                    }

                    guessedNumber = rng.Next(1, 101);

                    if (guessedNumber != randomNumber)
                    {
                        Console.Write("Participant {0} guessed with {1}.", Thread.CurrentThread.Name, guessedNumber);
                        if (OddOrEven(randomNumber) == OddOrEven(guessedNumber))
                        {
                            Console.WriteLine(" Participant {0} guessed the correct parity, {1}."
                                , Thread.CurrentThread.Name, OddOrEven(guessedNumber));
                        }
                        else
                        {
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Participant {0} won, guessed value was {1}.", Thread.CurrentThread.Name, guessedNumber);
                    }
                    Thread.Sleep(100);
                }            
            }
        }

        public string OddOrEven(int num)
        {
            string value;

            if (num % 2 == 0)
            {
                value = "Even";
            }
            else
            {
                value = "Odd";
            }

            return value;
        }
    }
}
