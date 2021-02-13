using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;
using System.Xml;
using System.Net;

using App2_0325.Models;

namespace App2_0325.ViewModels
{
    public class Global_days
    {
        public const int _days = 60;
        public const int divide_days = 10;
        public const int _days_ex = 120;
    }

    public struct stock_
    {
        public int s_date;
        public int s_dcp_int;
        public int s_dtv_int;
        public int s_dmp_int;
        public int s_dhp_int;
        public int s_dlp_int;
    }
    public struct stock_v2
    {
        public int s_date;
        public int s_dcp_int;
        public int s_dmp_int;
        public int s_dhp_int;
        public int s_dlp_int;
    }

    class Html_foreach
    {
        public stock_ html_get_event(stock_ get)
        {

            return get;
        }

    }
    public class html_addr : INotifyPropertyChanged
    {
        public html_addr()
        {

        }
        #region notifyproperty
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        string candleday;
        public string CandleDay
        {
            get => candleday;
            set => candleday = value;
        }
        public void html_png_erase()
        {
            candleday = "";
            NotifyPropertyChanged(nameof(CandleDay));
        }
        public void html_png_parsing(string jusik_code, int kinds)
        {
            //https://ssl.pstatic.net/imgfinance/chart/item/candle/day/005380.png
            //https://finance.naver.com/item/main.nhn?code=005380# 
            //https://finance.naver.com/item/main.nhn?code=005380
            //https://ssl.pstatic.net/imgfinance/chart/item/candle/day/005380.png
            var html="";

            switch (kinds)
            {
                case Kind_Constants.kinds_day:
                    html = @"https://ssl.pstatic.net/imgfinance/chart/item/candle/day/";
                    break;
                case Kind_Constants.kinds_week:
                    html = @"https://ssl.pstatic.net/imgfinance/chart/item/candle/week/";
                    break;
                case Kind_Constants.kinds_month:
                    html = @"https://ssl.pstatic.net/imgfinance/chart/item/candle/month/";
                    break;
                default:
                    break;
            }
            var test = jusik_code + ".png";
            html += test;
            candleday = html.ToString();
            NotifyPropertyChanged(nameof(CandleDay));
        }

        public string html_data_parsing(string jusik_code, ref stock_[] stock_days, int kind, int kind_version)
        {
            //Initial
            stock_[] stock = new stock_[Global_days.divide_days];
            string put = "";
            stock = stock_days; //ref

            int carry = 0;

            //Method Set
            MethodClass call_method = new MethodClass();

            var html = "";
            var test = "";
            switch (kind)
            {
                case Kind_Constants.days_10:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=1";
                    carry = 0;
                    break;
                case Kind_Constants.days_20:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=2";
                    carry = 10;
                    break;
                case Kind_Constants.days_30:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=3";
                    carry = 20;
                    break;
                case Kind_Constants.days_40:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=4";
                    carry = 30;
                    break;
                case Kind_Constants.days_50:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=5";
                    carry = 40;
                    break;
                case Kind_Constants.days_60:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=6";
                    carry = 50;
                    break;
                case Kind_Constants.days_70:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=7";
                    carry = 60;
                    break;
                case Kind_Constants.days_80:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=8";
                    carry = 70;
                    break;
                case Kind_Constants.days_90:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=9";
                    carry = 80;
                    break;
                case Kind_Constants.days_100:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=10";
                    carry = 90;
                    break;
                case Kind_Constants.days_110:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=11";
                    carry = 100;
                    break;
                case Kind_Constants.days_120:
                    html = @"https://finance.naver.com/item/sise_day.nhn?code=";
                    test = jusik_code + "&page=12";
                    carry = 110;
                    break;
            }
            html += test; // 주식 정보 종합

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            var HtmlDoc = web.Load(html);

            HtmlAgilityPack.HtmlNodeCollection[] htmlNodes = new HtmlAgilityPack.HtmlNodeCollection[Global_days.divide_days];

            //3,4,5,6,7
            //11,12,13,14,15
            for (int i = 0; i < Global_days.divide_days; i++)
            {
                int jump = 3;
                if (i >= 5)
                {
                    // ex) i=5 + jump -> 11 
                    jump = 6;
                }
                jump += i;
                htmlNodes[i] = HtmlDoc.DocumentNode.SelectNodes("//body/table[1]/tr[" + jump + "]");
                if (htmlNodes[i] == null) { return i + jump + "err"; }

                //td1 날짜, td2 종가, td3 전일비, td4 시가, td5 고가, td6 저가 td7 거래량
                foreach (var node in htmlNodes[i])
                {

                    if (node != null)
                    {
                        switch (kind_version)
                        {
                            case Kind_Constants.version_1:
                                var data_date = node.SelectSingleNode("td[1]").InnerText;
                                var data_closing_price = node.SelectSingleNode("td[2]").InnerText;
                                var data_market_price = node.SelectSingleNode("td[4]").InnerText;
                                var data_high_price = node.SelectSingleNode("td[5]").InnerText;
                                var data_low_price = node.SelectSingleNode("td[6]").InnerText;
                                var data_transaction_volume = node.SelectSingleNode("td[7]").InnerText;

                                put += "Date:" + data_date + " 종가:" + data_closing_price + " 시가:" + data_market_price +
                                   " 고가:" + data_high_price + " 저가:" + data_low_price + " 거래량:" + data_transaction_volume + Environment.NewLine;

                                //stock[carry].s_date = call_method.CnvStringToInt_4(data_date);
                                stock_days[carry].s_date = call_method.CnvStringToInt_4(data_date);
                                stock_days[carry].s_dcp_int = call_method.CnvStringToInt(data_closing_price);
                                stock_days[carry].s_dtv_int = call_method.CnvStringToInt(data_transaction_volume);
                                stock_days[carry].s_dmp_int = call_method.CnvStringToInt(data_market_price);
                                stock_days[carry].s_dhp_int = call_method.CnvStringToInt(data_high_price);
                                stock_days[carry].s_dlp_int = call_method.CnvStringToInt(data_low_price);
                                carry++;
                                break;
                            case Kind_Constants.version_2:
                                var data_date2 = node.SelectSingleNode("td[1]").InnerText;
                                var data_closing_price2 = node.SelectSingleNode("td[2]").InnerText;
                                var data_market_price2 = node.SelectSingleNode("td[4]").InnerText;
                                var data_high_price2 = node.SelectSingleNode("td[5]").InnerText;
                                var data_low_price2 = node.SelectSingleNode("td[6]").InnerText;
                                var data_transaction_volume2 = node.SelectSingleNode("td[7]").InnerText;

                                put += "Date:" + data_date2 + " 종가:" + data_closing_price2 + " 시가:" + data_market_price2 +
                                   " 고가:" + data_high_price2 + " 저가:" + data_low_price2 + " 거래량:" + data_transaction_volume2 + Environment.NewLine;

                                //stock[carry].s_date = call_method.CnvStringToInt_4(data_date);
                                stock_days[carry].s_date = call_method.CnvStringToInt_4(data_date2);
                                stock_days[carry].s_dcp_int = call_method.CnvStringToInt(data_closing_price2);
                                stock_days[carry].s_dmp_int = call_method.CnvStringToInt(data_market_price2);
                                stock_days[carry].s_dhp_int = call_method.CnvStringToInt(data_high_price2);
                                stock_days[carry].s_dlp_int = call_method.CnvStringToInt(data_low_price2);
                                carry++;
                                break;
                        }
                    }
                }
            }
            return put;
        }
    }
}
