using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using PWGenerator;
using Windows.ApplicationModel.Email;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace Passwortgenerator
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Grid[] arrGrid;
        private PasswortManager pwm = new PasswortManager();
        public MainPage()
        {
            InitializeComponent();
            NeuePasswörter(); 
        }

        private int anzahlPWs
        {
            get
            {
                //Anzahl der neuen PWs ausgeben
                return Einstellungen.pwAnzahl;
            }

            set
            {
                //... setzen
                Einstellungen.pwAnzahl = value;
            }
        }

        public PasswortManager.PasswortArt pwArt
        {
            get
            {
                //Aktuelle PW-Art ausgeben
                return Einstellungen.pwArt;
            }

            set
            {
                //... setzen
                Einstellungen.pwArt = value;
            }
        }

        private TextBox textboxLesen(int index)
        {
            //Gibt die TB der Zeile aus
            return (TextBox)arrGrid[index].Children[0];
        }

        private Button buttonLesen(int index)
        {
            //gibt den Button der Zeile aus
            return (Button)arrGrid[index].Children[1];
        }

        private async void NeuePasswörter()
        {
            ring.IsActive = true;
            StackAufbau();
            //Diese Methode erstellt neue Passwörter und zeigt diese an.
            for (int i = 0; i < anzahlPWs; i++)
            {
                //50 ms warten, dann Textboxen schreiben (Varianz bei PWs)
                await System.Threading.Tasks.Task.Delay(50);
                textboxLesen(i).Text = pwm.PasswortGenerieren(pwArt);
            }
            ring.IsActive = false;
        }

        private void StackAufbau()
        {
            //Griddarray bestimmen
            arrGrid = new Grid[anzahlPWs];
            //Stack leeren
            stackInhalt.Children.Clear();
            //Für jedes Grid im Array ein neues erstellen...
            for (int i = 0; i < anzahlPWs; i++)
                arrGrid[i] = GridErstellen(i);
            //und dem Stack hinzufügen
            foreach (Grid g in arrGrid)
                stackInhalt.Children.Add(g);
        }

        private Grid GridErstellen(int nummmer)
        {
            //Grid zum Ausgeben wird erstellt, Tag als Wiedererkennung
            Grid endGrid = new Grid() { Tag = nummmer };
            //Drei Spalten für Layout
            endGrid.ColumnDefinitions.Add(new ColumnDefinition());
            endGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
            endGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(15) });

            //Wenn erstes Element, nur Abstand nach unten, sonst o. & u.
            if (nummmer == 0) endGrid.Margin = new Thickness(0, 0, 0, 5);
            else endGrid.Margin = new Thickness(0, 0, 0, 5);

            //Passwortausgabefeld
            endGrid.Children.Add(new TextBox() { IsReadOnly = true });

            //Button für Optionen
            Button btn = new Button() { Content = "..." };
            Grid.SetColumn(btn, 1);

            //Neues Menuflyout für den Button
            MenuFlyout mf = new MenuFlyout() { Placement = FlyoutPlacementMode.Left };
            MenuFlyoutItem mfAuswählen = new MenuFlyoutItem() { Text = "Auswählen", Tag = nummmer };
            mfAuswählen.Tapped += MfAuswählen_Tapped;
            MenuFlyoutItem mfKopieren = new MenuFlyoutItem() { Text = "Kopieren", Tag = nummmer };
            mfKopieren.Tapped += MfKopieren_Tapped;
            MenuFlyoutItem mfSenden = new MenuFlyoutItem() { Text = "Senden", Tag = nummmer };
            mfSenden.Tapped += MfSenden_Tapped;
            mf.Items.Add(mfAuswählen);
            mf.Items.Add(mfKopieren);
            mf.Items.Add(mfSenden);

            btn.Flyout = mf;

            //Elemente dem Grid hinzufügen, ausgeben
            endGrid.Children.Add(btn);
            return endGrid;
        }

        private async void MfSenden_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuFlyoutItem nSender = (MenuFlyoutItem)sender;

            EmailMessage message = new EmailMessage();
            message.Body = "Mein soeben erstelltes Passwort lautet:\n" + textboxLesen((int)nSender.Tag).Text;
            message.Subject = "Mein Passwort";
            await EmailManager.ShowComposeNewEmailAsync(message);
        }

        private void MfKopieren_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuFlyoutItem nSender = (MenuFlyoutItem)sender;
            Windows.ApplicationModel.DataTransfer.DataPackage pack = new Windows.ApplicationModel.DataTransfer.DataPackage();
            pack.SetText(textboxLesen((int)(nSender.Tag)).Text);
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(pack);

            foreach (Grid g in arrGrid)
                ((TextBox)(g.Children[0])).Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            textboxLesen((int)nSender.Tag).Background = new SolidColorBrush(Windows.UI.Colors.GreenYellow);
        }

        private void MfAuswählen_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuFlyoutItem nSender = (MenuFlyoutItem)sender;
            textboxLesen((int)(nSender.Tag)).Focus(FocusState.Pointer);
            textboxLesen((int)(nSender.Tag)).SelectAll();
        }

        private void appbbSettings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Einstellungen));
        }

        private void appbbReload_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NeuePasswörter();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://rubeen.de"));
        }
    }
}
