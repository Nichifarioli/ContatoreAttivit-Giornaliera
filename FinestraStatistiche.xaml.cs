using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
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

namespace ContatoreAttivitàGiornaliera
{
    /// <summary>
    /// Logica di interazione per FinestraStatistiche.xaml
    /// </summary>
    public partial class FinestraStatistiche : Window
    {
        
        //assegno questi dati a variabili private della finestra 
        private int totaleCompletate;
        private List<string> listaAttivita;

        //devo ricevere dalla finestra principale la lista delle attività e quante sono completate
        public FinestraStatistiche(List <string> attività, int completate)
        {
            InitializeComponent();
            
            //passo la lista delle attività come parametro --> completate
            listaAttivita = attività;
            //passo il numero di attività completate --> attivitaCompletate
            totaleCompletate = completate;
            
            //popolo la ListBox con le attività 
            foreach(var att in listaAttivita)
            {
                lstAttivitaGiornaliere.Items.Add(att);
            }

            // Imposto il totale completate nel TextBox
            txtTotaleCompletate.Text = totaleCompletate.ToString();

            //calcolo la percentuale 
            double percentuale = 0;
            if(listaAttivita.Count > 0)
            {
                    percentuale = (double) totaleCompletate / listaAttivita.Count *100;
                    //mostro il totale completate e la percentuale 
                    txtPercentuale.Text = percentuale.ToString("F0") + "%";
                    // Aggiorno la progress bar
                    progressBarPercentuale.Value = percentuale;
            
            }
        }

        // Metodo evento per chiudere la finestra
        private void btnChiudi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
