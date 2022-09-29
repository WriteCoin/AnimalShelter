using System;
using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;

namespace AnimalShelter
{
    internal class GridAnimalShelter
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Telephone { get; set; }
        public string? Address { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для AnimalShelters.xaml
    /// </summary>
    public partial class AnimalShelters : Window
    {
        public AnimalShelters()
        {
            InitializeComponent();
            fillData();
        }

        private async void fillData()
        {
            using (animal_shelterContext db = new(MainWindow.dbOptions))
            {
                var data = await (from Shelter in db.Shelters
                                  select new GridAnimalShelter
                                  {
                                      Id = Shelter.Id,
                                      Name = Shelter.Name,
                                      Telephone = Shelter.Telephone,
                                      Address = Shelter.Address.FullAddr
                                  }).ToListAsync();
                grid.ItemsSource = data;
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("audio.wav");
            player.Play();
            bool isError = false;
            using (animal_shelterContext db = new(MainWindow.dbOptions))
            {
                try
                {
                    foreach (var Item in (dynamic)grid.ItemsSource)
                    {
                        if (isError)
                        {
                            break;
                        }
                        string fullAddr = Item.Address;
                        Address address = await db.Addresses.FirstOrDefaultAsync(a => a.FullAddr == fullAddr);
                        Address newAddress;
                        if (address != null)
                        {
                            newAddress = address;
                        }
                        else
                        {
                            newAddress = new Address
                            {
                                City = "",
                                Street = "",
                                House = 0,
                                UserId = 0,
                                FullAddr = fullAddr
                            };
                            db.Addresses.Add(newAddress);
                            //await db.SaveChangesAsync();
                            //newAddress = await db.Addresses.FirstOrDefaultAsync(a => a.FullAddr == fullAddr);
                            Address lastAddress = await db.Addresses.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
                            newAddress.Id = lastAddress.Id + 1;
                        }

                        Shelter data = new Shelter { Id = Item.Id, Name = Item.Name, Telephone = Item.Telephone, AddressId = newAddress.Id };
                        Shelter shelter = await db.Shelters.FirstOrDefaultAsync(sh => sh.Id == data.Id);
                        if (data != null)
                        {
                            shelter.Name = data.Name;
                            shelter.Telephone = data.Telephone;
                            shelter.AddressId = data.AddressId;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // MessageBox.Show($"Ошибка {ex.Message} {ex.StackTrace}");
                    MessageBox.Show("Ошибка");
                    isError = true;
                }
                if (!isError)
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Данные изменены успешно");
                }
            }
        }

        private async void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("audio.wav");
            player.Play();
            bool isError = false;
            using (animal_shelterContext db = new(MainWindow.dbOptions))
            {
                try
                {
                    foreach (var Item in (dynamic)gridAdd.ItemsSource)
                    {
                        string fullAddr = Item.Address;
                        Address address = await db.Addresses.FirstOrDefaultAsync(a => a.FullAddr == fullAddr);
                        Address newAddress;
                        if (address != null)
                        {
                            newAddress = address;
                        }
                        else
                        {
                            newAddress = new Address
                            {
                                City = "",
                                Street = "",
                                House = 0,
                                UserId = 0,
                                FullAddr = fullAddr
                            };
                            db.Addresses.Add(newAddress);
                            //await db.SaveChangesAsync();
                            //newAddress = await db.Addresses.FirstOrDefaultAsync(a => a.FullAddr == fullAddr);
                            Address lastAddress = await db.Addresses.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
                            newAddress.Id = lastAddress.Id + 1;
                        }

                        Shelter data = new Shelter { Name = Item.Name, Telephone = Item.Telephone, AddressId = newAddress.Id };
                        db.Shelters.Add(data);
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                    isError = true;
                }
                if (!isError)
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Данные добавлены успешно");
                }
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("audio.wav");
            player.Play();
            bool isError = false;
            using (animal_shelterContext db = new(MainWindow.dbOptions))
            {
                try
                {
                    foreach (var Item in (dynamic)gridAdd.ItemsSource)
                    {
                        string fullAddr = Item.Address;
                        Address address = await db.Addresses.FirstOrDefaultAsync(a => a.FullAddr == fullAddr);
                        Shelter data = new Shelter { Name = Item.Name, Telephone = Item.Telephone, AddressId = address.Id };
                        Shelter shelter = await db.Shelters.FirstOrDefaultAsync(sh => sh.Name == data.Name && sh.Telephone == data.Telephone && sh.AddressId == data.AddressId);
                        db.Shelters.Remove(shelter);
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка: Приют для животных не найден");
                    isError = true;
                }
                if (!isError)
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Данные удалены успешно");
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)this.Owner).AnimalSheltersWindow = null;
        }
    }
}
