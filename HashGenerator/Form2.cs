using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Diagnostics;

namespace HashGenerator
{
    public partial class Form2 : MaterialForm
    {
        public Form2()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(materialButton1, "Icon Credits");
            toolTip1.SetToolTip(materialButton2, "GitHub Profile");
            toolTip1.SetToolTip(materialButton3, "Report errors or bugs");
            toolTip1.SetToolTip(materialButton4, "Give coffee or food :)");
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            Process.Start("https://icons8.com/icons/set/hashtag-activity-feed");
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EmanDev");
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/EmanDev/Hash-Generator/issues");
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.me/marcaida");
        }
    }
}
