#if false
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

///////Excel/////////
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

///////Crawler/////////
using HtmlAgilityPack;
using System.Net;
using System.Web;
using System.IO;

namespace ReZero_Project_1
{
    public partial class Form1 : Form
    {
        //All Reference....
        //Excel.Application excelApp = null;
        //Excel.Workbook wb = null;
        //Excel.Worksheet ws = null;
        private Timer timer;
        
        private const int ROW_MAX = 3722;

        string jusik_code = null;
        int err_cnt = 0;

        //엑셀 표 대체
        sangjang s1 = new sangjang();

        //Method Set
        MethodClass call_method = new MethodClass();

        //const
        const int _5days = 5;
        const int _days = 60;

        Global_days GG = new Global_days();
        
        // int[] s_date = new int[_days]; //date get
        // int[] s_dcp_int = new int[_days]; //closing price
        // int[] s_dtv_int = new int[_days]; //transaction volume
        // int[] s_dmp_int = new int[_days]; //marget price
        // int[] s_dhp_int = new int[_days]; //high price
        // int[] s_dlp_int = new int[_days]; //low price

        stock_[] stock = new stock_[_days];

        //FLAG
        bool timer_end = false;
        bool excel_load_flg = false;
        bool AI_Learn_flg = false;

        public Form1()
        {
            InitializeComponent();

            // progressbar1 timer
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = BAP.SelectedIndex;
            //string item = BAP.SelectedItem.ToString();
            //
            //textBox9.Text = index + "/" + item + "Selected";
            
        }

        //Excel Load
        private void button2_Click(object sender, EventArgs e)
        {
        
            //progressbar1 + time
            timer.Start();
            progressBar1.PerformStep();

            string st_bt2 = textBox1.Text;

            sangjang sj = new sangjang();

            //3721 Company Max
            for (int i = 0; i < 3721; i++)
            {
                if (sj.jongmok[i].Equals(st_bt2))
                {
                    //종목 명
                    textBox1.Text = sj.jongmok[i];

                    //종목코드
                    //string sr = sj.company[i].ToString();
                    textBox2.Text = sj.company[i].ToString("D6");
                    jusik_code = textBox2.Text;
                    //textBox2.Text = Convert.ToString(sj.company[i]);
                    break;
                }
                if (i == 3720) textBox2.Text = "ERR";
            }
            timer_end = true;
        }

        //AI START
        private void button1_Click(object sender, EventArgs e)
        {
            double Learn_Result = 0;
            string item = BAP.SelectedItem.ToString();
            textBox9.Text = item + "Selected" + Environment.NewLine;
            if (AI_Learn_flg == false) { textBox9.Text += "DATA READ FAIL"; return; }

            BP_Learn BP = new BP_Learn();

            //1. martget price
            //2. high price
            //3. low price
            //4. transaction price
            //5. closing price (output)

            //5days
            //Learn_Result = BP.BP_START(s_dmp_int, s_dhp_int, s_dlp_int, s_dtv_int, s_dcp_int);
            Learn_Result = BP.BP_START_STOCK(ref stock);

            textBox9.Text += "종가 예측: " + (int)Learn_Result;
        }


        // Add Func
        void timer_Tick(object sender, EventArgs e)
        {
            if (timer_end == true)
            {
                timer.Stop();
                progressBar1.Enabled = false;
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        //회사 명
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.MaxLength = 15;
            if (excel_load_flg == true) textBox1.ReadOnly = true;
            else textBox1.ReadOnly = false; // 초기화 후 회사 명 다시 입력 가능
        }

        //종목 코드
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.MaxLength = 15;
            if (excel_load_flg == true) textBox2.ReadOnly = true;
        }

        //대표자명
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.MaxLength = 15;
            if (excel_load_flg == true) textBox3.ReadOnly = true;
        }

        //결산 월
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox5.MaxLength = 15;
            if (excel_load_flg == true) textBox5.ReadOnly = true;
        }

        //지역
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            textBox6.MaxLength = 15;
            if (excel_load_flg == true) textBox6.ReadOnly = true;
        }

        //업종
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            textBox7.MaxLength = 1000;
            if (excel_load_flg == true) textBox7.ReadOnly = true;
        }

        //주요제품
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.MaxLength = 1000;
            if (excel_load_flg == true) textBox4.ReadOnly = true;
        }

        //초기화 버튼
        private void button3_Click(object sender, EventArgs e)
        {
            excel_load_flg = false;
            AI_Learn_flg = false;
            //회사 명
            textBox1.Text = "검색어 입력";

            //종목 코드
            textBox2.Text = null;

            //대표자명
            textBox3.Text = null;

            //결산 월
            textBox5.Text = null;

            //지역
            textBox6.Text = null;

            //주요제품
            textBox4.Text = null;

            //업종
            textBox7.Text = null;

            //타이머 바
            progressBar1.Value = 0;

            jusik_code = null;

            //크롤링 정보
            textBox8.Text = null;

            // AI 선택
            textBox9.Text = null;
        }

        //Data 수집 버튼 
        //Naver 주식 정보 크롤링
        //https://html-agility-pack.net/
        ////https://finance.naver.com/item/sise_day.nhn?code=084680 (일별 시세)
        //https://finance.naver.com/item/sise_day.nhn?code=084680&page=1
        //jusik_code = "084680";
        //https://finance.naver.com/item/sise_time.nhn?code=084680&thistime=20200224161036
        private void button4_Click(object sender, EventArgs e)
        {
            AI_Learn_flg = true;
            html_addr HTML_ADDR = new html_addr();

            //NEW
            //Page1 10days
            textBox8.Text = HTML_ADDR.html_HtmlDoc_page1(jusik_code, ref stock);
            //Page2 20days
            textBox8.Text += HTML_ADDR.html_HtmlDoc_page2(jusik_code, ref stock);
            //Page3 30days
            textBox8.Text += HTML_ADDR.html_HtmlDoc_page3(jusik_code, ref stock);
            //Page4 40days
            textBox8.Text += HTML_ADDR.html_HtmlDoc_page4(jusik_code, ref stock);
            //Page5 50days
            textBox8.Text += HTML_ADDR.html_HtmlDoc_page5(jusik_code, ref stock);
            //Page6 60days
            textBox8.Text += HTML_ADDR.html_HtmlDoc_page6(jusik_code, ref stock);
#if false
            //시가,고가,저가,거래량 --> 종가   총 5개 데이터 필요, 5일선,20일선,60일선 
            var html = @"https://finance.naver.com/item/sise_day.nhn?code=";
            var test = jusik_code + "&page=1";
            html += test; // 주식 정보 종합

            HtmlWeb web = new HtmlWeb();
            var HtmlDoc = web.Load(html);

            int carry = 0;
            string[] s_string = new string[_days];

            var htmlNodes_1 = HtmlDoc.DocumentNode.SelectNodes("//body/table[1]/tr[3]");
            var htmlNodes_2 = HtmlDoc.DocumentNode.SelectNodes("//body/table[1]/tr[4]");
            var htmlNodes_3 = HtmlDoc.DocumentNode.SelectNodes("//body/table[1]/tr[5]");
            var htmlNodes_4 = HtmlDoc.DocumentNode.SelectNodes("//body/table[1]/tr[6]");
            var htmlNodes_5 = HtmlDoc.DocumentNode.SelectNodes("//body/table[1]/tr[7]");


            if (htmlNodes_1 == null) { err_cnt++; textBox8.Text = "error " + err_cnt + "\n"; return; }
            if (htmlNodes_2 == null) { err_cnt++; textBox8.Text = "error " + err_cnt + "\n"; return; }
            if (htmlNodes_3 == null) { err_cnt++; textBox8.Text = "error " + err_cnt + "\n"; return; }
            if (htmlNodes_4 == null) { err_cnt++; textBox8.Text = "error " + err_cnt + "\n"; return; }
            if (htmlNodes_5 == null) { err_cnt++; textBox8.Text = "error " + err_cnt + "\n"; return; }

            //td1 날짜, td2 종가, td3 전일비, td4 시가, td5 고가, td6 저가 td7 거래량
            foreach (var node in htmlNodes_1)
            {
                if (node != null)
                {
                    var data_date               = node.SelectSingleNode("td[1]").InnerText;
                    var data_closing_price      = node.SelectSingleNode("td[2]").InnerText;
                    var data_market_price       = node.SelectSingleNode("td[4]").InnerText;
                    var data_high_price         = node.SelectSingleNode("td[5]").InnerText;
                    var data_low_price          = node.SelectSingleNode("td[6]").InnerText;
                    var data_transaction_volume = node.SelectSingleNode("td[7]").InnerText;
                    textBox8.Text = "Date:" + data_date + " 종가:" + data_closing_price + " 시가:" + data_market_price + 
                        " 고가:"+ data_high_price + " 저가:" + data_low_price + " 거래량:" + data_transaction_volume + Environment.NewLine;

                    s_date[carry] = call_method.CnvStringToInt_4(data_date);
                    s_dcp_int[carry] = call_method.CnvStringToInt(data_closing_price);
                    s_dtv_int[carry] = call_method.CnvStringToInt(data_transaction_volume);
                    s_dmp_int[carry] = call_method.CnvStringToInt(data_market_price);
                    s_dhp_int[carry] = call_method.CnvStringToInt(data_high_price);
                    s_dlp_int[carry] = call_method.CnvStringToInt(data_low_price);
                    carry++;
                }
            }

            foreach (var node in htmlNodes_2)
            {
                if (node != null)
                {
                    var data_date = node.SelectSingleNode("td[1]").InnerText;
                    var data_closing_price = node.SelectSingleNode("td[2]").InnerText;
                    var data_market_price = node.SelectSingleNode("td[4]").InnerText;
                    var data_high_price = node.SelectSingleNode("td[5]").InnerText;
                    var data_low_price = node.SelectSingleNode("td[6]").InnerText;
                    var data_transaction_volume = node.SelectSingleNode("td[7]").InnerText;
                    textBox8.Text += "Date:" + data_date + " 종가:" + data_closing_price + " 시가:" + data_market_price +
                        " 고가:" + data_high_price + " 저가:" + data_low_price + " 거래량:" + data_transaction_volume + Environment.NewLine;

                    s_date[carry] = call_method.CnvStringToInt_4(data_date);
                    s_dcp_int[carry] = call_method.CnvStringToInt(data_closing_price);
                    s_dtv_int[carry] = call_method.CnvStringToInt(data_transaction_volume);
                    s_dmp_int[carry] = call_method.CnvStringToInt(data_market_price);
                    s_dhp_int[carry] = call_method.CnvStringToInt(data_high_price);
                    s_dlp_int[carry] = call_method.CnvStringToInt(data_low_price);
                    carry++;
                }
            }

            foreach (var node in htmlNodes_3)
            {
                if (node != null)
                {
                    var data_date = node.SelectSingleNode("td[1]").InnerText;
                    var data_closing_price = node.SelectSingleNode("td[2]").InnerText;
                    var data_market_price = node.SelectSingleNode("td[4]").InnerText;
                    var data_high_price = node.SelectSingleNode("td[5]").InnerText;
                    var data_low_price = node.SelectSingleNode("td[6]").InnerText;
                    var data_transaction_volume = node.SelectSingleNode("td[7]").InnerText;
                    textBox8.Text += "Date:" + data_date + " 종가:" + data_closing_price + " 시가:" + data_market_price +
                        " 고가:" + data_high_price + " 저가:" + data_low_price + " 거래량:" + data_transaction_volume + Environment.NewLine;

                    s_date[carry] = call_method.CnvStringToInt_4(data_date);
                    s_dcp_int[carry] = call_method.CnvStringToInt(data_closing_price);
                    s_dtv_int[carry] = call_method.CnvStringToInt(data_transaction_volume);
                    s_dmp_int[carry] = call_method.CnvStringToInt(data_market_price);
                    s_dhp_int[carry] = call_method.CnvStringToInt(data_high_price);
                    s_dlp_int[carry] = call_method.CnvStringToInt(data_low_price);
                    carry++;
                }
            }

            foreach (var node in htmlNodes_4)
            {
                if (node != null)
                {
                    var data_date = node.SelectSingleNode("td[1]").InnerText;
                    var data_closing_price = node.SelectSingleNode("td[2]").InnerText;
                    var data_market_price = node.SelectSingleNode("td[4]").InnerText;
                    var data_high_price = node.SelectSingleNode("td[5]").InnerText;
                    var data_low_price = node.SelectSingleNode("td[6]").InnerText;
                    var data_transaction_volume = node.SelectSingleNode("td[7]").InnerText;
                    textBox8.Text += "Date:" + data_date + " 종가:" + data_closing_price + " 시가:" + data_market_price +
                        " 고가:" + data_high_price + " 저가:" + data_low_price + " 거래량:" + data_transaction_volume + Environment.NewLine;

                    s_date[carry] = call_method.CnvStringToInt_4(data_date);
                    s_dcp_int[carry] = call_method.CnvStringToInt(data_closing_price);
                    s_dtv_int[carry] = call_method.CnvStringToInt(data_transaction_volume);
                    s_dmp_int[carry] = call_method.CnvStringToInt(data_market_price);
                    s_dhp_int[carry] = call_method.CnvStringToInt(data_high_price);
                    s_dlp_int[carry] = call_method.CnvStringToInt(data_low_price);
                    carry++;
                }
            }

            foreach (var node in htmlNodes_5)
            {
                if (node != null)
                {
                    var data_date = node.SelectSingleNode("td[1]").InnerText;
                    var data_closing_price = node.SelectSingleNode("td[2]").InnerText;
                    var data_market_price = node.SelectSingleNode("td[4]").InnerText;
                    var data_high_price = node.SelectSingleNode("td[5]").InnerText;
                    var data_low_price = node.SelectSingleNode("td[6]").InnerText;
                    var data_transaction_volume = node.SelectSingleNode("td[7]").InnerText;
                    textBox8.Text += "Date:" + data_date + " 종가:" + data_closing_price + " 시가:" + data_market_price +
                        " 고가:" + data_high_price + " 저가:" + data_low_price + " 거래량:" + data_transaction_volume + Environment.NewLine;

                    s_date[carry] = call_method.CnvStringToInt_4(data_date);
                    s_dcp_int[carry] = call_method.CnvStringToInt(data_closing_price);
                    s_dtv_int[carry] = call_method.CnvStringToInt(data_transaction_volume);
                    s_dmp_int[carry] = call_method.CnvStringToInt(data_market_price);
                    s_dhp_int[carry] = call_method.CnvStringToInt(data_high_price);
                    s_dlp_int[carry] = call_method.CnvStringToInt(data_low_price);
                    carry++;
                }
            }
            
            //chart add
            chart1.Series["Series1"].Points.Clear();
            chart1.Series["Series1"].Points.AddXY(s_date[4].ToString("D4"), s_dcp_int[4]);
            chart1.Series["Series1"].Points.AddXY(s_date[3].ToString("D4"), s_dcp_int[3]);
            chart1.Series["Series1"].Points.AddXY(s_date[2].ToString("D4"), s_dcp_int[2]);
            chart1.Series["Series1"].Points.AddXY(s_date[1].ToString("D4"), s_dcp_int[1]);
            chart1.Series["Series1"].Points.AddXY(s_date[0].ToString("D4"), s_dcp_int[0]);
#endif

            //chart add
            chart1.Series["Series1"].Points.Clear();
            for (int i = GG._days-1; i > 0; i--)
            {
                chart1.Series["Series1"].Points.AddXY(stock[i].s_date.ToString("D4"), stock[i].s_dcp_int);
            }
        }

        //크롤링 테스트 중
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        //간단한 차트 추가
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }

    //c# 크롤링 class
    class agility_parse
    {
        public Encoding utf = Encoding.GetEncoding("utf-8");
        public HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
        public WebClient web = new WebClient();

        public Stream stream_source;
    }
}
#endif