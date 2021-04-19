
/**
 * SIT232 Object-Oriented Programming
 * Project 1
 * Aaron Pethybridge
 * Student#: 217561644
 * Type1Account class - Account sub-class
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Project2
{
    class Type1Account : Account
    {
        // Class wide variable
        private static float ANNUAL_INTEREST_RATE = 2.0f;
        public float AnnualRate
        {
            get { return ANNUAL_INTEREST_RATE; }
        }

        // 3 paramter constructor - Owner, Open Date and Balance
        public Type1Account(Customer inOwner, DateTime inOpenedDate, float inBalance = 0.0f) :
            base(inOwner, inOpenedDate, inBalance) {}
        
        // 2 paramter constructor - Owner and Balance
        public Type1Account(Customer inOwner, float inBalance) : this(inOwner, DateTime.Now, inBalance) { }

        //public Type1Account(StreamReader sr) {}

        // Adds depositAmount to account balance
        public void Deposit(float depositAmount)
        {
            if (_Active) // Account active
            {
                if (depositAmount > 0.0) // Deposit > 0
                    _Balance += depositAmount;
                else
                    Console.Write(" - Transaction error - Deposit amount must be greater than zero.");
            }
            else
                Console.Write(" - Transaction error - Cannot deposit into an inactive account");
        }

        // Deducts withdrawlAmount from account balance
        public void Withdraw(float withdrawalAmount)
        {
            if (_Active) // Account active
            {
                if (withdrawalAmount > 0.0) // Withdrawal > 0
                {
                    if (withdrawalAmount <= _Balance)
                        _Balance -= withdrawalAmount;
                    else
                        Console.Write(" - Transaction error - Withdrawal amount greater than account balance.");
                }
                else
                    Console.Write(" - Transaction error - Withdrawal amount must be greater than zero.");
            }
            else
                Console.Write(" - Transaction error - Cannot withdraw from an inactive account");
        }

        // Transfers inAmount from this account to inAccount
        public override void Transfer(Account inAccount, float inAmount)
        {
            if (inAmount > 0.0)
            {
                if (inAmount <= _Balance)
                {
                    _Balance -= inAmount;
                    inAccount.Balance += inAmount;
                }
                else
                    Console.Write(" - Transaction error - Transfer amount greater than account balance.");
            }
            else
                Console.Write(" - Transaction error - Transfer amount must be greater than zero.");
        }

        // Returns account's accrued interest
        public override float CalculateInterest()
        {
            int nDays = 0;
            DateTime currentDate = DateTime.Now;

            if (currentDate.Month > OpenedDate.Month || currentDate.Year > OpenedDate.Year) // Account opened during a previous month
                nDays = currentDate.Day - 1;
            else // Account opened in this month
                nDays = currentDate.Day - OpenedDate.Day;
            return (AnnualRate / 365 / 100) * nDays * _Balance;
        }
    }
}
