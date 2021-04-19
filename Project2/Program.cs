/**
 * SIT232 Object-Oriented Programming
 * Project 2
 * Aaron Pethybridge
 * Student#: 217561644
 * Main program
 * Instantiates and executes Banking menu system
 */
using System;
using System.Collections.Generic;

namespace Project2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instantiate system object
            BankSystem bank = new BankSystem("accounts.txt", // Accounts file name
                                             "customers.txt" // Customer file name
                                             );
            // Loads customer & account data, handles main execution loop and
            // saves customer & account data
            bank.RunSystem(); 
        }
    }
}
