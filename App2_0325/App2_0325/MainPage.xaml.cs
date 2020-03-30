using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Xamarin.Forms;

namespace App2_0325
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        MethodClass Mt = new MethodClass();
        Global_days GG = new Global_days();
        const int _days = 60;
        stock_[] stock = new stock_[_days];
        stock_v2[] stk_v2 = new stock_v2[_days];
        bool AI_Learn_flg = false;

        public MainPage()
        {
            InitializeComponent();

        }
        
        private void Button_Clicked_Search(object sender, EventArgs e)
        {
            sangjang sj = new sangjang();

            // enter 제거?
            if (String.IsNullOrEmpty(DATA.Text)) { EDI1.Text = "ERR";return; }
            
            string search_data = DATA.Text.ToString();
            
            string jusik_code = "";
            EDI1.Text = "";

            for (int i = 0; i < 3721; i++)
            {
                if (sj.jongmok[i].Equals(search_data))
                {
                    EDI1.Text = sj.jongmok[i].ToString() + "\t";
                    jusik_code = sj.company[i].ToString("D6");
                    break;
                }
                if (i == 3720) { EDI1.Text = "ERR"; return; }
            }


            html_addr HTML_ADDR = new html_addr();

            //https://m.stock.naver.com/item/main.nhn#/stocks/084680/price

            //NEW
            // //Page1 10days
            // EDI1.Text = HTML_ADDR.html_HtmlDoc_page6(jusik_code, ref stock);
            // //Page2 20days
            // EDI1.Text += HTML_ADDR.html_HtmlDoc_page5(jusik_code, ref stock);
            // //Page3 30days
            // EDI1.Text += HTML_ADDR.html_HtmlDoc_page4(jusik_code, ref stock);
            // //Page4 40days
            // EDI1.Text += HTML_ADDR.html_HtmlDoc_page3(jusik_code, ref stock);
            // //Page5 50days
            // EDI1.Text += HTML_ADDR.html_HtmlDoc_page2(jusik_code, ref stock);
            // //Page6 60days
            // EDI1.Text += HTML_ADDR.html_HtmlDoc_page1(jusik_code, ref stock);

            EDI1.Text = HTML_ADDR.html_HtmlDoc_page6_v2(jusik_code, ref stk_v2);
            //Page2 20days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page5_v2(jusik_code, ref stk_v2);
            //Page3 30days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page4_v2(jusik_code, ref stk_v2);
            //Page4 40days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page3_v2(jusik_code, ref stk_v2);
            //Page5 50days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page2_v2(jusik_code, ref stk_v2);
            //Page6 60days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page1_v2(jusik_code, ref stk_v2);

            AI_Learn_flg = true;
        }

        private void Button_Clicked_erase(object sender, EventArgs e)
        {
            EDI1.Text = "CLEAR";
            Lable1.Text = "CLEAR";
            AI_Learn_flg = false;
            CLEAR stk_clr = new CLEAR();

            stk_clr.CLEAR_STOCK(ref stock,ref stk_v2);
            
        }

        private void Button_Clicked_Test(object sender, EventArgs e)
        {
            double Learn_Result = 0;
           
            if (AI_Learn_flg == false) { Lable1.Text += "DATA READ FAIL"; return; }

            BP_Learn BP = new BP_Learn();

            //1. martget price
            //2. high price
            //3. low price
            //4. transaction price
            //5. closing price (output)

            //5days
            //Learn_Result = BP.BP_START(s_dmp_int, s_dhp_int, s_dlp_int, s_dtv_int, s_dcp_int);

            //Version1
            //Learn_Result = BP.BP_START_STOCK(ref stock);

            //Version2
            Learn_Result = BP.BP_START_STOCK_VERSION2(ref stk_v2);
            Lable1.Text = "종가 예측: " + (int)Learn_Result;
        }
    }
}
