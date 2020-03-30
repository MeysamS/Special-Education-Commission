using System;
using System.Collections.Generic;
using System.Linq;
using Comision.Utility;

namespace Comision.Service.ImplementationService
{
    public static class EnumerationService
    {
        public class Enumx
        {
            public Enumx(string value, string text)
            {
                Value = value;
                Text = text;
            }
            public string Value { get; set; }
            public string Text { get; set; }
        }

        public static List<Enumx> GetEnumValues<TEnum>()
        {
            Type enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new ArgumentException("Not enum type.");

            var values = (TEnum[])System.Enum.GetValues(enumType);

            //List<Enumx> enumList = new List<Enumx>();
            //foreach (var value in values)
            //{
            //    System.Enum test = System.Enum.Parse(typeof(TEnum), value.ToString()) as System.Enum;
            //    int x = Convert.ToInt32(test);
            //    enumList.Add(new Enumx(x.ToString(), test.GetDescription().Replace('_', ' ')));
            //}
            //return enumList;

            return (from value in values select System.Enum.Parse(typeof (TEnum), value.ToString()) as System.Enum into test let x = Convert.ToInt32(test) select new Enumx(x.ToString(), test.GetDescription())).ToList();
        }
    }
}
