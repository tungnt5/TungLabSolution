using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabSolution.WEB.Common
{
    public static class InputValidator
    {
        public const string DateFormat = "dd/MM/yyyy";
        public const string PhoneNumberRegex = @"^[0-9\-]+$";
        public const string NumericRegex = @"^[0-9]+$";
        public const string AlphaRegex = @"^[a-zA-Z]+$";
        public const string AlphaNumericRegex = @"^[.0-9a-zA-Z]+$";
        public const string PasswordRegex = @"^(?=.*\d)(?=.*[a-zA-Z]).*$";
        public const string EmailRegex = @"^[0-9a-zA-Z]+([_\-.0-9a-zA-Z][_\-0-9a-zA-Z]+)*@[0-9a-zA-Z]+([_\-.0-9a-zA-Z][_\-0-9a-zA-Z]+)*$";
        public const string TimeRegex = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
    }
}