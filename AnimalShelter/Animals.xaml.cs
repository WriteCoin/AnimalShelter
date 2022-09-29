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

namespace AnimalShelter
{
    internal class GridAnimal
    {
        public string Type { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }
        public int Age { get; set; }
        public string Vaccinated { get; set; }
        public string ShelterName { get; set; }
        public string ShelterTelephone { get; set; }
        public string ShelterAddress { get; set; }
        public string AdoptionDate { get; set; }
        public string TermShelter { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для Animals.xaml
    /// </summary>
    public partial class Animals : Window
    {
        public Animals()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)this.Owner).AnimalsWindow = null;
        }

        private async void btnGetAnimals_Click(object sender, RoutedEventArgs e)
        {
            using (animal_shelterContext db = new(MainWindow.dbOptions))
            {
                try
                {
                    bool AgeCondIgnore = true;
                    bool TypeCondIgnore = true;
                    bool BreedCondIgnore = true;
                    bool ColorCondIgnore = true;
                    bool ShelterCondIgnore = true;

                    string AgeMinText = ageMin.Text;
                    string AgeMaxText = ageMax.Text;

                    int AgeMin = 1, AgeMax = 100;

                    if (!(AgeMinText == "" || AgeMaxText == ""))
                    {
                        AgeMin = int.Parse(AgeMinText);
                        AgeMax = int.Parse(AgeMaxText);

                        if (AgeMin < 0 || AgeMax < 0)
                        {
                            throw new Exception("Возраст не может быть отрицательным");
                        }

                        if (AgeMin == 0 || AgeMax == 0)
                        {
                            throw new Exception("Возраст не может быть нулевым");
                        }

                        if (AgeMin > AgeMax)
                        {
                            throw new Exception("Минимальный возраст не может быть больше максимального");
                        }

                        AgeCondIgnore = false;
                    }

                    if (!(type.Text == ""))
                    {
                        TypeCondIgnore = false;
                    }

                    if (!(breed.Text == ""))
                    {
                        BreedCondIgnore = false;
                    }

                    if (!(color.Text == ""))
                    {
                        ColorCondIgnore = false;
                    }

                    if (!(shelter.Text == ""))
                    {
                        ShelterCondIgnore = false;
                    }

                    var animals = (from Animal in db.Animals
                                   join ShelterAnimal in db.ShelterAnimals on Animal.Id equals ShelterAnimal.AnimalId
                                   where
                                       (TypeCondIgnore || Animal.Type == type.Text) &&
                                       (BreedCondIgnore || Animal.Breed == breed.Text) &&
                                       (ColorCondIgnore || Animal.Color == color.Text) &&
                                       (AgeCondIgnore || Animal.Age >= AgeMin && Animal.Age <= AgeMax) &&
                                       (ShelterCondIgnore || ShelterAnimal.Shelter.Name == shelter.Text) &&
                                       ShelterAnimal.AdoptionDate.ToDateTime(new()) >= adoptionDateMin.SelectedDate && ShelterAnimal.AdoptionDate.ToDateTime(new()) <= adoptionDateMax.SelectedDate &&
                                       ShelterAnimal.TermShelter.ToDateTime(new()) >= termShelterMin.SelectedDate && ShelterAnimal.TermShelter.ToDateTime(new()) <= termShelterMax.SelectedDate
                                   select new GridAnimal
                                   {
                                       Type = Animal.Type ?? "",
                                       Breed = Animal.Breed ?? "",
                                       Color = Animal.Color ?? "",
                                       Age = Animal.Age,
                                       Vaccinated = Animal.Vaccinated ? "Да" : "Нет",
                                       ShelterName = ShelterAnimal.Shelter.Name ?? "",
                                       ShelterTelephone = ShelterAnimal.Shelter.Telephone ?? "",
                                       ShelterAddress = ShelterAnimal.Shelter.Address.FullAddr ?? "",
                                       AdoptionDate = ShelterAnimal.AdoptionDate.ToShortDateString(),
                                       TermShelter = ShelterAnimal.TermShelter.ToShortDateString(),
                                   }).ToList();

                    grid.ItemsSource = animals;


                    // int OrderNumber = orderNumber.Text == "" ? 0 : int.Parse(orderNumber.Text);
                    // var orders = await (from Order in db.Orders
                    //                     join WaysToReceive in db.WaysToReceives on Order.WayToReceiveId equals WaysToReceive.Id
                    //                     join PaymentMethod in db.PaymentMethods on Order.PaymentMethodId equals PaymentMethod.Id
                    //                     join PointsOfIssue in db.PointsOfIssues on Order.PointOfIssueId equals PointsOfIssue.Id
                    //                     join OrderStatus in db.OrderStatuses on Order.OrderStatusId equals OrderStatus.Id
                    //                     join Client in db.Clients on Order.ClientId equals Client.Id
                    //                     where WaysToReceive.WayToReceiveName == wayToReceive.SelectedValue &&
                    //                           PaymentMethod.PaymentMethodName == paymentMethod.SelectedValue &&
                    //                           OrderStatus.OrderStatusName == orderStatus.SelectedValue &&
                    //                           (OrderNumber == 0 || Order.OrderNumber == OrderNumber)
                    //                     select new GridOrder
                    //                     {
                    //                         Id = Order.Id,
                    //                         OrderNumber = Order.OrderNumber.ToString(),
                    //                         ContactName = Order.ContactName,
                    //                         ContactEmail = Order.ContactEmail,
                    //                         ContactPhone = Order.ContactPhone,
                    //                         WayToReceive = WaysToReceive.WayToReceiveName,
                    //                         PaymentMethod = PaymentMethod.PaymentMethodName,
                    //                         OrderStatus = OrderStatus.OrderStatusName,
                    //                         PointOfIssue = PointsOfIssue.Name,
                    //                         RegDate = Order.RegDate.ToShortDateString(),
                    //                         DateOfReceipt = Order.DateOfReceipt.ToShortDateString(),
                    //                         ActualDateOfReceipt = Order.ActualDateOfReceipt.ToShortDateString(),
                    //                         Price = Order.Price.ToString(),
                    //                         RegTime = Order.RegTime.ToShortTimeString(),
                    //                         ReceiptTime = Order.ReceiptTime.ToShortTimeString(),
                    //                         ActualReceiptTime = Order.ActualReceiptTime.ToString(),
                    //                     }).ToListAsync();
                    // grid.ItemsSource = orders;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка");
                }
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
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


                        // string format = "HH:mm";
                        // string orderStatus = Item.OrderStatus;
                        // var StatusOrder = await (from OrderStatus in db.OrderStatuses where OrderStatus.OrderStatusName == orderStatus select OrderStatus.Id).ToListAsync();
                        // int OrderStatusId = StatusOrder[0];
                        // Order data = new Order { Id = Item.Id, DateOfReceipt = DateOnly.Parse(Item.DateOfReceipt), ActualDateOfReceipt = DateOnly.Parse(Item.ActualDateOfReceipt), ReceiptTime = DateTime.Parse(Item.ReceiptTime), ActualReceiptTime = DateTime.Parse(Item.ActualReceiptTime), OrderStatusId = OrderStatusId };
                        // // MessageBox.Show(data.Id.ToString());
                        // Order order = await db.Orders.FirstOrDefaultAsync(o => o.Id == data.Id);
                        // order.DateOfReceipt = data.DateOfReceipt;
                        // order.ActualDateOfReceipt = data.ActualDateOfReceipt;
                        // order.ReceiptTime = data.ReceiptTime;
                        // order.ActualReceiptTime = data.ActualReceiptTime;
                        // order.OrderStatusId = data.OrderStatusId;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Ошибка {ex.Message} {ex.StackTrace}");
                    // MessageBox.Show("Ошибка");
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
