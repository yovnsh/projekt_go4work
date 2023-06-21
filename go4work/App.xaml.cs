﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using go4work.Contexts;
using go4work.Models;

namespace go4work
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// baza danych - dostępna dla całej aplikacji
        /// </summary>
        public static JobContext db = new JobContext();
        public static User logged_user;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        
            // sprawdzenie czy cokolwiek jest w bazie danych
            try
            {
                // generowanie przykładowych hoteli
                if (!db.Hotels.Any())
                {
                    if (!GenerateSampleHotels())
                    {
                        // jeśli się nie udało to nie ma sensu dalej działać
                        return;
                    }
                }

                // generowanie przykładowych ofert pracy
                if (!db.JobOffers.Any())
                {
                    GenerateSampleOffers();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Nie można się połączyć z bazą danych");
                this.Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            db.Dispose();

            base.OnExit(e);
        }

        /// <summary>
        /// dodaje przykładowe oferty pracy
        /// </summary>
        private void GenerateSampleOffers()
        {
            try
            {
                db.JobOffers.Add(new Models.JobOffer {
                    Hotel = db.Hotels.FirstOrDefault(h => h.Name == "Mariott"),
                    Date = DateTime.Now.AddMonths(1),
                    ShiftStart = 8,
                    ShiftEnd = 16,
                    Salary = 100
                });
                db.JobOffers.Add(new Models.JobOffer
                {
                    Hotel = db.Hotels.FirstOrDefault(h => h.Name == "Mariott"),
                    Date = DateTime.Now.AddMonths(1).AddDays(1),
                    ShiftStart = 6,
                    ShiftEnd = 12,
                    Salary = 80
                });
                db.JobOffers.Add(new Models.JobOffer() {
                    Hotel = db.Hotels.FirstOrDefault(h => h.Name == "Hilton"),
                    Date = DateTime.Now.AddMonths(1),
                    ShiftStart = 18,
                    ShiftEnd = 6,
                    Salary = 200
                });
                db.JobOffers.Add(new Models.JobOffer() { 
                    Hotel = db.Hotels.FirstOrDefault(h => h.Name == "Ibis"),
                    Date = DateTime.Now.AddMonths(1).AddDays(5),
                    ShiftStart = 16,
                    ShiftEnd = 22,
                    Salary = 80
                });
                db.JobOffers.Add(new Models.JobOffer()
                {
                    Hotel = db.Hotels.FirstOrDefault(h => h.Name == "Mariott"),
                    Date = DateTime.Now.AddMonths(1).AddDays(7),
                    ShiftStart = 8,
                    ShiftEnd = 16,
                    Salary = 110
                });
                db.JobOffers.Add(new Models.JobOffer()
                {
                    Hotel = db.Hotels.FirstOrDefault(h => h.Name == "Ibis"),
                    Date = DateTime.Now.AddMonths(2).AddDays(3),
                    ShiftStart = 8,
                    ShiftEnd = 20,
                    Salary = 150
                });
                db.JobOffers.Add(new Models.JobOffer()
                {
                    Hotel = db.Hotels.FirstOrDefault(h => h.Name == "Hilton"),
                    Date = DateTime.Now.AddMonths(0).AddDays(10),
                    ShiftStart = 5,
                    ShiftEnd = 13,
                    Salary = 80
                });
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Błąd generowania ofert pracy: {e.Message}");
            }
        }

        /// <summary>
        /// dodaje przykładowe hotele
        /// </summary>
        /// <returns>czy operacja się powiodła</returns>
        private bool GenerateSampleHotels()
        {
            try
            {
                db.Hotels.Add(new Models.Hotel()
                {
                    Name = "Mariott"
                });
                db.Hotels.Add(new Models.Hotel()
                {
                    Name = "Hilton"
                });
                db.Hotels.Add(new Models.Hotel()
                {
                    Name = "Ibis"
                });
                db.Add(new Models.Hotel()
                {
                    Name = "Novotel"
                });
                db.SaveChanges();
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Błąd generowania hoteli: {e.Message}");
                return false;
            }
            return true;
        }
    }
}