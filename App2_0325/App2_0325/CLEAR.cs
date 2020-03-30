using System;
using System.Collections.Generic;
using System.Text;

namespace App2_0325
{
    class CLEAR
    {
        public void CLEAR_STOCK(ref stock_[] bp_stock, ref stock_v2[] bp_stk_v2)
        {
            Global_days GG = new Global_days();

            stock_[] clear_stock = new stock_[GG._days];
            stock_v2[] clear_stock_v2 = new stock_v2[GG._days];
            clear_stock = bp_stock;
            clear_stock_v2 = bp_stk_v2;

            for(int i = 0; i < GG._days; i++)
            {
                clear_stock[i].s_date = 0;
                clear_stock[i].s_dcp_int = 0;
                clear_stock[i].s_dhp_int = 0;
                clear_stock[i].s_dlp_int = 0;
                clear_stock[i].s_dmp_int = 0;
                clear_stock[i].s_dtv_int = 0;

                clear_stock_v2[i].s_date = 0;
                clear_stock_v2[i].s_dcp_int = 0;
                clear_stock_v2[i].s_dhp_int = 0;
                clear_stock_v2[i].s_dlp_int = 0;
                clear_stock_v2[i].s_dmp_int = 0;
            }
        }
    }
}
