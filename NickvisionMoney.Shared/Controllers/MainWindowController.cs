﻿using NickvisionMoney.Shared.Events;
using NickvisionMoney.Shared.Helpers;
using NickvisionMoney.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NickvisionMoney.Shared.Controllers;

/// <summary>
/// A controller for a MainWindow
/// </summary>
public class MainWindowController
{
    private bool _isOpened;
    private List<string> _openAccounts;

    /// <summary>
    /// The localizer to get translated strings from
    /// </summary>
    public Localizer Localizer { get; init; }
    /// <summary>
    /// Gets the AppInfo object
    /// </summary>
    public AppInfo AppInfo => AppInfo.Current;
    /// <summary>
    /// A PreferencesViewController
    /// </summary>
    public PreferencesViewController PreferencesViewController => new PreferencesViewController(Localizer);
    /// <summary>
    /// Whether or not the version is a development version or not
    /// </summary>
    public bool IsDevVersion => AppInfo.Current.Version.IndexOf('-') != -1;
    /// <summary>
    /// The prefered theme of the application
    /// </summary>
    public Theme Theme => Configuration.Current.Theme;
    /// <summary>
    /// The list of recent accounts
    /// </summary>
    public List<string> RecentAccounts => Configuration.Current.RecentAccounts;
    /// <summary>
    /// The number of accounts opened
    /// </summary>
    public int NumberOfOpenAccounts => _openAccounts.Count;
    /// <summary>
    /// The path of the first opened account
    /// </summary>
    public string FirstOpenAccountPath => _openAccounts[0];

    /// <summary>
    /// Occurs when a notification is sent
    /// </summary>
    public event EventHandler<NotificationSentEventArgs>? NotificationSent;
    /// <summary>
    /// Occurs when an account is added
    /// </summary>
    public event EventHandler? AccountAdded;

    /// <summary>
    /// Constructs a MainWindowController
    /// </summary>
    public MainWindowController()
    {
        _isOpened = false;
        _openAccounts = new List<string>();
        Localizer = new Localizer();
    }

    /// <summary>
    /// Get the string for greeting on the start screen
    /// </summary>
    public string WelcomeMessage
    {
        get
        {
            var timeNowHours = DateTime.Now.Hour;
            if(timeNowHours >= 0 && timeNowHours < 6)
            {
                return Localizer["GreetingNight"];
            }
            else if(timeNowHours >= 6 && timeNowHours < 12)
            {
                return Localizer["GreetingMorning"];
            }
            else if(timeNowHours >= 12 && timeNowHours < 18)
            {
                return Localizer["GreetingDay"];
            }
            else if(timeNowHours >= 18 && timeNowHours < 24)
            {
                return Localizer["GreetingEvening"];
            }
            else
            {
                return Localizer["Greeting"];
            }
        }
    }

    /// <summary>
    /// Runs startup functions
    /// </summary>
    public void Startup()
    {
        if(!_isOpened)
        {
            _isOpened = false;
        }
    }

    /// <summary>
    /// Adds an account to the list of opened accounts
    /// </summary>
    /// <param name="path">string</param>
    public void AddAccount(string path)
    {
        if(Path.GetExtension(path) != ".nmoney")
        {
            path += ".nmoney";
        }
        if(!_openAccounts.Contains(path))
        {
            _openAccounts.Add(path);
            Configuration.Current.AddRecentAccount(path);
            Configuration.Current.Save();
            AccountAdded?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Closes the account with the provided index
    /// </summary>
    /// <param name="index">int</param>
    public void CloseAccount(int index) => _openAccounts.RemoveAt(index);

    public AccountViewController CreateAccountViewControllerForLatestAccount()
    {
        return new AccountViewController();
    }
}