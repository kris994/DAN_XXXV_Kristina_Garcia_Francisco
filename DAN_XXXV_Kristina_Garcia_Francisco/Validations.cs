using System;

namespace DAN_XXXV_Kristina_Garcia_Francisco
{
    /// <summary>
    /// Validates the user input
    /// </summary>
    class Validations
    {
        /// <summary>
        /// The user input has to be a positive number above 0
        /// </summary>
        /// <returns>the integer value</returns>
        public int ValidPositiveNumber()
        {
            string s = Console.ReadLine();
            bool b = Int32.TryParse(s, out int num);

            while (!b || num < 1)
            {
                Console.Write("Value has to be above 0. Try again: ");
                s = Console.ReadLine();
                b = Int32.TryParse(s, out num);
            }
            return num;
        }

        /// <summary>
        /// The user input has to be number between 1 and 100
        /// </summary>
        /// <returns>the integer value</returns>
        public int RandomNumber()
        {
            string s = Console.ReadLine();
            bool b = Int32.TryParse(s, out int num);
            while (!b || (num < 1 || num > 100))
            {
                Console.Write("Invalid input. Number has to be between 1 and 100: ");
                s = Console.ReadLine();
                b = Int32.TryParse(s, out num);
            }
            return num;
        }
    }
}
