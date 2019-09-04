using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class OrderService
    {

        /// <summary>
        /// Utworzenie zamównienia
        /// </summary>
        /// <param name="profile">Profil z którego została wykonana aktualizacja</param>
        /// <returns>Nowe zamówienie</returns>
        public Order CreateOrder(Profile profile)
        {
            return new Order()
            {
                AdderId = profile.Id,
                ModifierId = profile.Id,
                Added = DateTime.Now,
                Updated = DateTime.Now,
                Status = Status.New
            };
        }

        /// <summary>
        /// Anulowanie zamówienia
        /// </summary>
        /// <param name="order">Zamówienie</param>
        /// <param name="profile">Profil z którego została wykonana aktualizacja</param>
        /// <returns>Anulowane zamówienie</returns>
        public Order Cancel(Order order, Profile profile)
        {
            order.Status = Status.Cancelled;
            order.Updated = DateTime.Now;
            order.ModifierId = profile.Id;
            return order;
        }

        /// <summary>
        /// Zwrócone zamówienie
        /// </summary>
        /// <param name="order">Zamówienie dostarczone</param>
        /// <param name="profile">Profil z którego została wykonana aktualizacja</param>
        /// <returns>Zwrócone zamówienie</returns>
        public Order Return(Order order, Profile profile)
        {
            order.Status = Status.Returned;
            order.Updated = DateTime.Now;
            order.ModifierId = profile.Id;

            return order;
        }

        /// <summary>
        /// Zamówienie kompletne. Oczekuje na opłatę
        /// </summary>
        /// <param name="order">Zamówienie nowe</param>
        /// <param name="profile">Profil z którego została wykonana aktualizacja</param>
        /// <param name="profile">Profil z którego została wykonana aktualizacja</param>
        /// <returns>Zamówienie oczekujące na opłacenie</returns>
        public Order ToPay(Order order, Profile profile)
        {
            order.Status = Status.ToPay;
            order.Updated = DateTime.Now;
            order.ModifierId = profile.Id;

            return order;
        }

        /// <summary>
        /// Opłacenie zamówienia
        /// </summary>
        /// <param name="order">Zamówienie oczekujące na opłacenie</param>
        /// <param name="profile">Profil z którego została wykonana aktualizacja</param>
        /// <returns>Zamówienie opłacone</returns>
        public Order Payed(Order order, Profile profile)
        {
            order.Status = Status.Payed;
            order.Updated = DateTime.Now;
            order.ModifierId = profile.Id;

            return order;
        }

        /// <summary>
        /// Zamówienie przeznaczone do dostarczenia
        /// </summary>
        /// <param name="order"></param>
        /// <param name="profile">Profil z którego została wykonana aktualizacja</param>
        /// <returns></returns>
        public Order Deliving(Order order, Profile profile)
        {
            order.Status = Status.Deliving;
            order.Updated = DateTime.Now;
            order.ModifierId = profile.Id;

            return order;
        }

        /// <summary>
        /// Zamówienie zostało dostarczone
        /// </summary>
        /// <param name="order">Zamówienie w trakcie dostarczania</param>
        /// <param name="profile">Profil z którego została wykonana aktualizacja</param>
        /// <returns>Zamówienie dostarczone</returns>
        public Order Delived(Order order, Profile profile)
        {
            order.Status = Status.Delived;
            order.Updated = DateTime.Now;
            order.ModifierId = profile.Id;

            return order;
        }

        public OrderList OrderList(ICollection<Order> orders)
        {
            List <OrderDetails> list = new List<OrderDetails>();

            foreach(Order order in orders)
            {
                list.Add(
                    this.OrderDetails(order)
                );
            }

            return new OrderList()
            {
                OrderDetails = list
            };
        }

        public OrderDetails OrderDetails(Order order)
        {
            return new OrderDetails()
            {
                Id = order.Id,
                Created = order.Added,
                Status = order.Status
            };
        }
    }
}