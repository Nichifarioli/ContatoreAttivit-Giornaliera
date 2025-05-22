using System.Diagnostics;
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
        private int attivitaTotali = 0; //cintatore per tenere traccia delle attività aggiunte
        

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
                }
                else
                {
                    MessageBox.Show("Questa attività è già presente nella lista.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                //mostro messaggio di errore 
                MessageBox.Show("Inserisci un nome valido per l'attività.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                return;
            }

            //verifio sia stata selezionata un'attività nella listbox 
            if (lstAttivita.SelectedItem != null)
            //imposto il tempo iniziale 
            {
                secondiRimanenti = 60; // per esempio 1 minuto
                lblTimer.Content = "01:00";
                timer.Start();//avvio il timer 

                // Disattiva i pulsanti
                btnAggiungiAttività.IsEnabled = false;
                btnSegnaCompletata.IsEnabled = false;
            }
            else //mostro il tempo rimanente nella label
            {
                // Mostra il tempo rimanente nella finestra (creo una Label per questo)
                TimeSpan time = TimeSpan.FromSeconds(secondiRimanenti);
                lblTimer.Content = time.ToString(@"mm\:ss");
                this.Title = $"Tempo rimanente: {time.ToString(@"mm\:ss")}";
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
            secondiRimanenti--;

            int minuti = secondiRimanenti / 60;
            int secondi = secondiRimanenti % 60;
            lblTimer.Content = $"{minuti:D2}:{secondi:D2}";

            if (secondiRimanenti <= 0)
            {
                timer.Stop();
                MessageBox.Show("Tempo scaduto per l'attività selezionata!");

                // Riattiva i pulsanti
                btnAggiungiAttività.IsEnabled = true;
                btnSegnaCompletata.IsEnabled = true;
            }
        }

        private void btnRiepilogo_Click(object sender, RoutedEventArgs e)
        {
            //conto attività completate 
            // Conta quante attività sono state completate
            int totaleCompletate = completate.Count;

            // Se nessuna attività è stata completata, mostra un messaggio diverso
            if (totaleCompletate == 0)
            {
                MessageBox.Show("Non hai completato ancora nessuna attività oggi!", "Riepilogo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Crea il messaggio di riepilogo
                string riepilogo = $"Hai completato {totaleCompletate} attività:\n";

                // Aggiungi l’elenco delle attività completate
                foreach (string attivita in completate)
                {
                    riepilogo += $"- {attivita}\n";
                }

                //mostro messaggio di riepilogo
                MessageBox.Show(riepilogo, "Riepilogo attività completate", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}