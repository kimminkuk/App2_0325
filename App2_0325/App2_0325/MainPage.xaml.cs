using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;
using Xamarin.Forms;

//namespace
using App2_0325.ViewModels;
using App2_0325.Models;

//drawing add
using SkiaSharp;
using SkiaSharp.Views.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace App2_0325
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
//        MethodClass Mt = new MethodClass();
        const int _days = 60;
        const int _days_ex = 120;
        stock_[] stock = new stock_[_days_ex];
        bool AI_Learn_flg = false;
        bool chart_erase = false;

        //Global
        string jusik_code = "";

        //Add 0415 Skiasharp
        SKPaint blackFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black,
            StrokeWidth = 5
        };

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new html_addr();
            
        }

        //https://ssl.pstatic.net/imgfinance/chart/item/candle/day/005380.png
        private void Button_Clicked_Search(object sender, EventArgs e)
        {
            /*21-02-13*/
            html_addr HA = (html_addr)BindingContext;
            StockCodeSearch codeSearch = new StockCodeSearch();
            /*21-02-12*/
            // enter 제거 (string.trim() -> 문자열 앞,뒤 공백 제거)
            if (String.IsNullOrEmpty(DATA.Text.Trim())) { EDI1.Text = "ERR"; return; }
            string search_data = DATA.Text.ToString().Trim();
            EDI1.Text = "";

            /*21-02-13*/
            jusik_code = codeSearch.code_search(search_data);

            if(jusik_code.Equals("STOCK_CODE_ERROR")) //Err Finish
            {
                EDI1.Text = "ERR";
                return; //finish
            }
            //Stock Image
            //Default Day
            HA.html_png_parsing(jusik_code, Kind_Constants.kinds_day);

            
            EDI1.Text = HA.html_data_parsing(jusik_code,  ref stock, Kind_Constants.days_60, Kind_Constants.version_2);
            EDI1.Text += HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_50, Kind_Constants.version_2);
            EDI1.Text += HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_40, Kind_Constants.version_2);
            EDI1.Text += HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_30, Kind_Constants.version_2);
            EDI1.Text += HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_20, Kind_Constants.version_2);
            EDI1.Text += HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_10, Kind_Constants.version_2);


            HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_70, Kind_Constants.version_2);
            HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_80, Kind_Constants.version_2);
            HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_90, Kind_Constants.version_2);
            HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_100, Kind_Constants.version_2);
            HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_110, Kind_Constants.version_2);
            HA.html_data_parsing(jusik_code, ref stock, Kind_Constants.days_120, Kind_Constants.version_2);


            //SKPaint(Chart)
            //            canvasView.InvalidateSurface();
            AI_Learn_flg = true;
            chart_erase = true;
        }

        private void Button_Clicked_erase(object sender, EventArgs e)
        {
            html_addr HA = (html_addr)BindingContext;
            EDI1.Text = "CLEAR";
            HA.html_png_erase(); // Image Binding Clear
            AI_Learn_flg = false;
            chart_erase = false;
            CLEAR stk_clr = new CLEAR();

            stk_clr.CLEAR_STOCK(ref stock, Kind_Constants.test_120days);

            //SKPaint(Chart)
//            canvasView.InvalidateSurface();
        }

        //60days Learn
        private void Button_Clicked_Test(object sender, EventArgs e)
        {
            double Learn_Result = 0;
            if (AI_Learn_flg == false) { EDI1.Text += "DATA READ FAIL"; return; }

            BP_Learn BP = new BP_Learn();

            //1. martget price
            //2. high price
            //3. low price
            //4. transaction price
            //5. closing price (output)

            //Version2
            Learn_Result = BP.BP_START_STOCK_VERSION2(ref stock, Kind_Constants.test_60days);
            EDI1.Text = "종가 예측: " + (int)Learn_Result;
        }

        //120days
        private void Button_Clicked_Test_120(object sender, EventArgs e)
        {
            double Learn_Result = 0;

            if (AI_Learn_flg == false) { EDI1.Text += "DATA READ FAIL"; return; }

            BP_Learn BP = new BP_Learn();

            //1. martget price
            //2. high price
            //3. low price
            //4. transaction price
            //5. closing price (output)

            //Version2
            Learn_Result = BP.BP_START_STOCK_VERSION2(ref stock, Kind_Constants.test_120days);
            EDI1.Text = "종가 예측: " + (int)Learn_Result;
        }

        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.White);
            
            int width = e.Info.Width;
            int height = e.Info.Height;

            SKBitmap sKBitmap = new SKBitmap(width, height);

            SKPoint[] points = new SKPoint[Global_days._days];

            int new_width = width / Global_days._days;
            //int new_height = height

            //for (int i = GG._days - 1; i > 0; i--)
            //{
            //    points[i] = new SKPoint(i* new_width, stk_v2[i].s_date);
            //} 

            // 20/12/14 code add
            // add for High Price to Height 
            int temp_height = 0;
            for (int i = 0; i < Global_days._days; i++ )
            {
                if (temp_height < stock[i].s_dhp_int )
                {
                    temp_height = stock[i].s_dhp_int;
                }
            }
            //add for Low Price to Height
            int temp_height_low = stock[0].s_dlp_int;
            for (int i = 0; i < Global_days._days; i++)
            {
                if (temp_height_low > stock[i].s_dlp_int)
                {
                    temp_height_low = stock[i].s_dlp_int;
                }
            } 
#if false
            for (int i = 0; i < GG._days; i++)
            {
                float new_height = (stk_v2[i].s_dcp_int / (float)stk_v2[i].s_dhp_int) * height;
                points[i] = new SKPoint(i * new_width, new_height);
            }
#endif
            for (int i = 0; i < Global_days._days; i++)
            {
                float new_height = (float)stock[i].s_dcp_int;
                points[i] = new SKPoint(i, new_height);
            }

            for (int i = 1; i < Global_days._days; i++)
            {
                //canvas
                
                canvas.DrawLine(points[i - 1], points[i], blackFillPaint);
            }

            Point[] point_oxy = new Point[Global_days._days];
            

             //OxyPlot
             var model = new PlotModel
             {
                 Title = "Line Title",
                 PlotType = PlotType.XY
             };
            
             //X
             model.Axes.Add(new LinearAxis
             {
                 Title = "Date",
                 Position = AxisPosition.Bottom
             });
            
             //Y
             model.Axes.Add(new LinearAxis
             {
                 Title = "Close Price",
                 Position = AxisPosition.Left
             });
            
             var Points_Oxy = new List<DataPoint>();

            //CreateLine();

            var series2 = new OxyPlot.Series.LineSeries
            {
                LineStyle = LineStyle.None,
                MarkerFill = OxyColors.Transparent,
                MarkerStroke = OxyColors.Black,
                MarkerStrokeThickness = 2
            };


            for (int i = 0; i < Global_days._days; i++)
            {
                series2.Points.Add(new DataPoint(stock[i].s_date, stock[i].s_dcp_int/100));
            }
            series2.Smooth = true;
            model.Series.Add(series2);
            

        }

//        private void CreateLine()
//        {
//            var myModel = new PlotModel { Title = "Example 2" };
//            myModel.Series.Add
//                (
//                new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)")
//                );
//
//            this.PlotChart.Model = myModel;
//            
//        }
        private void Button_Clicked_Save(object sender, EventArgs e)
        {

        }
        private void Button_Clicked_Next(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_Day(object sender, EventArgs e)
        {
            /*21-02-13*/
            html_addr HA = (html_addr)BindingContext;
            HA.html_png_parsing(jusik_code, Kind_Constants.kinds_day);
        }
        private void Button_Clicked_Week(object sender, EventArgs e)
        {
            /*21-02-13*/
            html_addr HA = (html_addr)BindingContext;
            HA.html_png_parsing(jusik_code, Kind_Constants.kinds_week);
        }
        private void Button_Clicked_Month(object sender, EventArgs e)
        {
            /*21-02-13*/
            html_addr HA = (html_addr)BindingContext;
            HA.html_png_parsing(jusik_code, Kind_Constants.kinds_month);
        }
    }
}
