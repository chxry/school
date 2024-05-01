using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

static class Program {
	static void Main() {
		Application.Run(new GameForm());
	}

	class GameForm: Form {
		static Pack pack;
		static Player[] players = new Player[3];
	
		public GameForm() {
			BackColor = SystemColors.Window;
			ClientSize = new Size(640, 720);
			Text = "blackjack";

			pack = new Pack();
			pack.Shuffle();

			for (int i = 0; i < players.Length; i ++) {
				players[i] = new Player(i, this);
				players[i].AddCard(pack.DrawCard());
			}
			players[0].StartTurn();
		}

		public class Player {
			List<CardImage> cards = new List<CardImage>();
			int pNum;
			bool stood;

			Form form;
			Label label;
			Button hit;
			Button stand;

			public Player(int n, Form f) {
				pNum = n;
				stood = false;
				form = f;

				Font big = new Font("comic sans ms", 24);
				label = new Label();
				label.Location = new Point(16, GetY());
				label.AutoSize = true;
				label.Font = big;
				form.Controls.Add(label);

				hit = new Button();
				hit.Text = "hit";
				hit.Location = new Point(16, GetY() + 48);
				hit.Size = new Size(224, 40);
				hit.Font = big;
				hit.Enabled = false;
				hit.Click += (_, _) => Hit();
				form.Controls.Add(hit);

				stand = new Button();
				stand.Text = "stand";
				stand.Location = new Point(16, GetY() + 96);
				stand.Size = new Size(224, 40);
				stand.Font = big;
				stand.Enabled = false;
				stand.Click += (_, _) => Stand();
				form.Controls.Add(stand);

				RefreshLabel();
			}

			public void AddCard(Card c) {
				CardImage p = new CardImage(c);
				p.Location = new Point(256 + cards.Count * 32, GetY());
				cards.Add(p);
				form.Controls.Add(p);
				p.BringToFront();
				RefreshLabel();
			}

			public int GetScore() {
				int score = 0;
				for (int i = 0; i < cards.Count; i++) {
					int rank = cards[i].GetCard().GetRank();
					if (rank >= 2 && rank <= 10) {
						score += rank;
					} else if (rank == 1 && score + 11 > 21) {
						score += 1;
					} else {
						score += 11;
					}
				}
				return score;
			}

			public void StartTurn() {
				if (stood) {
					EndTurn();
					return;
				}
				hit.Enabled = true;
				stand.Enabled = true;
				RefreshLabel();
			}

			public void EndTurn() {
				hit.Enabled = false;
				stand.Enabled = false;
				RefreshLabel();
				// check game state here?
				players[(pNum + 1) % players.Length].StartTurn();
			}

			void Hit() {
				AddCard(pack.DrawCard());
				EndTurn();
			}

			void Stand() {
				stood = true;
				EndTurn();
			}

			void RefreshLabel() {
				label.Text = (stood ? "X " : (hit.Enabled ? "> " : "")) + "player " + (pNum + 1) + ": " + GetScore();
			}

			int GetY() {
				return 16 + pNum * 240;
			}
		}
	}

	public class CardImage: PictureBox {
		Card card;
		
		public CardImage(Card c) {
			card = c;
			SizeMode = PictureBoxSizeMode.StretchImage;
			Size = new Size(108, 144);
			Image = Image.FromFile("cards/" + card.GetRank() + card.GetSuitName()[0] + ".png");
		}

		public Card GetCard() {
			return card;
		}
	}
	
	public class Card {
		int rank;
		int suit;

		public Card(int r, int s) {
			rank = r;
			suit = s;
		}

		public int GetRank() { return rank; }
		
		public int GetSuit() { return suit; }
		
		public int GetScore() {
			return rank * 4 + suit;
		}

		public string GetRankName() {
			return rank switch {
				1 => "ace",
				2 => "two",
				3 => "three",
				4 => "four",
				5 => "five",
				6 => "six",
				7 => "seven",
				8 => "eight",
				9 => "nine",
				10 => "ten",
				11 => "jack",
				12 => "queen",
				13 => "king",
				_ => "?"
			};
		}
		
		public string GetSuitName() {
			return suit switch {
				1 => "clubs",
				2 => "diamonds",
				3 => "hearts",
				4 => "spades",
				_ => "?"
			};
		}

		public string GetCardName() {
			return GetRankName() + " of " + GetSuitName();
		}
	}

	public class Pack {
		List<Card> cards = new List<Card>();
		Random rng = new Random();

		public Pack() {
			for (int s = 1; s <= 4; s++) {
				for(int r = 1; r <= 13; r++) {
					cards.Add(new Card(r, s));
				}
			}
		}

		public void Shuffle() {
			for (int i = 0; i < cards.Count - 1; i++) {
                int r = rng.Next(i + 1, cards.Count);
                Card temp = cards[i];
                cards[i] = cards[r];
                cards[r] = temp;
            }
		}

		public Card DrawCard() {
			Card c = cards[cards.Count - 1];
			cards.RemoveAt(cards.Count - 1);
			return c;
		}
	}
}
