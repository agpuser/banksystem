/**
 * SIT232 Object-Oriented Programming
 * Project 2
 * Aaron Pethybridge
 * Student#: 217561644
 * MenuSystem class
 * Class to display the main menu and provide utility functions to handle specific requests for input
 */
using System;
using System.Collections.Generic;

namespace Project2
{
    class MenuSystem
    {
        // Methods

        // Displays main menu options and prompt
        public static void DisplayMainMenu()
        {
            Console.WriteLine("\n***********************");
            Console.WriteLine("BANK SYSTEM - MAIN MENU");
            Console.WriteLine("***********************");
            Console.WriteLine("\nEnter 1 if you want to create a new customer");
            Console.WriteLine("Enter 2 if you want to search a customer");
            Console.WriteLine("Enter 3 if you want to open an account");
            Console.WriteLine("Enter 4 if you want to search an account");
            Console.WriteLine("Enter 5 if you want to transfer money");
            Console.WriteLine("Enter 6 if you want to deposit");
            Console.WriteLine("Enter 7 if you want to withdraw");
            Console.WriteLine("Enter 8 if you want to set monthly deposit");
            Console.WriteLine("Enter any other keys if you want to exit");
            Console.Write("Your option: ");
        }

        // Collect amount input that is valid and greater than zero
        public static float GetValidAmount(string inMsg = "")
        {
            bool formatOkay = false, amtValid = false;
            string amount = "";
            float validFormatAmt = 0.0f;

            while (!formatOkay || !amtValid)
            {
                if (inMsg == "")
                    Console.Write("Enter Amount: ");
                else
                    Console.Write(inMsg);
                amount = Console.ReadLine();
                try
                {
                    validFormatAmt = float.Parse(amount);
                    formatOkay = true;
                    if (validFormatAmt > 0.0f)
                        amtValid = true;
                    else
                        Console.WriteLine("Amount entered must be greater than zero. Please re-enter.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Amount entered is not in a valid format. Please re-enter.");
                }
            }
            return validFormatAmt;
        }

        // Collects input to store in either First name, Last name or Address data member
        public static string GetCustomerElement(string prompt, string element)
        {
            string result = "";
            while (result == "")
            {
                Console.Write(prompt);
                result = Console.ReadLine();
                if (result == "")
                    Console.WriteLine("Cannot enter a blank value for {0}. Please re-enter.", element);
            }
            return result;
        }
    }
}
