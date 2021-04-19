/**
 * SIT232 Object-Oriented Programming
 * Project 2
 * Aaron Pethybridge
 * Student#: 217561644
 * Account class - Abstract class
 */
using System;
using System.Collections.Generic;

namespace Project2
{
    abstract class Account
    {
        // Class variables, instance variables (& properties) and constants
        private static float DEFAULT_BALANCE = 0.0f;
        private static uint IDCounter = 0;

        protected uint _ID;
        public uint ID
        { get { return _ID; } }

        protected DateTime _OpenedDate;
        public DateTime OpenedDate
        { get { return _OpenedDate; } }

        protected DateTime _ClosedDate;
        public DateTime ClosedDate
        { get { return _ClosedDate; } }

        protected bool _Active;
        public bool Active
        { get { return _Active; } }

        protected float _Balance = 0.0f;
        public float Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }
        protected Customer _Owner;
        public Customer Owner
        { get { return _Owner; } }

        // class constructors
        // 3 paramter constructor - Owner, Open Date and Balance
        public Account(Customer inOwner, DateTime inOpenedDate, float inBalance = 0.0f)
        {
            _Owner = inOwner;
            if (inOpenedDate <= DateTime.Now)
                _OpenedDate = inOpenedDate;
            else
                _OpenedDate = DateTime.Now;

            if (inBalance >= DEFAULT_BALANCE)
                _Balance = inBalance;
            else
                _Balance = DEFAULT_BALANCE;
            _Active = true; // Acount made active
            _ID = ++IDCounter; // Increment class ID counter
            inOwner.AddAccount(this); // Add this Account object to inOwner's Account List
            _Owner = inOwner;
        }

        // 2 paramter constructor - Owner and Balance
        public Account(Customer inOwner, float inBalance) : this(inOwner, DateTime.Now, inBalance) { }

        // class methods
        // Closes off account (only if still active), sets to inactive, sets closed date to today
        public void Close()
        {
            if (_Active)
            {
                _ClosedDate = DateTime.Now;
                _Active = false;
            }
            else
                Console.Write(" - Transaction error - Account is already closed.");
        }

        // Closes off account (only if still active), sets to inactive, sets closed date to inClosedDate
        public void Close(DateTime inClosedDate)
        {
            if (_Active)
            {
                _ClosedDate = inClosedDate;
                _Active = false;
            }
            else
                Console.Write(" - Transaction error - Account is already closed.");
        }

        // Abstract method - Transfers inAmount from this account to inAccount account
        public abstract void Transfer(Account inAccount, float inAmount);

        // Abstract method - Returns account's accrued interest
        public abstract float CalculateInterest();

        // Update current balance by applying accrued interest
        public void UpdateBalance()
        {
            _Balance += CalculateInterest();
        }

        // Overriden ToString method
        public override string ToString()
        {
            string result = String.Format("ID: {0} Opened Date: {1} Balance: {2:#,##0.0} Owner: {3} {4}", _ID, _OpenedDate.ToString("d/MM/yyyy"), _Balance, _Owner.FirstName, _Owner.LastName);
            return result;  //(result + closedString);
        }
    }
}
