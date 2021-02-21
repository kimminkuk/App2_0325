using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//namespace
using App2_0325.ViewModels;
using App2_0325.Models;


namespace App2_0325
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NextPage1 : ContentPage
    {
        string str ="";
        public NextPage1(string jusik_code_)
        {
            InitializeComponent();
            BindingContext = new html_addr();
            ///*21-02-13*/
            //html_addr HA = (html_addr)BindingContext;
            //HA.html_png_parsing("005380", Kind_Constants.kinds_day);
            str = jusik_code_;

            html_addr HA = (html_addr)BindingContext;
            HA.html_png_parsing(jusik_code_, Kind_Constants.kinds_day);

        }


        private void nextButton_Clicked(object sender, EventArgs e)
        {
            html_addr HA = (html_addr)BindingContext;
            HA.html_png_parsing(str, Kind_Constants.kinds_day);
        }

        async private void previousButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}