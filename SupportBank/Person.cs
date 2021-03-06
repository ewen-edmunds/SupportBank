﻿using System;
using System.Collections.Generic;

namespace SupportBank
{
    public class Person
    {
        public string Name;
        public decimal Balance = 0;
        public List<Payment> Payments = new List<Payment>();

        public Person(string newName)
        {
            this.Name = newName;
        }

        public void AddPayment(Payment newPayment)
        {
            this.Payments.Add(newPayment);
            
            if (newPayment.FromAccount == this.Name)
            {
                this.Balance -= newPayment.Amount;
            }
            else if (newPayment.ToAccount == this.Name)
            {
                this.Balance += newPayment.Amount;
            }
        }
    }
}