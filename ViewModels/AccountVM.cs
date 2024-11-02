using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Test_Account.Models;
using Test_Account.Utilities;

namespace Test_Account.ViewModels
{
    public class AccountVM : BaseVM
    {
        private Account _selecteditem;
        public Account SelectedItem
        {
            get { return _selecteditem; }
            set
            {
                _selecteditem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if (SelectedItem != null)
                {
                    NewItem = new Account
                    {
                        AccId = SelectedItem.AccId,
                        AccStatus = SelectedItem.AccStatus,
                        Address = SelectedItem.Address,
                        Dob = SelectedItem.Dob,
                        Email = SelectedItem.Email,
                        FullName = SelectedItem.FullName,
                        Password = SelectedItem.Password,
                        Role = SelectedItem.Role,
                        Sex = SelectedItem.Sex,
                    };
                    OnPropertyChanged(nameof(NewItem));
                }
            }

        }
        private Account _newitem;
        public Account NewItem
        {
            get { return _newitem; }
            set
            {
                _newitem = value;
                OnPropertyChanged(nameof(NewItem));

            }

        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }

        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<RoleAccount> Roles { get; set; }
        //Constructor
        public AccountVM()
        {
            Accounts = new ObservableCollection<Account>();
            Roles = new ObservableCollection<RoleAccount>();
            NewItem = new();
            AddCommand = new RelayCommand(ADD);
            UpdateCommand = new RelayCommand(UPDATE);
            DeleteCommand = new RelayCommand(DELETE);
            Load();
        }

        private void DELETE(object obj)
        {
            using (var context = new TestAccountContext())
            {
                if(SelectedItem != null)
                {
                    var item = context.Accounts.Where(x => x.AccId == SelectedItem.AccId).FirstOrDefault();
                    if (item != null)
                    {
                        context.Accounts.Remove(item);
                        context.SaveChanges();
                        Load();
                        SelectedItem = null;
                        NewItem = new();
                    }
                }
            }
        }

        private void UPDATE(object obj)
        {
            using (var context = new TestAccountContext())
            {
                if (SelectedItem != null)
                {
                    var item = context.Accounts.Where(x => x.AccId == SelectedItem.AccId).FirstOrDefault();
                    if (item != null)
                    {
                        item.FullName = NewItem.FullName;
                        item.Address = NewItem.Address;
                        item.Email = NewItem.Email;
                        item.Password = NewItem.Password;
                        item.Role = NewItem.Role;
                        item.Dob = NewItem.Dob;
                        item.Sex = NewItem.Sex;

           
                        item.AccStatus = NewItem.AccStatus;

                        context.SaveChanges();

         
                        Load();

        
                        SelectedItem = null;
                        NewItem = new();

                        MessageBox.Show("Account updated successfully!", "Successfully!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }



        private void ADD(object obj)
        {
            using (var context = new TestAccountContext()) {
                NewItem.AccId = 0;
                NewItem.AccStatus = 1;
                context.Accounts.Add(NewItem);
                context.SaveChanges();
                Load();
                NewItem = new();
                MessageBox.Show("Account is added successfully!", "Successfully!",MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Load()
        {
            using (var context = new TestAccountContext())
            {
                var items = context.Accounts.Include(x => x.RoleNavigation).ToList();
                Accounts.Clear();
                foreach (var item in items)
                {
                    if (item != null)
                    {
                        Accounts.Add(item);
                    }

                }
                Roles.Clear();
                var roles = context.RoleAccounts.ToList();
                foreach (var item in roles)
                {
                    Roles.Add(item);
                }
            }
        }
    }
}

