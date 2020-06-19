using System;
using System.Threading;

namespace DAN_XXXV_Kristina_Garcia_Francisco
{
    class Program
    {
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
