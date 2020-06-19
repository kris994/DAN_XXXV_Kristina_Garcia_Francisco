using System;
using System.Collections.Generic;
using System.Threading;

namespace DAN_XXXV_Kristina_Garcia_Francisco
{
    /// <summary>
    /// Class used to guess the correct number
    /// </summary>
    class NumberGuesser
    {
        #region Properties       
        /// <summary>
        /// Number of threads that are created
        /// </summary>
        private int noParticipants = 0;
        /// <summary>
        /// The given number to search
        /// </summary>
        private int randomNumber = 0;
        /// <summary>
        /// The currently guessed number by the thread
        /// </summary>
        private int guessedNumber = 0;
        /// <summary>
        /// Contains all created threads
        /// </summary>
        private List<Thread> allParticipants = new List<Thread>();
        /// <summary>
        /// Guessed number randomizer
        /// </summary>
        private Random rng = new Random();
        /// <summary>
        /// The locked object
        /// </summary>
        private static readonly object l = new object();
        #endregion

        /// <summary>
        /// Reads user input values and announces them
        /// </summary>
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

        /// <summary>
        /// Creates new threads equal to the number of participants
        /// </summary>
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

                // Start all generated threads
                foreach (var item in allParticipants)
                {
                    item.Start();
                }
            }
        }
        
        /// <summary>
        /// Announces the given inputs
        /// </summary>
        public void Announcer()
        {
            Console.WriteLine("User entered the number of participants, {0}, " +
                "and the number to be guessed, {1}.\n", noParticipants, randomNumber);
        }

        /// <summary>
        /// Guess the number and announce the tries
        /// </summary>
        public void GuessTheNumber()
        {
            // Repeat until the guessed number is the same as the given
            while (guessedNumber != randomNumber)
            {
                lock (l)
                {
                    // In case a thread entered with the wrong number even 
                    // after the correct one has been guessed
                    if (guessedNumber == randomNumber)
                    {
                        break;
                    }

                    guessedNumber = rng.Next(1, 101);

                    // Wrong guess
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
                    // Correct guess
                    else
                    {
                        Console.WriteLine("Participant {0} won, guessed value was {1}.", Thread.CurrentThread.Name, guessedNumber);
                    }
                    Thread.Sleep(100);
                }            
            }
        }

        /// <summary>
        /// Check if the number is odd or even
        /// </summary>
        /// <param name="num">the number that is being checked</param>
        /// <returns>Even or Odd</returns>
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
