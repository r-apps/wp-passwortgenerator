using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Passwortgenerator
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Einstellungen : Page
    {
        public static int pwAnzahl = 5;
        public static PWGenerator.PasswortManager.PasswortArt pwArt = new PWGenerator.PasswortManager.PasswortArt() { anzahlZeichen = 20, buchstaben = PWGenerator.PasswortManager.BuchstabenArt.großKlein, sonderzeichen = "äöüß[]*♥!=(){}\"\\~%?.-_₀₁₂₃₄₅₆₇₈₉±¾↑&↓·←↗¤¼→™½¢ªµ´¯⁰¹²³⁴⁵⁶⁷⁸⁹♀¬°£$♂†…¥÷®º€¸§¶¨∞©", zahlen = true };
        public Einstellungen()
        {
            this.InitializeComponent();
            SystemNavigationManager manager = SystemNavigationManager.GetForCurrentView();
            manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            manager.BackRequested += Manager_BackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            sliderlänge.Value = pwArt.anzahlZeichen;
            cbBuchstaben.SelectedIndex = (int)pwArt.buchstaben;
            tbSonder.Text = pwArt.sonderzeichen;
            switchPW.IsOn = pwArt.zahlen;
            slideranzahl.Value = pwAnzahl;
            base.OnNavigatedTo(e);
        }

        private void abbSave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Speichern();
            Frame.GoBack();
        }

        private void abbAbbrechen_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SystemNavigationManager manager = SystemNavigationManager.GetForCurrentView();
            manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Frame.GoBack();
        }

        private void Manager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Speichern();
        }

        private void Speichern()
        {
            pwArt.anzahlZeichen = (int)sliderlänge.Value;
            pwArt.buchstaben = (PWGenerator.PasswortManager.BuchstabenArt)int.Parse((string)(((ComboBoxItem)(cbBuchstaben.SelectedItem)).Tag));
            pwArt.sonderzeichen = tbSonder.Text;
            pwArt.zahlen = switchPW.IsOn;
            pwAnzahl = (int)slideranzahl.Value;
        }
    }
}
