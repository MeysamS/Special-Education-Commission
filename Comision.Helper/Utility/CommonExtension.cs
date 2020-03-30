using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comision.Helper.Utility
{
    public static class CommonExtension
    {
        public static string NullToString(this string value)
        {

            // Value.ToString() allows for Value being DBNull, but will also convert int, double, etc.
            return value ?? " ";

            // If this is not what you want then this form may suit you better, handles 'Null' and DBNull otherwise tries a straight cast
            // which will throw if Value isn't actually a string object.
            //return Value == null || Value == DBNull.Value ? "" : (string)Value;


        }
    }
}
