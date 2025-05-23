﻿using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Media; //Serve per i suoni windows

namespace ContatoreAttivitàGiornaliera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //per avere un timer devo crearmi delle variabili
        private int secondiRimanenti; //contatore dei secondi rimanenti
        private DispatcherTimer timer; //timer

        
        //variabile booleana per controllare attività completate
        private List<string> completate = new List<string>();
        private int attivitaCompletate = 0; //variabile contatore per tenere traccia del numero di attività completate
        private int attivitaTotali = 0; //contatore per tenere traccia delle attività aggiunte
        

        public MainWindow()
        {
            InitializeComponent(); //inizializzo i componenti

            //imposto le proprietà del DispatcherTimer
            timer = new DispatcherTimer();
            //collego evento Tick del timer
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void btnAggiungiAttività_Click(object sender, RoutedEventArgs e)
        {
            //controllo che il textbox (txtNome) non sia vuoto 
            if (!string.IsNullOrWhiteSpace(txtNome.Text))
            {
                //controllo se attività esiste già nella lista 
                if (!lstAttivita.Items.Contains(txtNome.Text))
                {
                    //aggiungo attività alla listbox (lstAttivita)
                    lstAttivita.Items.Add(txtNome.Text);
                    //incremento contatore attività inserite
                    attivitaTotali++;

                    //Suono informativo: attività aggiunta con successo
                    SystemSounds.Asterisk.Play();
                }
                else
                {
                    MessageBox.Show("Questa attività è già presente nella lista.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //Suono di avviso
                    SystemSounds.Exclamation.Play();
                }
            }
            else
            {
                //mostro messaggio di errore 
                MessageBox.Show("Inserisci un nome valido per l'attività.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);

                //Suono di errore
                SystemSounds.Hand.Play();
            }
            //svuoto textbox 
            txtNome.Clear();
        }

        private void btnAvvioTimer_Click(object sender, RoutedEventArgs e)
        {

            //aggiungo controllo per evitare di avviare il timer quando è già in esecuzione
            if (timer.IsEnabled)
            {
                MessageBox.Show("Il timer è già in esecuzione.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Information);
                // Suono di avviso
                SystemSounds.Exclamation.Play();
                return;
            }


            //avendo aggiunto la textbox "Durata(min), leggo il valore inserito 
            if(!int.TryParse(txtDurata.Text, out int minutes) || minutes <= 0)
            {
                //messaggio di errore e suono 
                MessageBox.Show("La durata (min) non è valida!", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                //Suono di errore
                SystemSounds.Hand.Play();
                return;

            }

            //verifico sia stata selezionata un'attività nella listbox 
            if (lstAttivita.SelectedItem != null)
            //imposto il tempo iniziale 
            {
                secondiRimanenti = 60*minutes; // per esempio 1 minuto
                TimeSpan time = TimeSpan.FromSeconds(secondiRimanenti);
                lblTimer.Content = time.ToString(@"mm\:ss");
                this.Title = $"Tempo rimanente: {time.ToString(@"mm\:ss")}";
                timer.Start();//avvio il timer 

                // Disattiva i pulsanti
                btnAggiungiAttività.IsEnabled = false;
                btnSegnaCompletata.IsEnabled = false;
                btnAvvioTimer.IsEnabled = false; // disabilito anche il pulsante avvio timer

                //  Suono di conferma/inizio
                SystemSounds.Beep.Play();
            }
            else //mostro il tempo rimanente nella label
            {
                // Mostra il tempo rimanente nella finestra (creo una Label per questo)
                TimeSpan time = TimeSpan.FromSeconds(secondiRimanenti);
                lblTimer.Content = time.ToString(@"mm\:ss");
                this.Title = $"Tempo rimanente: {time.ToString(@"mm\:ss")}";

                MessageBox.Show("Seleziona un'attività prima di avviare il timer.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);

                //Suono di errore
                SystemSounds.Hand.Play();
            }
        }

        private void btnStopTimer_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop(); // Ferma il timer

                //Resetta label timer
                lblTimer.Content = "00:00";
                this.Title = "Contatore Attività Giornaliere";

                //Riattiva i pulsanti
                btnAggiungiAttività.IsEnabled = true;
                btnSegnaCompletata.IsEnabled = true;
                btnAvvioTimer.IsEnabled = true;

                //Suono di notifica
                System.Media.SystemSounds.Asterisk.Play();
            }
            else
            {
                MessageBox.Show("Il timer non è attivo.", "Informazione", MessageBoxButton.OK, MessageBoxImage.Information);
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void btnSegnaCompletata_Click(object sender, RoutedEventArgs e)
        {
            //Verifica che un’attività sia selezionata.
            if (lstAttivita.SelectedItem != null)
            {
                // Ottiene l’attività selezionata
                string attivita = lstAttivita.SelectedItem.ToString();
                // Aggiunge l’attività alla lista delle attività completate.
                completate.Add(attivita);
                //Rimuove l’attività dalla ListBox.
                lstAttivita.Items.Remove(lstAttivita.SelectedItem);

                //inremento il contatore di attività 
                attivitaCompletate++;
                //aggiorno la label 
                lblCompletate.Content = $"Attività completate: {attivitaCompletate}";
            }
            else
            {
                //mostro messaggio di errore 
                MessageBox.Show("Seleziona un'attività da segnare come completa.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
            


        //aggiungo metodo da eseguire ogni volta che il timer fa "Tick"
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (secondiRimanenti > 0)
            {
                secondiRimanenti--;
                int minuti = secondiRimanenti / 60;
                int secondi = secondiRimanenti % 60;
                lblTimer.Content = $"{minuti:D2}:{secondi:D2}";
                // Mostra il tempo rimanente nella finestra (puoi creare una Label per questo)
                TimeSpan time = TimeSpan.FromSeconds(secondiRimanenti);
                lblTimer.Content = time.ToString(@"mm\:ss");
                this.Title = $"Tempo rimanente: {time.ToString(@"mm\:ss")}";
            }

            else
            {
                timer.Stop();
                MessageBox.Show("Tempo scaduto per l'attività selezionata!");

                // Riattiva i pulsanti
                btnAggiungiAttività.IsEnabled = true;
                btnSegnaCompletata.IsEnabled = true;
                btnAvvioTimer.IsEnabled = true;
            }
        }

        private void btnRiepilogo_Click(object sender, RoutedEventArgs e)
        {
            // Creo una lista di tutte le attività: quelle ancora non completate + quelle completate
            List<string> tutteAttivita = new List<string>();

            // Aggiungo attività dalla ListBox attuali (non completate)
            foreach (var item in lstAttivita.Items)
            {
                tutteAttivita.Add(item.ToString());
            }

            // Aggiungo quelle completate
            tutteAttivita.AddRange(completate);

            int totaleCompletate = completate.Count;

            // Apro la finestra statistiche passando i dati
            FinestraStatistiche finestraStat = new FinestraStatistiche(tutteAttivita, totaleCompletate);
            finestraStat.ShowDialog();
        }

        private bool temaScuroAttivo = false; // stato corrente del tema
        private void btnCambiaTema_Click(object sender, RoutedEventArgs e)
        {
            string tema = temaScuroAttivo ? "LightTheme.xaml" : "DarkTheme.xaml";
            string uri = $"Themes/{tema}";

            // Pulisce i dizionari attuali
            Application.Current.Resources.MergedDictionaries.Clear();

            // Carica il nuovo tema
            var themeDict = new ResourceDictionary();
            themeDict.Source = new Uri(uri, UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(themeDict);

            // Cambia il contenuto del pulsante
            Button btn = sender as Button;
            btn.Content = temaScuroAttivo ? "☀️ Modalità Giorno" : "🌙 Modalità Notte";


            // Cambia lo stato
            temaScuroAttivo = !temaScuroAttivo;
        }
    }
}