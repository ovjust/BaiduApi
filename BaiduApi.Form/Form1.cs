using BaiduApi.BaseApi;
using BaiduApi.BaseApi.Stock;
using DotNet_Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace BaiduApi.WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

 

 

        private void button1_Click(object sender, EventArgs e)
        {
            var imgStr = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDABMNDxEPDBMREBEWFRMXHTAfHRsbHTsqLSMwRj5KSUU+RENNV29eTVJpU0NEYYRiaXN3fX59S12Jkoh5kW96fXj/2wBDARUWFh0ZHTkfHzl4UERQeHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHj/wAARCAAfACEDAREAAhEBAxEB/8QAGAABAQEBAQAAAAAAAAAAAAAAAAQDBQb/xAAjEAACAgICAgEFAAAAAAAAAAABAgADBBESIRMxBSIyQXGB/8QAFAEBAAAAAAAAAAAAAAAAAAAAAP/EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/APawEBAQEBAgy8i8ZTVV3UY6V1eU2XoWDDZB19S646Gz39w9fkKsW1r8Wm2yo1PYis1be0JG9H9QNYCAgc35Cl3yuVuJZl0cB41rZQa32dt2y6OuOiOxo61vsLcVblxaVyXD3hFFjL6La7I/sDWAgICAgICB/9k=";
           var img= ImageToString.FromBase64String(imgStr);
           img.Save("test2.jpg");
           var img2 = ImageIO.ReadFile("test2.jpg");
           var str2 = ImageToString.ToBase64String(img2, ImageFormat.Jpeg);
            
           var b = imgStr == str2;
           var str3 = ImageToString.ToBase64String(img);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var img = ImageIO.ReadFile("test1.jpg");
            var imgStr=ImageToString.ToBase64String(img,ImageFormat.Jpeg);
            imgStr = HttpUtility.UrlEncode(imgStr);
            var words = BaiduOcr.GetResult(imgStr);
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            //BaiduStock.GetResult(txtStock.Text);
            TencentStock.GetStockLong(new string[]{ txtStock.Text});
        }

        private void btnRangePrice_Click(object sender, EventArgs e)
        {
         var dd=   SinaStock.GetDatesPriceSlice(txtStock.Text, dateTimePicker1.Value, dateTimePicker2.Value);
        }

    }
}
