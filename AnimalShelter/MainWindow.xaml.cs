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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using BCrypt.Net;

namespace AnimalShelter
{
    internal class GridRequest
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string RegDate { get; set; }
        public int UserId { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DbContextOptions<animal_shelterContext> dbOptions = animal_shelterContext.GetOptions();
        public static Admin? LoggedUser;
        public AnimalShelters? AnimalSheltersWindow;
        public Animals? AnimalsWindow;

        public MainWindow()
        {
            Logic();
        }

        private async void Logic()
        {
            await PreAuth();
            InitializeComponent();
            await fillTypes();
            await fillStatuses();
        }

        private async Task PreAuth()
        {
            PasswordWindow passwordWindow = new();

            if (passwordWindow.ShowDialog() == true)
            {
                if (!(await authValidate(passwordWindow.UserEmail, passwordWindow.UserPassword)))
                {
                    await PreAuth();
                }
                else
                {
                    Show();
                }
            }
            else
            {
                Close();
            }

        }

        private async Task<bool> authValidate(string userEmail, string userPassword)
        {
            if (userEmail.Length == 0)
            {
                MessageBox.Show("Введите E-mail");
                return false;
            }
            if (userPassword.Length == 0)
            {
                MessageBox.Show("Введите пароль");
                return false;
            }

            using (animal_shelterContext db = new(dbOptions))
            {
                Person? person = await (from Person in db.People
                                        where Person.Email == userEmail
                                        select Person).FirstOrDefaultAsync();
                if (person == null)
                {
                    MessageBox.Show("Пользователь не найден");
                    return false;
                }

                Admin? @admin = await (from Admin in db.Admins
                                             where Admin.PersonId == person.Id
                                             select Admin).FirstOrDefaultAsync();
                if (@admin == null)
                {
                    MessageBox.Show("Нет прав для входа");
                    return false;
                }

                bool checkPassword = BCrypt.Net.BCrypt.Verify(userPassword.Trim(), @admin.Password);

                if (!checkPassword)
                {
                    MessageBox.Show("Неверный пароль");
                    return false;
                }

                LoggedUser = admin;
                /*MessageBox.Show("Пользователь авторизован");*/
                return true;
            }
        }

        private async void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser = null;
            Hide();
            if (AnimalSheltersWindow != null)
            {
                AnimalSheltersWindow.Close();
            }
            if (AnimalsWindow != null)
            {
                AnimalsWindow.Close();
            }
            /*MessageBox.Show("Вы вышли из системы");*/
            await PreAuth();
        }

        private async Task fillTypes()
        {
            using (animal_shelterContext db = new(dbOptions))
            {
                var typeNames = await (from RequestType in db.RequestTypes select RequestType.Name).ToListAsync();
                requestTypesList.ItemsSource = typeNames;
                requestTypesList.SelectedValue = typeNames[0];
            }
        }

        private async Task fillStatuses()
        {
            using (animal_shelterContext db = new(dbOptions))
            {
                var statusNames = await (from RequestStatus in db.RequestStatuses select RequestStatus.Name).ToListAsync();
                requestStatusesList.ItemsSource = statusNames;
                requestStatusesList.SelectedValue = statusNames[0];
            }
        }

        private async void btnFillGrid_Click(object sender, RoutedEventArgs e)
        {
            using (animal_shelterContext db = new(dbOptions))
            {
                try
                {
                    //Request.RegDate.ToDateTime(new()).CompareTo(regDateMin.SelectedDate) >= 0 && Request.RegDate.ToDateTime(new()).CompareTo(regDateMax.SelectedDate ) <= 0
                    var requests = await (from Request in db.Requests
                                          //join RequestType in db.RequestTypes on Request.RequestTypeId equals RequestType.Id
                                          //join RequestStatus in db.RequestStatuses on Request.RequestStatusId equals RequestStatus.Id
                                          where
                                            Request.RequestType.Name == (string)requestTypesList.SelectedValue &&
                                            Request.RequestStatus.Name == (string)requestStatusesList.SelectedValue &&
                                            Request.RegDate.ToDateTime(new()) >= regDateMin.SelectedDate && Request.RegDate.ToDateTime(new()) <= regDateMax.SelectedDate
                                          select new GridRequest
                                          {
                                              Id = Request.Id,
                                              Type = Request.RequestType.Name,
                                              Status = Request.RequestStatus.Name,
                                              RegDate = Request.RegDate.ToShortDateString(),
                                              UserId = Request.UserId,
                                              Telephone = Request.ContactTelephone,
                                              Email = Request.ContactEmail,
                                              Address = Request.Address.FullAddr
                                          }).ToListAsync();

                    grid.ItemsSource = requests;
                } catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void btnAnimalShelters_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalSheltersWindow == null)
            {
                AnimalSheltersWindow = new();
                AnimalSheltersWindow.Owner = this;
                AnimalSheltersWindow.Show();
            } else
            {
                AnimalSheltersWindow.Focus();
            }
        }

        private void btnAnimals_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalsWindow == null)
            {
                AnimalsWindow = new();
                AnimalsWindow.Owner = this;
                AnimalsWindow.Show();
            }
            else
            {
                AnimalsWindow.Focus();
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool isError = false;
            using (animal_shelterContext db = new(dbOptions))
            {
                try
                {
                    foreach (var Item in (dynamic)grid.ItemsSource)
                    {
                        if (isError)
                        {
                            break;
                        }

                        string requestType = Item.Type;
                        var TypeRequest = await (from RequestType in db.RequestTypes where RequestType.Name == requestType select RequestType.Id).ToListAsync();
                        int RequestTypeId = TypeRequest[0];

                        string requestStatus = Item.Status;
                        var StatusRequest = await (from RequestStatus in db.RequestStatuses where RequestStatus.Name == requestStatus select RequestStatus.Id).ToListAsync();
                        int RequestStatusId = StatusRequest[0];

                        string fullAddr = Item.Address;
                        Address address = await db.Addresses.FirstOrDefaultAsync(a => a.FullAddr == fullAddr);
                        Address newAddress;
                        if (address != null)
                        {
                            newAddress = address;
                        } else
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

                        Request data = new Request { 
                            Id = Item.Id,
                            RequestTypeId = RequestTypeId,
                            RegDate = DateOnly.Parse(Item.RegDate),
                            UserId = Item.UserId,
                            ContactTelephone = Item.Telephone,
                            ContactEmail = Item.Email,
                            AddressId = newAddress.Id,
                            RequestStatusId = RequestStatusId
                        };
                        Request request = await db.Requests.FirstOrDefaultAsync(r => r.Id == data.Id);
                        if (request != null)
                        {
                            request.Id = data.Id;
                            request.RequestTypeId = data.RequestTypeId;
                            request.RegDate = data.RegDate;
                            request.UserId = data.UserId;
                            request.ContactTelephone = data.ContactTelephone;
                            request.ContactEmail = data.ContactEmail;
                            request.AddressId = data.AddressId;
                            request.RequestStatusId = data.RequestStatusId;
                        }

                        //PointsOfIssue data = new PointsOfIssue { Id = Item.Id, Name = Item.Name, Address = Item.Address, WorkTimeStart = TimeOnly.Parse(Item.WorkTimeStart), WorkTimeEnd = TimeOnly.Parse(Item.WorkTimeEnd) };
                        //PointsOfIssue point = await db.PointsOfIssues.FirstOrDefaultAsync(p => p.Id == data.Id);
                        //if (point != null)
                        //{
                        //    point.Id = data.Id;
                        //    point.Name = data.Name;
                        //    point.Address = data.Address;
                        //    point.WorkTimeStart = data.WorkTimeStart;
                        //    point.WorkTimeEnd = data.WorkTimeEnd;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка {ex.Message} {ex.StackTrace}");
                    //MessageBox.Show("Ошибка");
                    isError = true;
                }
                if (!isError)
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Данные изменены успешно");
                }
            }
        }
    }
}
