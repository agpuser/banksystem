/**
 * SIT232 Object-Oriented Programming
 * Project 2
 * Aaron Pethybridge
 * Student#: 217561644
 * FileHandler class
 * Class to handle file loading and saving for Customer and Account data
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Project2
{
    class FileHandler
    {
        // Instance variables
        private FileStream _CustomersFileStream; // Customers file object
        private FileStream _AccountsFileStream; // Accounts file object
        private string _CustomersFileName; // Customers data file name
        private string _AccountsFileName; // Accounts data file name

        // methods
        // Constructor
        public FileHandler(string inAccountsFileName, string inCustomersFileName)
        {
            _AccountsFileName = inAccountsFileName;
            _CustomersFileName = inCustomersFileName;
        }

        // Reads in Customer data from file
        public List<Customer> LoadCustomerData()
        {
            List<Customer> inCustomers = new List<Customer>();

            try
            {
                _CustomersFileStream = File.Open(_CustomersFileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(_CustomersFileStream);
                int noCustomers = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < noCustomers; i++)
                {
                    Customer cust = new Customer(sr);
                    inCustomers.Add(cust);
                }
                sr.Close();
                _CustomersFileStream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to load or read file: {0}. No data loaded.", _CustomersFileName);
            }
            return inCustomers;
        }

        // Writes Customer data to file
        public void SaveCustomerData(List<Customer> inCustomers)
        {
            try
            {
                _CustomersFileStream = File.Open(_CustomersFileName, FileMode.Open, FileAccess.Write);
                StreamWriter sw = new StreamWriter(_CustomersFileStream);
                sw.WriteLine(inCustomers.Count);
                for (int i = 0; i < inCustomers.Count; i++)
                {
                    sw.WriteLine(inCustomers[i].ID);
                    sw.WriteLine(inCustomers[i].FirstName);
                    sw.WriteLine(inCustomers[i].LastName);
                    sw.WriteLine(inCustomers[i].Address);
                    sw.WriteLine(inCustomers[i].DateOfBirth.ToString("dd/MM/yyyy"));
                    sw.WriteLine(inCustomers[i].ContactNo);
                    sw.WriteLine(inCustomers[i].Email);
                }
                sw.Close();
                _CustomersFileStream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to load or write file: {0}. No data saved.", _CustomersFileName);
            }
        }

        // Reads in Account data from file
        public List<Account> LoadAccountData(List<Customer> inCustomers)
        {
            List<Account> inAccounts = new List<Account>();
            DateTime openedDate, closedDate;
            string opened = "", closed = "";
            Account acc;

            try
            {
                _AccountsFileStream = File.Open(_AccountsFileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(_AccountsFileStream);
                int noAccounts = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < noAccounts; i++)
                {
                    int accountType = Convert.ToInt32(sr.ReadLine());
                    int accountID = Convert.ToInt32(sr.ReadLine());
                    int ownerID = Convert.ToInt32(sr.ReadLine());
                    opened = sr.ReadLine();
                    try
                    {
                        openedDate = DateTime.ParseExact(opened, "d/M/yyyy",
                                                    System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid Opened date for Account ID: {0}. Today's date used instead", accountID);
                        openedDate = DateTime.Now;
                    }
                    closed = sr.ReadLine();
                    float balance = (float)Convert.ToDecimal(sr.ReadLine());
                    if (accountType == 1)
                        acc = new Type1Account(inCustomers[ownerID - 1], openedDate, balance);
                    else
                        acc = new Type2Account(inCustomers[ownerID - 1], openedDate, balance);
                    if (closed != "")
                    {
                        try
                        {
                            closedDate = DateTime.ParseExact(closed, "d/M/yyyy",
                                                        System.Globalization.CultureInfo.InvariantCulture);
                            acc.Close(closedDate);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid Closed date for Account ID: {0}. Today's date used instead", accountID);
                            closedDate = DateTime.Now;
                        }
                    }
                    inAccounts.Add(acc);
                }
                sr.Close();
                _AccountsFileStream.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to load or read file: {0}. No data loaded.", _AccountsFileName);
            }
            return inAccounts;
        }

        // Writes Account data to file
        public void SaveAccountData(List<Account> inAccounts)
        {
            try
            {
                _AccountsFileStream = File.Open(_AccountsFileName, FileMode.Open, FileAccess.Write);
                StreamWriter sw = new StreamWriter(_AccountsFileStream);
                sw.WriteLine(inAccounts.Count);
                for (int i = 0; i < inAccounts.Count; i++)
                {
                    if (inAccounts[i].GetType() == typeof(Type1Account))
                        sw.WriteLine("1");
                    else
                        sw.WriteLine("2");
                    sw.WriteLine(inAccounts[i].ID);
                    sw.WriteLine(inAccounts[i].Owner.ID);
                    sw.WriteLine(inAccounts[i].OpenedDate.ToString("d/M/yyyy"));
                    if (!inAccounts[i].Active)
                        sw.WriteLine(inAccounts[i].ClosedDate.ToString("d/M/yyyy"));
                    else
                        sw.WriteLine("");
                    sw.WriteLine(inAccounts[i].Balance);
                }
                sw.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to load or write file: {0}. No data saved.", _AccountsFileName);
            }
        }
    }
}
