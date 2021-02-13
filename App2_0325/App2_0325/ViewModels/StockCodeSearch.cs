using System;
using System.Collections.Generic;
using System.Text;
using App2_0325.Models;

namespace App2_0325.ViewModels
{
    public class StockCodeSearch
    {
        public string code_search( string stock_name)
        {
            string stock_code = "";
            sangjang sj = new sangjang();
            for (int i = 0; i < ALL_Constants.SJ_Number; i++)
            {
                if (sj.jongmok[i].Equals(stock_name))
                {
                    stock_code = sj.company[i].ToString("D6");
                    break;
                }
                if (i == (ALL_Constants.SJ_Number - 1)) 
                {
                    stock_code = "STOCK_CODE_ERROR"; 
                    return stock_code; 
                }
            }
            return stock_code;
        }
    }
}
