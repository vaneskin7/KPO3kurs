using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KPO3kurs
{
    public partial class LanguageWindow : Window
    {
        public LanguageWindow()
        {
            InitializeComponent();

            App.LanguageChanged += LanguageChanged;

            CultureInfo currLang = App.Language;

            menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                
                MenuItem menuLang = new MenuItem();
                switch (Convert.ToString(lang))
                {
                    case "en-US":
                        menuLang.Header = "English (USA)";
                        break;
                    case "ru-RU":
                        menuLang.Header = "Русский (Россия)";
                        break;
                    default:
                        break;
                }
                
                menuLang.Tag = lang;
                if (lang.Equals(currLang) == false)
                {
                    menuLang.IsChecked = true;
                }
                
                menuLang.Click += ChangeLanguageClick;
                menuLanguage.Items.Add(menuLang);
            }
            
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;

            //Отмечаем нужный пункт смены языка как выбранный язык
            foreach (MenuItem i in menuLanguage.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }

        }


    }
}
