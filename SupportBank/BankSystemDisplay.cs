﻿namespace SupportBank
{
    public abstract class BankSystemDisplay
    {
        public abstract void DisplayWelcome();
        public abstract void DisplayAllInformation();
        public abstract void DisplaySpecificPersonInformation(string user);
    }
}