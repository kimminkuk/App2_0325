using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2_0325.ViewModels
{
    class MethodClass
    {
        public int CnvStringToInt(string dcp)
        {
            string s_dtv = "";
            int stringtoint = 0;
            foreach (char c in dcp)
            {
                if (c >= '0' && c <= '9')
                {
                    s_dtv = String.Concat(s_dtv, c);
                }
            }
            if (Int32.TryParse(s_dtv, out stringtoint))
            {
                return stringtoint;
            }
            else
            {
                return 0;
            }
        }

        public int CnvStringToInt_4(string dcp)
        {
            string s_dtv = "";
            int stringtoint = 0;
            foreach (char c in dcp)
            {
                if (c >= '0' && c <= '9')
                {
                    s_dtv = String.Concat(s_dtv, c);
                }
            }
            s_dtv = s_dtv.Substring(4);

            if (Int32.TryParse(s_dtv, out stringtoint))
            {
                return stringtoint;
            }
            else
            {
                return 0;
            }
        }

        public bool IsParseNumber(object obj)
        {
            int parse = -1;

            try
            {
                parse = Int32.Parse(obj.ToString());
                return true;
            }

            catch
            {
                Int32.TryParse(obj.ToString(), out parse);

                return parse == -1 || parse == 0 ? false : true ; 
            }
        }
    }
}
