namespace StringApp.Services.Identity.Exceptions
{
    using System;

    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message)
            : base(message)
        {
        }
    }
}