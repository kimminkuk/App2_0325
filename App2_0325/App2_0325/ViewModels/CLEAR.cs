using System;
using System.Collections.Generic;
using System.Text;
using App2_0325.Models;

namespace App2_0325.ViewModels
{
    class CLEAR
    {
        public void CLEAR_STOCK(ref stock_[] bp_stock, int kind)
        {

            stock_[] clear_stock = new stock_[Global_days._days];
            clear_stock = bp_stock;

            for(int i = 0; i < Global_days._days; i++)
            {
                clear_stock[i].s_date = 0;
                clear_stock[i].s_dcp_int = 0;
                clear_stock[i].s_dhp_int = 0;
                clear_stock[i].s_dlp_int = 0;
                clear_stock[i].s_dmp_int = 0;
                clear_stock[i].s_dtv_int = 0;
            }
        }
    }
}
