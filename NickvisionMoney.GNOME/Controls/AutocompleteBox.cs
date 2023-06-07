using NickvisionMoney.GNOME.Helpers;
using System;
using System.Collections.Generic;

namespace NickvisionMoney.GNOME.Controls;

/// <summary>
/// A dialog for showing autocomplete suggestions
/// </summary>
public class AutocompleteBox : Gtk.Box
{
    private readonly List<Gtk.Widget> _rows;
    
    [Gtk.Connect] private readonly Adw.PreferencesGroup _group;
    
    public event EventHandler<string>? SuggestionClicked;
    
    /// <summary>
    /// Constructs an AutocompleteDialog
    /// </summary>
    /// <param name="builder">Gtk.Builder</param>
    private AutocompleteBox(Gtk.Builder builder) : base(builder.GetPointer("_root"), false)
    {
        _rows = new List<Gtk.Widget>();
        //Build UI
        builder.Connect(this);
    }
    
    /// <summary>
    /// Constructs an AutocompleteDialog
    /// </summary>
    public AutocompleteBox() : this(Builder.FromFile("autocomplete_box.ui"))
    {
    }
    
    /// <summary>
    /// Updates the list of suggestions
    /// </summary>
    /// <param name="suggestions">A list of suggestions</param>
    public void UpdateSuggestions(List<string> suggestions)
    {
        foreach(var row in _rows)
        {
            _group.Remove(row);
        }
        _rows.Clear();
        foreach(var suggestion in suggestions)
        {
            var row = Adw.ActionRow.New();
            row.SetTitle(suggestion);
            row.SetActivatable(true);
            row.OnActivated += (sender, e) =>
            {
                SuggestionClicked?.Invoke(this, row.GetTitle());
                SetVisible(false);
            };
            if(_rows.Count == 0)
            {
                row.SetSizeRequest(240, -1);
            }
            _rows.Add(row);
            _group.Add(row);
        }
    }
}