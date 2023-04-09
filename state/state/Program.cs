using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace state
{
    abstract class State
    {
        public Account account;
        public double balance;
        public double interest;
        public double lowerLimit;
        public double upperLimit;

        public Account GetAccount()
        {
            return account;
        }
        public void SetAccount(Account account)
        {
            this.account = account;
        }
        public double GetBalance()
        {
            return balance;
        }
        public void SetBalance(double balance)
        {
            this.balance = balance;
        }
        public abstract void Deposit(double amount);
        public abstract bool Withdraw(double amount);
        public abstract bool PayInterest();
    }
    class Account
    {
        private State state;
        private string owner;

        public Account(string owner)
        {
            this.owner = owner;
        }
        public double GetBalance()
        {
            return state.GetBalance();
        }
        public State GetState()
        {
            return state;
        }
        public void SetState(State state)
        {
            this.state = state;
        }
        public void Deposit(double amount)
        {
            state.Deposit(amount);
            string buffer = Convert.ToString(amount);
            Console.WriteLine(buffer);
            buffer = Convert.ToString(GetBalance());
            Console.WriteLine(buffer);
            buffer = Convert.ToString(GetState());
            Console.WriteLine(buffer);
        }
        public void Withdraw(double amount)
        {
            if (state.Withdraw(amount) == true)
            {
                state.Deposit(amount);
                string buffer = Convert.ToString(amount);
                Console.WriteLine(buffer);
                buffer = Convert.ToString(GetBalance());
                Console.WriteLine(buffer);
                buffer = Convert.ToString(GetState());
                Console.WriteLine(buffer);
            }
        }
        public void PayInterest()
        {
            if (state.PayInterest() == true)
            {
                Console.WriteLine("Interest Paid: \n");
                string buffer = Convert.ToString(GetBalance());
                Console.WriteLine(buffer);
                buffer = Convert.ToString(GetState());
                Console.WriteLine(buffer);
            }
        }
    }
    class GoldState : State
    {
        private void Initialize()
        {
            interest = 0.07;
            lowerLimit = 1000.0;
            upperLimit = 10000000.0;
        }
        private void StateChangeCheck()
        {
            if (balance < 0.0)
            {
                account.SetState(new RedState(this));
            }
            else if (balance < lowerLimit)
            {
                account.SetState(new SilverState(this));
            }
        }
        public GoldState(double balance, Account account)
        {
            this.balance = balance;
            this.account = account;
            Initialize();
        }
        public GoldState(State state)
        {
            var TempBalance = state.GetBalance();
            var TempAcc = state.GetAccount();
            Console.WriteLine(TempBalance + " | " + TempAcc);
        }
        public override void Deposit(double amount)
        {
            balance += amount;
            StateChangeCheck();
        }
        public override bool Withdraw(double amount)
        {
            balance -= amount;
            StateChangeCheck();
            return true;
        }
        public override bool PayInterest()
        {
            balance += interest * balance;
            StateChangeCheck();
            return true;
        }
    }
    class RedState : State
    {
        private void Initialize()
        {
            interest = 0.0;
            lowerLimit = -100.0;
            upperLimit = 0.0;
        }
        private void StateChangeCheck()
        {
            if (balance > upperLimit)
            {
                account.SetState(new SilverState(this));
            }
        }

        public RedState(State state)
        {
            this.balance = state.GetBalance();
            this.account = state.GetAccount();
            Initialize();
        }
        public override void Deposit(double amount)
        {
            balance += amount;
            StateChangeCheck();
        }
        public override bool Withdraw(double amount)
        {
            Console.WriteLine("No funds available for withdrawal!\n");
            return false;
        }
        public override bool PayInterest()
        {
            Console.WriteLine("No interest is paid!\n");
            return false;
        }
    }
    class SilverState : State
    {
        private void Initialize()
        {
            interest = 0.01;
            lowerLimit = 0.0;
            upperLimit = 1000.0;
        }
        private void StateChangeCheck()
        {
            if (balance < lowerLimit)
            {
                account.SetState(new RedState(this));
            }
            else if (balance > upperLimit)
            {
                account.SetState(new GoldState(this));
            }
        }

        public SilverState(double balance, Account account)
        {
            this.balance = balance;
            this.account = account;
            Initialize();
        }
        public SilverState(State state)
        {
            var TempBalance = state.GetBalance();
            var TempAcc = state.GetAccount();
            Console.WriteLine(TempBalance + " | " + TempAcc);
        }
        public override void Deposit(double amount)
        {
            balance += amount;
            StateChangeCheck();
        }
        public override bool Withdraw(double amount)
        {
            balance -= amount;
            StateChangeCheck();
            return true;
        }
        public override bool PayInterest()
        {
            balance += interest * balance;
            StateChangeCheck();
            return true;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Account a = new Account(new GoldState());
            a.PayInterest();
        }
    }
}
