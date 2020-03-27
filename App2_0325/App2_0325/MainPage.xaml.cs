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
        
        public MainPage()
        {
            InitializeComponent();
            
        }
        
        int count = 0;
        private void Button_Clicked_Search(object sender, EventArgs e)
        {
            count++;
            EDI1.Text = "Clicked!!!" + count;
            
        }

        private void Button_Clicked_erase(object sender, EventArgs e)
        {
            count = 0;
            EDI1.Text = "CLEAR";
        }

        private void Button_Clicked_Test(object sender, EventArgs e)
        {
            sangjang sj = new sangjang();
            
            // enter 제거?
            string search_data = DATA.Text.ToString(); 
            
            EDI1.Text = "";
        
            for(int i = 0; i <3721; i++)
            {
                if ( sj.jongmok[i].Equals(search_data))
                {
                    EDI1.Text = sj.jongmok[i].ToString() + "\t";
                    EDI1.Text += sj.company[i].ToString();
                    break;
                }
                if (i == 3720) EDI1.Text = "ERR";
            }
        }
    }
}
