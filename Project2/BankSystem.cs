/**
 * SIT232 Object-Oriented Programming
 * Project 2
 * Aaron Pethybridge
 * Student#: 217561644
 * BankSystem class
 * Class to handle menu processing, transactions, searches, data creation and manage Account & Customer data
 */
using System;
using System.Collections.Generic;
//using System.IO;

namespace Project2
{
    class BankSystem
    {
        // Instance variables
        private List<Customer> _Customers = new List<Customer>(); // Collection of system Customers
        private List<Account> _Accounts = new List<Account>(); // Collection of system Accounts
        private FileHandler _FileIO; 

        // Constructor
        public BankSystem(string inAccountsFileName, string inCustomersFileName)
        {
            _FileIO = new FileHandler(inAccountsFileName, inCustomersFileName);
        }

        // Executes main program loop until a non menu value is entered
        public void RunSystem()
        {
            bool ProgramDone = false;
            string Input = "";

            _Customers = _FileIO.LoadCustomerData();
            _Accounts = _FileIO.LoadAccountData(_Customers);
            while (!ProgramDone)
            {
                MenuSystem.DisplayMainMenu();
                Input =  Console.ReadLine();
                ProgramDone = HandleOption(Input);
            }
            _FileIO.SaveCustomerData(_Customers);
            _FileIO.SaveAccountData(_Accounts);
        }

        // Executes main menu option based on user input
        private bool HandleOption(string input)
        {
            bool done = false;

            switch(input)
            {
                case "1":
                    CreateCustomer();
                    break;
                case "2":
                    SearchCustomer();
                    break;
                case "3":
                    CreateAccount();
                    break;
                case "4":
                    SearchAccount();
                    break;
                case "5":
                    TransferFunds();
                    break;
                case "6":
                    DepositFunds();
                    break;
                case "7":
                    WithdrawFunds();
                    break;
                case "8":
                    SetMonthlyDeposit();
                    break;
                default:
                    done = true; 
                    break;
            }
            return done;
        }

        // Collects and sets monthly deposit amount for Type 2 account
        private void SetMonthlyDeposit()
        {
            uint validID = 0;
            float validAmount = 0.0f;
            bool done = false;

            while (!done)
            {
                validID = GetValidandExistingID("Enter Account ID: ");
                validAmount = MenuSystem.GetValidAmount("Enter Monthly Deposit: ");
                Console.WriteLine("Press “s” or “S” if you want to submit ");
                Console.Write("Press any other keys if you want to cancel ");
                string input = Console.ReadLine();
                if (input == "s" || input == "S")
                {
                    if (_Accounts[(int)validID - 1].GetType() == typeof(Type2Account)) // must be Type 2 account
                    {
                        Type2Account temp = (Type2Account)_Accounts[(int)validID - 1];
                        temp.MonthlyDeposit = validAmount;
                        _Accounts[(int)validID - 1] = temp;
                        done = true;
                    }
                    else
                        Console.WriteLine("You must select a Type 2 account to set a monthly deposit. Please re-enter account ID.");
                }
            }
        }

        // Collects and withdraws amount from Type 1 account
        private void WithdrawFunds()
        {
            uint validID = 0;
            float validAmount = 0.0f;
            bool done = false;

            while (!done)
            {
                validID = GetValidandExistingID("Enter Account ID: ");
                validAmount = MenuSystem.GetValidAmount();
                Console.WriteLine("Press “s” or “S” if you want to submit ");
                Console.Write("Press any other keys if you want to cancel ");
                string input = Console.ReadLine();
                if (input == "s" || input == "S")
                {
                    if (ValidWithdraw((int)validID - 1, validAmount))
                    {
                        Type1Account temp = (Type1Account)_Accounts[(int)validID - 1];
                        temp.Withdraw(validAmount);
                        _Accounts[(int)validID - 1] = temp;
                        done = true;
                    }
                }
            }
        }

        // Validate account ID and amount to enable a withdrawal
        private bool ValidWithdraw(int inAccID, float inAmount)
        {
            bool okayResult = true;

            if (_Accounts[inAccID].GetType() == typeof(Type1Account)) // Must be account Type 1
            {
                if (!_Accounts[inAccID].Active) // Account must be open
                {
                    okayResult = false;
                    Console.WriteLine("Cannot withdraw from a closed account. Withdrawal cancelled.");
                }
                else
                {
                    if (inAmount > _Accounts[inAccID].Balance) // amount must be less than account balance
                    {
                        okayResult = false;
                        Console.WriteLine("Withdrawal amount greater than specified account balance. Withdrawal cancelled.");
                    }
                }
            }
            else
            {
                okayResult = false;
                Console.WriteLine("Cannot make a withdrawal from a Type 2 account. Withdrawal cancelled.");
            }
            return okayResult;
        }

        // Collects and deposits amount into given account
        private void DepositFunds()
        {
            uint validID = 0;
            float validAmount = 0.0f;
            bool done = false;

            while (!done)
            {
                validID = GetValidandExistingID("Enter Account ID: ");
                validAmount = MenuSystem.GetValidAmount();
                Console.WriteLine("Press “s” or “S” if you want to submit ");
                Console.Write("Press any other keys if you want to cancel ");
                string input = Console.ReadLine();
                if (input == "s" || input == "S")
                {
                    if (ValidDeposit((int)validID - 1, validAmount))
                    {
                        Type1Account temp = (Type1Account)_Accounts[(int)validID - 1]; // cast to Type 1 account
                        temp.Deposit(validAmount);
                        _Accounts[(int)validID - 1] = temp;
                        done = true;
                    }
                }
            }
        }

        // Validates account ID and amount to enable a deposit
        private bool ValidDeposit(int inAccID, float inAmount)
        {
            bool okayResult = true;

            if (_Accounts[inAccID].GetType() == typeof(Type1Account))
            {
                if (!_Accounts[inAccID].Active)
                {
                    okayResult = false;
                    Console.WriteLine("Cannot deposit into a closed account. Deposit cancelled.");
                }
            }
            else
            {
                okayResult = false;
                Console.WriteLine("Cannot make a deposit into a Type 2 account. Deposit cancelled.");
            }
            return okayResult;
        }

        // Collect an account ID until a valid value is obtained
        private uint GetValidandExistingID(string prompt)
        {
            bool IDokay = false;
            string inputID = "";
            uint validID = 0;

            while (!IDokay)
            {
                Console.Write(prompt);
                inputID = Console.ReadLine();
                validID = GetValidAccountID(inputID);
                if (validID > 0)
                    IDokay = true;
            }
            return validID;
        }
 
        // Collect and validate account ID and amount to enable a transfer
        private void TransferFunds()
        {
            uint validSourceID = 0, validDestID = 0;
            float validAmount = 0.0f;
            bool done = false;

            while (!done)
            {
                validSourceID = GetValidandExistingID("Enter Source Account ID: ");
                validDestID = GetValidandExistingID("Enter Destination Account ID: ");
                validAmount = MenuSystem.GetValidAmount();
                Console.WriteLine("Press “s” or “S” if you want to submit ");
                Console.Write("Press any other keys if you want to cancel ");
                string input = Console.ReadLine();
                if (input == "s" || input == "S")
                {
                    if (ValidTransfer((int)validSourceID - 1, (int)validDestID - 1, validAmount)) // Okay to transfer
                    {
                        _Accounts[(int)validSourceID - 1].Transfer(_Accounts[(int)validDestID - 1], validAmount);
                        done = true;
                    }
                }
            }
        }

        // Validate account types and owner, and that amount is greater than zero
        private bool ValidTransfer(int inSource, int inDest, float inAmount)
        {
            bool okayResult = true;

            if (_Accounts[inSource].GetType() == typeof(Type2Account)) // Source account is Type2
            {
                if (_Accounts[inDest].GetType() == typeof(Type1Account)) // Destination account is Type 1
                {
                    if (_Accounts[inSource].Owner.ID != _Accounts[inDest].Owner.ID) // check for different owners
                    {
                        okayResult = false;
                        Console.WriteLine("A Type 2 account cannot transfer to an account belonging to a different owner. Transfer cancelled.");
                    }
                }
                else
                {
                    okayResult = false;
                    Console.WriteLine("A Type 2 cannot transfer to another Type 2 account. Transfer cancelled.", inSource + 1);
                }
            }
            if (inAmount > _Accounts[inSource].Balance && okayResult)
            {
                okayResult = false;
                Console.WriteLine("Transfer amount greater than balance of Account ID: {0}. Transfer cancelled.", inSource + 1);
            }
            return okayResult;
        }

        // Checks to determine that specified account ID is the ID of an existing account
        private bool AccountIDExists(uint inAccID)
        {
            bool result = false;

            for (int i = 0; i < _Accounts.Count; i++)
            {
                if (_Accounts[i].ID == inAccID)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        // Validates that account ID is numeric, that account exists and that account is open
        private uint GetValidAccountID(string inAccID)
        {
            uint result = 0;

            try
            {
                result = uint.Parse(inAccID);
                if (!AccountIDExists(result))
                {
                    Console.WriteLine("Account ID does not exist. Please re-enter.");
                    result = 0;
                }
                else
                {
                    if (!_Accounts[(int)(result - 1)].Active)
                    {
                        Console.WriteLine("Account ID belongs to a closed account. Please re-enter.");
                        result = 0;
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid value entered for account ID. Please re-enter.");
            }
            return result;
        }

        // Collects input to conduct search for an account, based on account ID.
        private void SearchAccount()
        {
            uint searchID = 0;

            searchID = GetValidandExistingID("Enter Account ID: ");
            Console.WriteLine("Press “s” or “S” if you want to submit ");
            Console.Write("Press any other keys if you want to cancel ");
            string menuInput = Console.ReadLine();
            if (menuInput == "S" || menuInput == "s")
            {
                DisplayAccountSearchResults(searchID); // Gather & display search results
                Console.Write("\nPress Enter to go back to Main menu");
                string enterKey = Console.ReadLine();
            }
        }

        // Conducts account search and displays appropriate results
        private void DisplayAccountSearchResults(uint inSearchID)
        {
            List<Account> searchResults = new List<Account>();

            for (int i = 0; i < _Accounts.Count; i++)
            {
                if (_Accounts[i].ID == inSearchID) // Determin match based on matching IDs
                    searchResults.Add(_Accounts[i]); // Gather matched accounts
            }
            if (searchResults.Count > 0) // If at least one account found
            {
                for (int i = 0; i < searchResults.Count; i++)
                {
                    Console.WriteLine("\nSearch results:");
                    Console.WriteLine(searchResults[i]);
                }
            }
            else // No matching accounts
                Console.WriteLine("\nNo search results found.");
        }

        // Validates input string contains an existing owner (Customer) ID
        private uint ValidOwnerID(string inOwnerID)
        {
            bool validID = false, foundID = false;
            uint id = 0;

            try
            {
                id = uint.Parse(inOwnerID);
                validID = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Not a valid ID value. Please re-enter.");
            }
            if (validID) // Obtained valid numeric ID
            {
                for (int i = 0; i < _Customers.Count; i++)
                {
                    if (_Customers[i].ID == id) // Check valid ID matches an existing Customer ID
                    {
                        foundID = true;
                        break;
                    }
                }
                if (!foundID)
                    Console.WriteLine("Owner ID not found. Please re-enter.");
            }
            if (!foundID || !validID) // Don't have a valid or existing ID
                id = 0;
            return id;
        }

        // Collects and validates input to create a new Account
        private void CreateAccount()
        {
            bool done = false, typeOkay = false, balOkay = false;
            string inOwnerID = "", inAccountID = "", inInitBal = "";
            uint ownerID = 0;
            float balance = 0.0f;
            Account acc;
            
            while (!done)
            {
                Console.Write("Enter Owner ID: ");
                inOwnerID = Console.ReadLine();
                ownerID = ValidOwnerID(inOwnerID);
                if (ownerID == 0) // Not a valid ID, need to re-enter
                    continue;
                while (!typeOkay)
                {
                    Console.Write("Enter Account Type (1 or 2): ");
                    inAccountID = Console.ReadLine();
                    if (inAccountID == "1" || inAccountID == "2") // Account type 1 or 2
                    {
                        typeOkay = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid account type entered. Please re-enter.");
                    }
                }
                while (!balOkay)
                {
                    Console.Write("Enter Initial Balance: ");
                    inInitBal = Console.ReadLine();
                    try
                    {
                        balance = float.Parse(inInitBal);
                        if (balance > 0.0f)
                            balOkay = true;
                        else
                            Console.WriteLine("Inital balance must be greater then zero. Please re-enter.");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid initial balance entered. Please re-enter.");
                    }
                }
                Console.WriteLine("Press “s” or “S” if you want to submit ");
                Console.Write("Press any other keys if you want to cancel ");
                string menuInput = Console.ReadLine();
                if (menuInput == "S" || menuInput == "s")
                {
                    if (inAccountID == "1")
                    {
                        acc = new Type1Account(_Customers[(int)ownerID-1], DateTime.Now, balance); // bind Type 1 Account
                    }
                    else
                    {
                        acc = new Type2Account(_Customers[(int)ownerID - 1], DateTime.Now, balance); // bind Type 2 Account
                    }
                    _Accounts.Add(acc); // Add to system collection of accounts
                    Console.WriteLine(acc);
                }
                done = true;
            }
        }

        // Gather input to conduct customer search, based on any of the following:
        // First name, Last Name, Address, Date of Birth, Contact or Email
        private void SearchCustomer()
        {
            string inFirst = "", inLast = "", inAddress = "", inDoB = "", inContact = "", inEmail = "";
            DateTime DoB = DateTime.Now;
            bool DoBValid = false; ;

            Console.Write("Enter First Name: ");
            inFirst = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            inLast = Console.ReadLine();
            Console.Write("Enter Address: ");
            inAddress = Console.ReadLine();
            while (!DoBValid)
            {
                Console.Write("Enter Date of birth (DOB) eg.\"dd/MM/yyyy\": ");
                inDoB = Console.ReadLine();
                if (inDoB != "") // if non-empty value entered ie. should be date value
                {
                    try
                    {
                        DoB = DateTime.ParseExact(inDoB, "d/M/yyyy",
                                                    System.Globalization.CultureInfo.InvariantCulture);
                        DoBValid = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid date of birth entered. Please re-enter.");
                    }
                }
                else
                    break;
            }
            Console.Write("Enter Contact: ");
            inContact = Console.ReadLine();
            Console.Write("Enter Email: ");
            inEmail = Console.ReadLine();
            Console.WriteLine("Press “s” or “S” if you want to submit ");
            Console.Write("Press any other keys if you want to cancel ");
            string menuInput = Console.ReadLine();
            if (menuInput == "S" || menuInput == "s")
            {
                DisplayCustomerSearchResults(inFirst, inLast, inAddress, DoB, inContact, inEmail);
                Console.Write("\nPress Enter to go back to Main menu");
                string EnterKey = Console.ReadLine();
            }
        }

        // Conduct customer search and display appropriate results
        private void DisplayCustomerSearchResults(string inFirst, string inLast, string inAddress, DateTime inDoB, string inContact, string inEmail)
        {
            List<Customer> searchResults = new List<Customer>();

            for (int i = 0; i < _Customers.Count; i++)
            {
                // If any of the input values match corressponding customer data member ...
                if (_Customers[i].FirstName == inFirst && inFirst != "" ||
                    _Customers[i].LastName == inLast && inLast != "" ||
                    _Customers[i].Address == inAddress && inAddress != "" ||
                    _Customers[i].DateOfBirth == inDoB ||
                    _Customers[i].ContactNo == inContact && inContact != "" ||
                    _Customers[i].Email == inEmail && inEmail != "")
                    searchResults.Add(_Customers[i]); // ... gather into search results
            }
            if (searchResults.Count > 0)
            {
                for (int i = 0; i < searchResults.Count; i++)
                {
                    Console.WriteLine("\nSearch results:");
                    Console.WriteLine(searchResults[i]);
                }
            }
            else
                Console.WriteLine("\nNo search results found.");
        }

        // Collects and validates input to create a new Customer
        private void CreateCustomer()
        {
            string inFirst = "", inLast = "", inAddress = "", inDoB = "", inContact = "", inEmail = "";
            DateTime DoB = DateTime.Now;
            bool done = false, DoBValid = false, contactOkay = false;

            while(!done)
            {
                inFirst = MenuSystem.GetCustomerElement("Enter First Name: ", "a first name");
                inLast = MenuSystem.GetCustomerElement("Enter Last Name: ", "a last name");
                if (FoundCustomer(inFirst, inLast)) // if Customer name (First & Last) already in system
                {
                    Console.WriteLine("Customer name already exists. Please re-enter.");
                    continue;
                }
                inAddress = MenuSystem.GetCustomerElement("Enter Address: ", "an address");
                while (!DoBValid)
                {
                    Console.Write("Enter Date of birth (DOB) eg.\"dd/MM/yyyy\": ");
                    inDoB = Console.ReadLine();
                    try
                    {
                        DoB = DateTime.ParseExact(inDoB, "d/M/yyyy",
                                                    System.Globalization.CultureInfo.InvariantCulture);
                        DoBValid = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid date of birth entered. Please re-enter.");
                    }
                    if (DoBValid)
                    {
                        if (!Customer.DoBValid(DoB.Year)) // Verify that customer is at least 16 years old
                        {
                            Console.WriteLine("Customer cannot be less than 16 years of age. Please re-enter.");
                            DoBValid = false;
                        }
                    }
                }
                while (!contactOkay)
                {
                    Console.Write("Enter Contact (optional - press Enter for blank): ");
                    inContact = Console.ReadLine();
                    if (inContact.Length != 10 && inContact != "") // Validate contact is zero or 10 characters long
                        Console.WriteLine("Contact must be exactly 10 characters long or blank. Please re-enter.");
                    else
                        contactOkay = true;
                }
                Console.Write("Enter Email (optional - press Enter for blank): ");
                inEmail = Console.ReadLine();
                Console.WriteLine("Press “s” or “S” if you want to submit");
                Console.Write("Press any other keys if you want to cancel: ");
                string menuInput = Console.ReadLine();
                if (menuInput == "S" || menuInput == "s")
                {
                    Customer cust = new Customer(inFirst, inLast, inAddress, DoB, inContact, inEmail);
                    _Customers.Add(cust); // Add new customer to system
                }
                done = true;
            }
        }

        // Checks to see if combination of specified First & Last name is
        // in use by a customer already in the system 
        private bool FoundCustomer(string inFirstName, string inLastName)
        {
            bool found = false;
            for (int i = 0; i < _Customers.Count; i++)
            {
                if (_Customers[i].FirstName == inFirstName &&
                        _Customers[i].LastName == inLastName)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }
    }
}
