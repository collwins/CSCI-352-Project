using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main_Menu
{
    /// <summary>
    /// Interaction logic for Blackjack_Page.xaml
    /// </summary>

   
    public partial class Blackjack_Page : Page
    {
        Frame parentFrame;
        int uid;
        int balance;
        int bet;
        List<card> playerHand;
        List<card> dealerHand;
        List<card> splitHand;
        int playerPoints;
        int dealerPoints;
        int splitPoints;
        bool betDoubled; //may remove later
        bool split;
        bool splitBlackjack;
        bool splitBust;
   
        public Blackjack_Page(Frame parentFrame, int uid, int balance, int bet)
        {
            this.parentFrame = parentFrame;
            this.uid = uid;
            this.balance = balance;
            this.bet = bet;
            playerHand = new List<card>(2);
            dealerHand = new List<card>(2);
            splitHand = new List<card>(2);
            playerPoints = 0;
            dealerPoints = 0;
            splitPoints = 0;
            betDoubled = false;
            split = false;
            splitBust = false;
            splitBlackjack = false;
            InitializeComponent();
            balance_label.Content = $"BALANCE: ${this.balance}";
            bet_label.Content = $"BET: ${this.bet}";
            split_btn.IsEnabled = false;
            if (this.balance < this.bet) double_btn.IsEnabled = false;
        }

        public struct card
        {
            public card(int value, string face, string suit)
            {
                Value = value;
                Face = face;
                Suit = suit;
            }
            public int Value;
            public string Face;
            public string Suit;

        }

        public enum CardType
        {
            TWOSPADE,
            THREESPADE,
            FOURSPADE,
            FIVESPADE,
            SIXSPADE,
            SEVENSPADE,
            EIGHTSPADE,
            NINESPADE,
            TENSPADE,
            JACKSPADE,
            QUEENSPADE,
            KINGSPADE,
            ACESPADE,

            TWOCLUB,
            THREECLUB,
            FOURCLUB,
            FIVECLUB,
            SIXCLUB,
            SEVENCLUB,
            EIGHTCLUB,
            NINECLUB,
            TENCLUB,
            JACKCLUB,
            QUEENCLUB,
            KINGCLUB,
            ACECLUB,

            TWOHEART,
            THREEHEART,
            FOURHEART,
            FIVEHEART,
            SIXHEART,
            SEVENHEART,
            EIGHTHEART,
            NINEHEART,
            TENHEART,
            JACKHEART,
            QUEENHEART,
            KINGHEART,
            ACEHEART,

            TWODIAMOND,
            THREEDIAMOND,
            FOURDIAMOND,
            FIVEDIAMOND,
            SIXDIAMOND,
            SEVENDIAMOND,
            EIGHTDIAMOND,
            NINEDIAMOND,
            TENDIAMOND,
            JACKDIAMOND,
            QUEENDIAMOND,
            KINGDIAMOND,
            ACEDIAMOND,
        }


        //checks the value of the given hand
        //if the hand is a blackjack, returns "blackjack"
        //if the hand is two cards of the same face, returns "equal"
        //if the hand's total value is > 21, returns "bust"
        //otherwise returns total value of the hand.
        private string checkHand(List<card> hand)
        {
            int sum = 0;
            if (hand.Count == 2 && hand[0].Face == hand[1].Face)
            {
                foreach (card c in hand)
                {
                    if (c.Face == "A" && sum > 10) sum += 1;
                    else sum += c.Value;
                }

                if (hand == playerHand) playerPoints = sum;
                else if (hand == dealerHand) dealerPoints = sum;
                else splitPoints = sum;
                return "equal";
            }
            else if (hand.Count == 2 && hand[0].Value + hand[1].Value == 21) {
                if (hand == playerHand) playerPoints = 21;
                else if (hand == dealerHand) dealerPoints = 21;
                else splitPoints = 21;
                return "blackjack";
            }
            else
            {

                foreach (card c in hand)
                {
                    if (c.Face == "A" && sum > 10) sum += 1;
                    else sum += c.Value;
                }

                if (sum > 21)
                {
                    if (hand == playerHand) playerPoints = sum;
                    else if (hand == dealerHand) dealerPoints = sum;
                    else splitPoints = sum;
                    return "bust";
                }

                if (hand == playerHand) playerPoints = sum;
                else if (hand == dealerHand) dealerPoints = sum;
                else splitPoints = sum;
                return $"{sum}";
            }
        }

        //deals two cards to the player and dealer
        private void Deal()
        {
            //generate player's starting hand
            CardFactory factory = new CardFactory();
            Random rand = new Random();
            int card1 = rand.Next(0, 51);
            int card2 = rand.Next(0, 51);
            while(card1 == card2) //ensures both cards are not the same
            {
                card2 = rand.Next(0, 51);
            }
            CardType type = (CardType)card1;
            playerHand.Insert(0, factory.getCard(type));
            type = (CardType)card2;
            playerHand.Insert(1, factory.getCard(type));

            //generate the dealer's starting hand
            card1 = rand.Next(0, 51);
            card2 = rand.Next(0, 51);
            while (card1 == card2) //ensures both cards are not the same
            {
                card2 = rand.Next(0, 51);
            }
            type = (CardType)card1;
            dealerHand.Insert(0, factory.getCard(type));
            type = (CardType)card2;
            dealerHand.Insert(1, factory.getCard(type));

            string dealerCheck = checkHand(dealerHand);
            string playerCheck = checkHand(playerHand);

            player_points_display.Content = playerPoints.ToString();
            dealer_points_display.Content = dealerPoints.ToString();

            if (dealerCheck == "blackjack" && playerCheck == "blackjack")
            {
                //reveal dealer's other card
                //wait a second
                //add bet to player's balance
                MessageBox.Show("Push! Your bet has been returned.");
                parentFrame.Content = new Bet_Page(parentFrame, uid, balance + bet);
            }
            else if (dealerCheck == "blackjack" && playerCheck != "blackjack")
            {
                //reveal dealer's other card
                //Nevada style: face down when not revealed, face up when revealed
                //London style: card is not there until reveal
                //update player's balance in db
                MessageBox.Show("Dealer has blackjack! You lose!");
                parentFrame.Content = new Bet_Page(parentFrame, uid, balance);
            }
            else if (dealerCheck != "blackjack" && playerCheck == "blackjack")
            {
                //wait a second
                //update player's balance in db
                MessageBox.Show("Blackjack! You win!");
                parentFrame.Content = new Bet_Page(parentFrame, uid, balance + (bet*2) + (int)Math.Floor(bet * 1.5));
            }
            else if (playerCheck == "equal" && balance >= bet) split_btn.IsEnabled = true;
        }

        //adds a card to the given hand
        private void insertCard(List<card> hand)
        {
            CardFactory factory = new CardFactory();
            Random rand = new Random();
            int card = rand.Next(0, 51);
            CardType type = (CardType)card;
            hand.Add(factory.getCard(type));
        }

        //Factory method for creating card objects.
        class CardFactory
        {
            public card getCard(CardType type)
            {
                switch (type)
                {
                    case CardType.TWOSPADE:
                        {
                            return new card(2, "2", "spade");
                        }
                    case CardType.THREESPADE:
                        {
                            return new card(3, "3", "spade");
                        }
                    case CardType.FOURSPADE:
                        {
                            return new card(4, "4", "spade");
                        }
                    case CardType.FIVESPADE:
                        {
                            return new card(5, "5", "spade");
                        }
                    case CardType.SIXSPADE:
                        {
                            return new card(6, "6", "spade");
                        }
                    case CardType.SEVENSPADE:
                        {
                            return new card(7, "7", "spade");
                        }
                    case CardType.EIGHTSPADE:
                        {
                            return new card(8, "8", "spade");
                        }
                    case CardType.NINESPADE:
                        {
                            return new card(2, "2", "spade");
                        }
                    case CardType.TENSPADE:
                        {
                            return new card(10, "10", "spade");
                        }
                    case CardType.JACKSPADE:
                        {
                            return new card(10, "J", "spade");
                        }
                    case CardType.QUEENSPADE:
                        {
                            return new card(10, "Q", "spade");
                        }
                    case CardType.KINGSPADE:
                        {
                            return new card(10, "K", "spade");
                        }
                    case CardType.ACESPADE:
                        {
                            return new card(11, "A", "spade");
                        }
                    case CardType.TWOCLUB:
                        {
                            return new card(2, "2", "club");
                        }
                    case CardType.THREECLUB:
                        {
                            return new card(3, "3", "club");
                        }
                    case CardType.FOURCLUB:
                        {
                            return new card(4, "4", "club");
                        }
                    case CardType.FIVECLUB:
                        {
                            return new card(5, "5", "club");
                        }
                    case CardType.SIXCLUB:
                        {
                            return new card(6, "6", "club");
                        }
                    case CardType.SEVENCLUB:
                        {
                            return new card(7, "7", "club");
                        }
                    case CardType.EIGHTCLUB:
                        {
                            return new card(8, "8", "club");
                        }
                    case CardType.NINECLUB:
                        {
                            return new card(9, "9", "club");
                        }
                    case CardType.TENCLUB:
                        {
                            return new card(10, "10", "club");
                        }
                    case CardType.JACKCLUB:
                        {
                            return new card(10, "J", "club");
                        }
                    case CardType.QUEENCLUB:
                        {
                            return new card(10, "Q", "club");
                        }
                    case CardType.KINGCLUB:
                        {
                            return new card(10, "K", "club");
                        }
                    case CardType.ACECLUB:
                        {
                            return new card(11, "A", "club");
                        }
                    case CardType.TWOHEART:
                        {
                            return new card(2, "2", "heart");
                        }
                    case CardType.THREEHEART:
                        {
                            return new card(3, "3", "heart");
                        }
                    case CardType.FOURHEART:
                        {
                            return new card(4, "4", "heart");
                        }
                    case CardType.FIVEHEART:
                        {
                            return new card(5, "5", "heart");
                        }
                    case CardType.SIXHEART:
                        {
                            return new card(6, "6", "heart");
                        }
                    case CardType.SEVENHEART:
                        {
                            return new card(7, "7", "heart");
                        }
                    case CardType.EIGHTHEART:
                        {
                            return new card(8, "8", "heart");
                        }
                    case CardType.NINEHEART:
                        {
                            return new card(9, "9", "heart");
                        }
                    case CardType.TENHEART:
                        {
                            return new card(10, "10", "heart");
                        }
                    case CardType.JACKHEART:
                        {
                            return new card(10, "J", "heart");
                        }
                    case CardType.QUEENHEART:
                        {
                            return new card(10, "Q", "heart");
                        }
                    case CardType.KINGHEART:
                        {
                            return new card(10, "K", "heart");
                        }
                    case CardType.ACEHEART:
                        {
                            return new card(11, "A", "heart");
                        }
                    case CardType.TWODIAMOND:
                        {
                            return new card(2, "2", "diamond");
                        }
                    case CardType.THREEDIAMOND:
                        {
                            return new card(3, "3", "diamond");
                        }
                    case CardType.FOURDIAMOND:
                        {
                            return new card(4, "4", "diamond");
                        }
                    case CardType.FIVEDIAMOND:
                        {
                            return new card(5, "5", "diamond");
                        }
                    case CardType.SIXDIAMOND:
                        {
                            return new card(6, "6", "diamond");
                        }
                    case CardType.SEVENDIAMOND:
                        {
                            return new card(7, "7", "diamond");
                        }
                    case CardType.EIGHTDIAMOND:
                        {
                            return new card(8, "8", "diamond");
                        }
                    case CardType.NINEDIAMOND:
                        {
                            return new card(9, "9", "diamond");
                        }
                    case CardType.TENDIAMOND:
                        {
                            return new card(10, "10", "diamond");
                        }
                    case CardType.JACKDIAMOND:
                        {
                            return new card(10, "J", "diamond");
                        }
                    case CardType.QUEENDIAMOND:
                        {
                            return new card(10, "Q", "diamond");
                        }
                    case CardType.KINGDIAMOND:
                        {
                            return new card(10, "K", "diamond");
                        }
                    case CardType.ACEDIAMOND:
                        {
                            return new card(11, "A", "diamond");
                        }
                    default:
                        {
                            return new card(-1, "null", "null");
                        }
                }
            }
            
        }

        //handles the dealer logic
        //dealer stands when its points are >= 17
        //otherwise, it keeps hitting
        //no splits or double downs allowed
        private void dealerPlay()
        {
            hit_btn.IsEnabled = false;
            stand_btn.IsEnabled = false;
            if (double_btn.IsEnabled) double_btn.IsEnabled = false;
            if (split_btn.IsEnabled) split_btn.IsEnabled = false;
            while (dealerPoints < 17)
            {
                insertCard(dealerHand);
                checkHand(dealerHand);
                //display card being added to dealer's hand
            }
            dealer_points_display.Content = dealerPoints.ToString();

            if (dealerPoints > 21)
            {
                if (player_points_display.Content.ToString().Contains("/") && splitBust) //split hand bust and dealer bust, effectively a push
                {
                    MessageBox.Show("Dealer bust! Your main hand wins!");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance + bet);
                }
                else
                {
                    MessageBox.Show("Dealer bust! You win!");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance + (bet * 2));
                }
            }

            else if (player_points_display.Content.ToString().Contains("/")) //if a split happened
            {
                /***player loses main hand***/
                if (dealerPoints > playerPoints && dealerPoints > splitPoints) //split hand loses
                {
                    MessageBox.Show("Dealer wins!");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance - (bet / 2));
                }

                else if (dealerPoints > playerPoints && dealerPoints < splitPoints)//split hand wins
                {
                    MessageBox.Show("Your main hand loses, but your split hand wins!");
                    if (splitBlackjack) parentFrame.Content = new Bet_Page(parentFrame, uid, balance + (bet / 2) + (int)Math.Floor((bet / 2) * 1.5));
                    else parentFrame.Content = new Bet_Page(parentFrame, uid, balance + bet);
                }

                else if (dealerPoints > playerPoints && dealerPoints == splitPoints) //split hand pushes
                {
                    MessageBox.Show("Your main hand loses, but your split hand pushes!");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance);
                }
                /***player pushes main hand***/
                else if (dealerPoints == playerPoints && dealerPoints == splitPoints) //split hand pushes
                {
                    //update balance in db
                    MessageBox.Show("Push! Your bet has been returned.");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance + (bet / 2));
                }

                else if (dealerPoints == playerPoints && dealerPoints < splitPoints) //split hand wins
                {
                    MessageBox.Show("Your main hand pushes, but your split hand wins!");
                    if (splitBlackjack) parentFrame.Content = new Bet_Page(parentFrame, uid, balance + bet + (int)Math.Floor((bet / 2) * 1.5));
                    else parentFrame.Content = new Bet_Page(parentFrame, uid, balance + bet);
                }

                else if (dealerPoints == playerPoints && dealerPoints > splitPoints) //split hand loses
                {
                    MessageBox.Show("Your main hand pushes, and your split hand loses!");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance - (bet / 2));
                }
                /***player wins main hand***/
                else if (dealerPoints < playerPoints && dealerPoints == splitPoints) //split hand pushes
                {
                    MessageBox.Show("Your main hand wins, and your split hand pushes!");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance + bet);
                }
                else if (dealerPoints < playerPoints && dealerPoints > splitPoints) //split hand loses
                {
                    MessageBox.Show("Your main hand wins, but your split hand loses!");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance + (bet / 2));
                }
                else //split hand wins
                {
                    //update balance in db
                    MessageBox.Show("You win!");
                    if (splitBlackjack) parentFrame.Content = new Bet_Page(parentFrame, uid, balance + (bet * 2) + (int)Math.Floor((bet / 2) * 1.5));
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance + (bet * 2));
                }
            }

            else //no split, dealer does not bust
            {
                if (dealerPoints > playerPoints)
                {
                    MessageBox.Show("Dealer wins!");
                    if (betDoubled) parentFrame.Content = new Bet_Page(parentFrame, uid, balance - (bet / 2));
                    else parentFrame.Content = new Bet_Page(parentFrame, uid, balance);
                }

                else if (dealerPoints == playerPoints)
                {
                    //update balance in db
                    MessageBox.Show("Push! Your bet has been returned.");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance + bet);
                }
                else
                {
                    //update balance in db
                    MessageBox.Show("You win!");
                    parentFrame.Content = new Bet_Page(parentFrame, uid, balance + (bet * 2));
                }
            }

        }

        //displays a card to the screen
        private void displayCard(card c)
        {
            Rectangle cardBase = new Rectangle();
            cardBase.Height = 117;
            cardBase.Width = 100;
            cardBase.Margin = new Thickness(125, 316, 0, 0);
            cardBase.HorizontalAlignment = HorizontalAlignment.Left;
            cardBase.VerticalAlignment = VerticalAlignment.Top;
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            BitmapImage image = new BitmapImage(new Uri($"{projectDirectory}/Pages/Images/Back_Cover.png", UriKind.Relative));
            cardBase.Fill = new ImageBrush(image);
            grid.Children.Add(cardBase);
        }

        private void hit_btn_Click(object sender, RoutedEventArgs e)
        {
            if (double_btn.IsEnabled) double_btn.IsEnabled = false;
            if (split_btn.IsEnabled) split_btn.IsEnabled = false;
            if (split)
            {
                insertCard(splitHand);
                //display new card in split hand
                string check = checkHand(splitHand);
                if (check == "21")
                {
                    player_points_display.Content = $"{playerPoints}/21";
                    split = false;
                }
                else if (check == "bust")
                {
                    //update player's balance in db
                    player_points_display.Content = $"{playerPoints}/{splitPoints}";
                    MessageBox.Show("Bust! Your split hand lost!");
                    split = false;
                    splitBust = true;
                    split_hand_arrow.Visibility = Visibility.Hidden;
                    main_hand_arrow.Visibility = 0;
                }
                else player_points_display.Content = $"{playerPoints}/{splitPoints}";
            }
            else
            {
                insertCard(playerHand);
                displayCard(playerHand[0]);
                //display new card in player's hand
                string check = checkHand(playerHand);
                if (check == "21")
                {
                    if (player_points_display.Content.ToString().Contains("/"))
                    {
                        player_points_display.Content = $"21/{splitPoints}";
                        split_hand_arrow.Visibility = Visibility.Hidden;
                        main_hand_arrow.Visibility = 0;
                        dealerPlay();
                    }
                    else
                    {
                        player_points_display.Content = "21";
                        dealerPlay();
                    }
                }
                else if (check == "bust")
                {
                    //update player's balance in db
                    if (player_points_display.Content.ToString().Contains("/"))
                    {
                        if (splitBust)
                        {
                            player_points_display.Content = $"{playerPoints}/{splitPoints}";
                            MessageBox.Show("Bust! You lose!");
                            parentFrame.Content = new Bet_Page(parentFrame, uid, balance);
                        }
                        else
                        {
                            player_points_display.Content = $"{playerPoints}/{splitPoints}";
                            dealerPlay();
                        }
                    }
                    else
                    {
                        player_points_display.Content = $"{playerPoints}";
                        MessageBox.Show("Bust! You lose!");
                        parentFrame.Content = new Bet_Page(parentFrame, uid, balance);
                    }

                }
                else
                {
                    if (player_points_display.Content.ToString().Contains("/"))
                    {
                        player_points_display.Content = $"{playerPoints}/{splitPoints}";
                    }
                    else player_points_display.Content = playerPoints.ToString();
                }
            }
            
        }


        private void double_btn_Click(object sender, RoutedEventArgs e)
        {
            insertCard(playerHand);
            //display new card in player's hand
            double_btn.IsEnabled = false;
            bet *= 2;
            betDoubled = true;
            bet_label.Content = $"BET: ${bet}";
            string check = checkHand(playerHand);
            if (check == "bust")
            {
                //update player's balance in db
                player_points_display.Content = playerPoints.ToString();
                MessageBox.Show("Bust! You lose!");
                parentFrame.Content = new Bet_Page(parentFrame, uid, balance - bet);
            }
            else
            {
                player_points_display.Content = playerPoints.ToString();
                dealerPlay();
            }
        }

        private void stand_btn_Click(object sender, RoutedEventArgs e)
        {
            if (split)
            {
                split = false;
                split_hand_arrow.Visibility = Visibility.Hidden;
                main_hand_arrow.Visibility = 0;
            }
            else dealerPlay();
        }

        private void split_btn_Click(object sender, RoutedEventArgs e)
        {
            //split the player's hand into two distinct hands in display
            //in other words, display splitHand and playerHand separately.
            split_btn.IsEnabled = false;
            double_btn.IsEnabled = false;
            bet *= 2;
            betDoubled = true;
            bet_label.Content = $"BET: ${bet}";
            splitHand.Insert(0, playerHand[1]);
            playerHand.RemoveAt(1);
            insertCard(splitHand);
            insertCard(playerHand);
            split = true;
            split_hand_arrow.Visibility = 0;

            string check = checkHand(playerHand);
            string splitCheck = checkHand(splitHand);
            if (splitCheck == "blackjack" && check == "blackjack")
            {
                player_points_display.Content = $"21/21";
                MessageBox.Show("You got blackjack on both hands!");
                parentFrame.Content = new Bet_Page(parentFrame, uid, balance + ((bet * 2) + (int)Math.Floor(bet * 1.5)) * 2);
            }
            else if (splitCheck == "blackjack" && check != "blackjack")
            {
                player_points_display.Content = $"{playerPoints}/21";
                split = false;
                MessageBox.Show("You got blackjack on your split hand!");
                splitBlackjack = true;
                split_hand_arrow.Visibility = Visibility.Hidden;
                main_hand_arrow.Visibility = 0;
            }
            else
            {
                player_points_display.Content = $"{playerPoints}/{splitPoints}";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        { 
            hit_btn.IsEnabled = false;
            hit_btn.Visibility = Visibility.Hidden;
            stand_btn.IsEnabled = false;
            stand_btn.Visibility = Visibility.Hidden;
            double_btn.IsEnabled = false;
            double_btn.Visibility = Visibility.Hidden;
            split_btn.IsEnabled = false;
            split_btn.Visibility = Visibility.Hidden;
            player_points_display.Visibility = Visibility.Hidden;
            player_point_counter_label.Visibility = Visibility.Hidden;
            dealer_points_display.Visibility = Visibility.Hidden;
            dealer_point_counter_label.Visibility = Visibility.Hidden;
            balance_label.Visibility = Visibility.Hidden;
            player_rectangle.Visibility = Visibility.Hidden;
            dealer_rectangle.Visibility = Visibility.Hidden;
            bet_label.Visibility = Visibility.Hidden;
            split_hand_arrow.Visibility = Visibility.Hidden;
            main_hand_arrow.Visibility = Visibility.Hidden;
        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void play_btn_Click(object sender, RoutedEventArgs e)
        {
            hit_btn.IsEnabled = true;
            hit_btn.Visibility = 0;
            stand_btn.IsEnabled = true;
            stand_btn.Visibility = 0;
            double_btn.IsEnabled = true;
            double_btn.Visibility = 0;
            split_btn.Visibility = 0;
            player_points_display.Visibility = 0;
            player_point_counter_label.Visibility = 0;
            dealer_points_display.Visibility = 0;
            dealer_point_counter_label.Visibility = 0;
            balance_label.Visibility = 0;
            player_rectangle.Visibility = 0;
            dealer_rectangle.Visibility = 0;
            bet_label.Visibility = 0;
            grid.Children.Remove(play_btn);
            Deal();
        }
    }
}
