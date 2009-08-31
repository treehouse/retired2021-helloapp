using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hello.Utils
{
    public static class CodeHelper
    {
        private static Random _r;
        private static Random r
        {
            get
            {
                if (_r == null)
                    _r = new Random();
                return _r;
            }
        }

        public static string GenerateUniqueSeatCode(List<string> seatCodes)
        {
            var code = String.Empty;
            do
            {
                code = GenerateSeatCode();
            }
            while (seatCodes.Contains(code));

            seatCodes.Add(code);

            return code;
        }

        private static string GenerateSeatCode()
        {
            return GenerateCode(r, 5);
        }

        public static string GenerateTokenCode()
        {
            return GenerateCode(r, 10);
        }

        private static string GenerateCode(Random r, int length)
        {
            var code = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                var c = r.Next(36) + 48;
                if (c > 57)
                    c += 7;
                code.Append((char)c);
            }

            return code.ToString();
        }
    }
}
