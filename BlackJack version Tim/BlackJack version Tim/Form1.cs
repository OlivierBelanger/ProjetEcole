using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack_version_Tim
{
    public partial class Form1 : Form
    {
        const int NBCARTES = 52;
        const int HANDMAX = 11;
        #region DefaultDeck
        string[] DefaultDeck = new string[NBCARTES] {"as_trefle", "_2_trefle",
            "_3_trefle", "_4_trefle", "_5_trefle", "_6_trefle", "_7_trefle",
            "_8_trefle", "_9_trefle", "_10_trefle", "valet_trefle","reine_trefle", "roi_trefle",
            "as_pique", "_2_pique", "_3_pique","_4_pique", "_5_pique", "_6_pique", "_7_pique",
            "_8_pique", "_9_pique", "_10_pique","valet_pique", "reine_pique", "roi_pique",
            "as_coeur","_2_coeur","_3_coeur","_4_coeur","_5_coeur","_6_coeur","_7_coeur",
            "_8_coeur","_9_coeur","_10_coeur","valet_coeur","reine_coeur","roi_coeur",
            "as_carreau","_2_carreau","_3_carreau","_4_carreau","_5_carreau","_6_carreau","_7_carreau",
            "_8_carreau","_9_carreau","_10_carreau","valet_carreau","reine_carreau","roi_carreau"};
        #endregion
        int[] ValueCard = new int[NBCARTES / 4] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

        string[] Deck = new string[NBCARTES];

        string[] HandPlayerOne = new string[HANDMAX];
        string[] HandPlayerTwo = new string[HANDMAX];

        Random rnd = new Random();

        int NbTour { get; set; }
        int NbCartesRestantes { get; set; }



        public Form1()
        {
            // grd gdgrdg dr
            NbCartesRestantes = NBCARTES;
            NbTour = 0;
            Deck = DefaultDeck;
            InitializeComponent();
            Shuffle();
            do
            {
                PickCard(NbTour, HandPlayerOne);
                PickCard(NbTour, HandPlayerTwo);
                NbTour++;
            } while (CalculateScore(HandPlayerOne) < 21 && CalculateScore(HandPlayerTwo) < 21);

        }
        /// <summary>
        /// Mélange le paquet de carte
        /// </summary>
        void Shuffle()
        {
            string[] Temp = new string[NBCARTES];
            int position;

            for (int i = 0; i < 52; i++)
            {
                position = rnd.Next(0, NBCARTES);
                if (Temp[position] == null)
                {
                    Temp[position] = Deck[i];
                }
                else
                {
                    i--;
                }
            }

            Deck = Temp;
        }

        /// <summary>
        /// Fais piger une carte aux 2 joueurs 
        /// </summary>
        /// <param name="nbTour">Le tour courant (0 étant le premier)</param>
        void PickCard(int nbTour, string[] playerHand)
        {
            if (AIchoice(nbTour, playerHand))
            {
                if (nbTour < HANDMAX)
                {
                    HandPlayerOne[nbTour] = Deck[NbCartesRestantes - 1];
                    NbCartesRestantes--;
              //      HandPlayerTwo[nbTour] = Deck[(NBCARTES - nbTour * 2) - 2];
             //       NbCartesRestantes--;
                }
            }
        }

        int CalculateScore(string[] playerHand)
        {
            string temp;
            int position;
            int somme = 0;
            for (int i = 0; i <= NbTour; i++)
            {
                if (playerHand[i] != null)
                {
                    temp = playerHand[i];
                    position = Array.FindIndex(DefaultDeck, t => t == temp);
                    position = position % 13;
                    somme += ValueCard[position++];
                }
            }
            return somme;
        }

        int CalculateScore(string[] playerHand, string Card)
        {
            string temp;
            int position;
            int somme = 0;
            for (int i = 0; i <= NbTour; i++)
            {
                if (playerHand[i] != null)
                {
                    temp = playerHand[i];
                    position = Array.FindIndex(DefaultDeck, t => t == temp);
                    position = position % 13;
                    somme += ValueCard[position++];
                }
            }
            position = Array.FindIndex(DefaultDeck, t => t == Card);
            position = position % 13;
            somme += ValueCard[position++];

            return somme;
        }

        bool AIchoice(int nbTour, string[] playerHand)
        {
            // 30 60 85

            int Score = CalculateScore(playerHand);
            int NbAs = 0;
            int NbCartesValides = 0; // Nombre de cartes qui lui font pas dépasser 21

            // Yo t'as tu des as ?
            for (int i = 0; i < nbTour; i++)
            {
                if (playerHand[i].Substring(0, 2) == "as")
                {
                    NbAs++;
                }
            }

            // Essaye toutes les cartes restantes du packet une par une pi check si tu dépasses 21 ou pas
            for (int i = 0; i < Deck.Length; i++)
            {
                if (CalculateScore(playerHand, Deck[i]) < 21) // c'est chill tu dépasses pas 21
                {
                    NbCartesValides++;
                }
            }



            float probabilite = (NbCartesValides / Deck.Length) * 100;



            return probabilite > difficulty;
        }
    }
}
