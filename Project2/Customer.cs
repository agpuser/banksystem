/**
 * SIT232 Object-Oriented Programming
 * Project 2
 * Aaron Pethybridge
 * Student#: 217561644
 * Customer class
 */
using System;
using System.Collections.Generic;
using System.IO;


namespace Project2
{
    class Customer
    {
        // Class wide variable
        private static uint MIN_AGE_YEAR = 16;
        private static uint IDCounter = 0;

        protected uint _ID;
        public uint ID
        { get { return _ID; } }

        // Instance variables (and properties) to store customer details
        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        private DateTime _DateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set { _DateOfBirth = value; }
        }
        private string _ContactNo;
        public string ContactNo
        {
            get { return _ContactNo; }
            set { _ContactNo = value; }
        }
        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        // Collection of customer owned accounts
        private List<Account> _Accounts = new List<Account>();
        public List<Account> Accounts
        {
            get { return _Accounts; }
        }

        // Customer class constructors
        public Customer(string inFName, string inLName, 
            string inAddress, DateTime inDoB, string inContactNo = "", string inEmail = "")
        {
            if (DoBValid(inDoB.Year))
            {
                _FirstName = inFName;
                _LastName = inLName;
                _Address = inAddress;
                _DateOfBirth = inDoB;
                _ContactNo = validateContact(inContactNo);
                _Email = inEmail;
                _ID = ++IDCounter; // Increment class ID counter
            }
            else
                Console.WriteLine(" - System Error - cannot create accounts for persons less than 16 years of age.");
        }

        public Customer(StreamReader inStrRead)
        {
            _ID = Convert.ToUInt32(inStrRead.ReadLine());
            IDCounter = Math.Max(IDCounter, _ID);
            _FirstName = inStrRead.ReadLine();
            _LastName = inStrRead.ReadLine();
            _Address = inStrRead.ReadLine();
            try
            {
                _DateOfBirth = DateTime.ParseExact(inStrRead.ReadLine(), "d/M/yyyy",
                                            System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Date of Birth for {0} {1}. Today's date used instead",_FirstName, _LastName);
                _DateOfBirth = DateTime.Now;
            }
            _ContactNo = inStrRead.ReadLine();
            _Email = inStrRead.ReadLine();
        }

        // Customer copy constructor
        public Customer(Customer source) : this(source._FirstName, source._LastName,
            source._Address, source._DateOfBirth, source._ContactNo, source.Email)
        {
            _Accounts = new List<Account>(source._Accounts);
        }

        // Class methods
        // Validate DOB - confirm customer is at least 16 years old
        public static bool DoBValid(int inYear)
        {
            return inYear <= DateTime.Now.Year - MIN_AGE_YEAR;
        }

        // Validate Contact No. - returns a value that is 10 characters long
        public string validateContact(string inContact)
        {
            string result = "";

            if (inContact.Length > 10)
                result = inContact.Substring(0, 10);
            else
                result = inContact;
            return result;
        }

        // Adds an account to customer account list
        public void AddAccount(Account inAccount)
        {
            _Accounts.Add(inAccount);
        }

        // Returns sum of all customer owned account balances
        public float SumBalance()
        {
            float result = 0.0f;
            for (int i = 0; i < _Accounts.Count; i++)
                result += _Accounts[i].Balance;
            return result;
        }

        // Returns String representation of object
        public override string ToString()
        {
            return string.Format("ID: {0} Name: {1} {2} Address: {3} DOB: {4} Contact: {5} Email: {6} Total balance: {7:#,##0.0}", _ID, _FirstName, _LastName, _Address, _DateOfBirth.ToString("d/MM/yyyy"), ContactNo, Email, SumBalance());
        }
    }
}
