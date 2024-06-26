using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using System;
using System.Windows;


namespace Zhukov_Proizv_Pract2;

public partial class AddEditForm : Window
{
    private List<Service> _Services;
    private Service CurrentServ;

    public AddEditForm(Service currentServ, List<Service> _services)
    {
        InitializeComponent();
        CurrentServ = currentServ;
        this.DataContext = currentServ;
        _Services = _services;
        FillCmb();
    }

    private MySqlConnection conn;
    string connStr = "server=localhost;database=abd1_11;port=3306;User Id=root;password=Qwerty_123456";

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var usr = _Services.FirstOrDefault(x => x.ID_услуги == CurrentServ.ID_услуги);
        if (usr == null)
        {
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                string add =
                    "INSERT INTO Service (Name, Price, TimeWork, Picture, Discont) VALUES ('" +
                    Name.Text + "', '" + Convert.ToDouble(Price.Text) + "', '" + Long.Text + " " + TimeComboBox.SelectedValue + "', '" + Img.Text + "', " +
                    Convert.ToInt32(Discount.Text) + ");";
                MySqlCommand cmd = new MySqlCommand(add, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error" + exception);
            }
        }
        else
        {
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                string upd = "UPDATE Service SET Name = '" + Name.Text +
                             "', Price = '" + Convert.ToInt32(Price.Text) + "', TimeWork = '" +
                             Long.Text + " " + TimeComboBox.SelectedValue + 
                             "', Picture = '" + Img.Text + "', Discont = '" +
                             Convert.ToInt32(Discount.Text) + "'  WHERE ID_service = " + Convert.ToInt32(ID.Text) + ";";
                MySqlCommand cmd = new MySqlCommand(upd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception exception)
            {
                Console.Write("Error" + exception);
            }
        }
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        Uslugi back = new Uslugi();
        this.Close();
        back.Show();
    }

    private async void File_Select(object sender, RoutedEventArgs e)
    {
        try
        {
            OpenFileDialog fileDialog = new OpenFileDialog(); //создание диалогового окна выбора файла
            fileDialog.Filters.Add(new FileDialogFilter() { Name = "Image Files", Extensions = { "jpg", "jpeg", "png", "gif" } }); //ограничение на выбор только изображений
            string[]? fileNames = await fileDialog.ShowAsync(this); //отображение диалогового окна и получение выбранных файлов
            if (fileNames != null && fileNames.Length > 0) //если файл выбран
            {
                string imagePath = System.IO.Path.GetFileName(fileNames[0]); //получение пути к выбранному файлу
                Img.Text = imagePath;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public void FillCmb()
    {
        var discontComboBox = this.Find<ComboBox>("TimeComboBox");
        discontComboBox.ItemsSource = new List<string>
        {
            "минут",
            "минуты",
            "часа",
            "часов"
        };
    }
}