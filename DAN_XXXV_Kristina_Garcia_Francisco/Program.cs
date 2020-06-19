using System;
using System.Threading;

namespace DAN_XXXV_Kristina_Garcia_Francisco
{
    /// <summary>
    /// The main program class
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            NumberGuesser ng = new NumberGuesser();

            Thread getValues = new Thread(ng.EnterValues);
            getValues.Start();
            getValues.Join();

            Console.ReadKey();
        }
    }
}
