/**
 * SIT232 Object-Oriented Programming
 * Project 1
 * Aaron Pethybridge
 * Student#: 217561644
 * Type1Account class - Account sub-class
 */
using System;
using System.Collections.Generic;

namespace Project2
{
    class Type2Account : Account
    {
        // Class wide variables
        private static float ANNUAL_INTEREST_RATE = 3.0f;
        private static float DEPOSIT_INTEREST_RATE = 4.0f;
        public float AnnualRate
        {
            get { return ANNUAL_INTEREST_RATE; }
        }
        public float DepositRate
        {
            get { return DEPOSIT_INTEREST_RATE; }
        }

        // Instance variables (& properties)
        protected float _MonthlyDeposit;
        public float MonthlyDeposit
        {
            get { return _MonthlyDeposit; }
            set { _MonthlyDeposit = value; }
        }

        // 3 paramter constructor - Owner, Open Date and Balance
        public Type2Account(Customer inOwner, DateTime inOpenedDate, float inBalance = 0.0f) :
            base(inOwner, inOpenedDate, inBalance) { }

        // 2 paramter constructor - Owner and Balance
        public Type2Account(Customer inOwner, float inBalance) : this(inOwner, DateTime.Now, inBalance) { }

        // Transfers inAmount from this account to inAccount
        public override void Transfer(Account inAccount, float inAmount)
        {
            if (inAccount.Owner == _Owner) // Same owner for InAccount and this account
            {
                if (inAccount.GetType() == typeof(Type1Account)) // transfer account is Type1Account
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
                else
                    Console.Write(" - Transaction error - Cannot transfer to a Type 2 Account.");
            }
            else
                Console.Write(" - Transaction error - Cannot transfer to account of a different owner.");
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
            return (AnnualRate / 365 / 100) * nDays * Balance +
                (DepositRate / 365 / 100) * nDays * MonthlyDeposit;
        }

        // Update current balance by applying accrued monthly interest
        public new void UpdateBalance()
        {
            base.UpdateBalance();
            _MonthlyDeposit = 0.0f;
        }
    }
}
