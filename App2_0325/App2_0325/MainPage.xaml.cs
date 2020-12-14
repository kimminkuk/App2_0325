using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Xamarin.Forms;

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
        MethodClass Mt = new MethodClass();
        Global_days GG = new Global_days();
        const int _days = 60;
        stock_[] stock = new stock_[_days];
        stock_v2[] stk_v2 = new stock_v2[_days];
        bool AI_Learn_flg = false;
        bool chart_erase = false;
        
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
            
        }

        private void Button_Clicked_Search(object sender, EventArgs e)
        {
            sangjang sj = new sangjang();
            ALL_Constants constants = new ALL_Constants();

            // enter 제거?
            if (String.IsNullOrEmpty(DATA.Text)) { EDI1.Text = "ERR"; return; }
            
            string search_data = DATA.Text.ToString();
            
            string jusik_code = "";
            EDI1.Text = "";

            // 20/12/13            
#if false
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
#endif
            for (int i = 0; i < constants.SJ_Number; i++)
            {
                if (sj.jongmok[i].Equals(search_data))
                {
                    EDI1.Text = sj.jongmok[i].ToString() + "\t";
                    jusik_code = sj.company[i].ToString("D6");
                    break;
                }
                if (i == (constants.SJ_Number - 1)) { EDI1.Text = "ERR"; return; }
            }

            html_addr HTML_ADDR = new html_addr();
            //EDI1.Text += " 60Day data analysis";
            //https://m.stock.naver.com/item/main.nhn#/stocks/084680/price

            //NEW
            //Page1 10days
            EDI1.Text = HTML_ADDR.html_HtmlDoc_page6(jusik_code, ref stock);
            //Page2 20days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page5(jusik_code, ref stock);
            //Page3 30days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page4(jusik_code, ref stock);
            //Page4 40days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page3(jusik_code, ref stock);
            //Page5 50days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page2(jusik_code, ref stock);
            //Page6 60days
            EDI1.Text += HTML_ADDR.html_HtmlDoc_page1(jusik_code, ref stock);

//              HTML_ADDR.html_HtmlDoc_page6_v2(jusik_code, ref stk_v2);
////            //Page2 20days
//              HTML_ADDR.html_HtmlDoc_page5_v2(jusik_code, ref stk_v2);
////            //Page3 30days
//                HTML_ADDR.html_HtmlDoc_page4_v2(jusik_code, ref stk_v2);
////            //Page4 40days
//                HTML_ADDR.html_HtmlDoc_page3_v2(jusik_code, ref stk_v2);
////            //Page5 50days
//                HTML_ADDR.html_HtmlDoc_page2_v2(jusik_code, ref stk_v2);
////            //Page6 60days
//                HTML_ADDR.html_HtmlDoc_page1_v2(jusik_code, ref stk_v2);


            //SKPaint(Chart)
            canvasView.InvalidateSurface();

            AI_Learn_flg = true;
            chart_erase = true;
        }

        private void Button_Clicked_erase(object sender, EventArgs e)
        {
            //EDI1.Text = "CLEAR";
            Lable1.Text = "CLEAR";
            AI_Learn_flg = false;
            chart_erase = false;
            CLEAR stk_clr = new CLEAR();

            stk_clr.CLEAR_STOCK(ref stock,ref stk_v2);

            //SKPaint(Chart)
            canvasView.InvalidateSurface();
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

        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.White);
            
            int width = e.Info.Width;
            int height = e.Info.Height;

            SKBitmap sKBitmap = new SKBitmap(width, height);

            SKPoint[] points = new SKPoint[GG._days];

            int new_width = width / GG._days;
            //int new_height = height

            //for (int i = GG._days - 1; i > 0; i--)
            //{
            //    points[i] = new SKPoint(i* new_width, stk_v2[i].s_date);
            //} 

            // 20/12/14 code add
            // add for High Price to Height 
            int temp_height = 0;
            for (int i = 0; i < GG._days; i++ )
            {
                if (temp_height < stk_v2[i].s_dhp_int )
                {
                    temp_height = stk_v2[i].s_dhp_int;
                }
            }
            //add for Low Price to Height
            int temp_height_low = stk_v2[0].s_dlp_int;
            for (int i = 0; i < GG._days; i++)
            {
                if (temp_height_low > stk_v2[i].s_dlp_int)
                {
                    temp_height_low = stk_v2[i].s_dlp_int;
                }
            } 
#if false
            for (int i = 0; i < GG._days; i++)
            {
                float new_height = (stk_v2[i].s_dcp_int / (float)stk_v2[i].s_dhp_int) * height;
                points[i] = new SKPoint(i * new_width, new_height);
            }
#endif            
            for (int i = 0; i < GG._days; i++)
            {
                float new_height = (float)stk_v2[i].s_dcp_int;
                points[i] = new SKPoint(i, new_height);
            }

            for (int i = 1; i < GG._days; i++)
            {
                //canvas
                
                canvas.DrawLine(points[i - 1], points[i], blackFillPaint);
            }

            Point[] point_oxy = new Point[GG._days];
            

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


            for (int i = 0; i < GG._days; i++)
            {
                series2.Points.Add(new DataPoint(stk_v2[i].s_date, stk_v2[i].s_dcp_int/100));
            }
            series2.Smooth = true;
            model.Series.Add(series2);
            

        }

        private void CreateLine()
        {
            var myModel = new PlotModel { Title = "Example 2" };
            myModel.Series.Add
                (
                new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)")
                );

            this.PlotChart.Model = myModel;
            
        }

    }
}
