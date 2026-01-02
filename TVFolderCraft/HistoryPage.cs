using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace TVFolderCraft
{
    public partial class HistoryPage : Form
    {
        //create data tranfer class to get and set history data
        public class HistoryItem
        {
            public string Date { get; set; }
            public string Title { get; set; }
            public string Path { get; set; }
            public string Source { get; set; }
        }

        private NavigationHeader navigationHeader;
        private FooterPanel footerPanel;
        private Panel contentPanel;
        private Label historyTitleLabel;
        private Button loadMoreButton;
        private FlowLayoutPanel historyItemsPanel;
        private List<HistoryItem> historyItems;

        public HistoryPage()
        {
            InitializeComponent();
            SetupUI();
            RefreshHistory();
        }

        private void SetupUI()
        {
            // create navigation header
            navigationHeader = new NavigationHeader(this, "HistoryPage");
            this.Controls.Add(navigationHeader);

            // create footer panel
            footerPanel = new FooterPanel(this);
            this.Controls.Add(footerPanel);

            //create contenc panal
            CreateContentPanel();
        }
        //create content area
        private void CreateContentPanel()
        {
            contentPanel = new Panel
            {
                //Dock = DockStyle.Fill,
                Size = new Size(this.Width,this.Height-90),
                BackColor = Color.FromArgb(240, 242, 245),
                AutoScroll = true,
                Padding = new Padding(40, 20, 40, 20),
                BackgroundImage = Properties.Resources.backgrount,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            //history title with icon
            var titlePanel = new Panel
            {
                Size = new Size(this.Width - 80, 60),
                Location = new Point(0, 65),
                BackColor = Color.Transparent
            };

            var historyIcon = new Label
            {
                Text = "🕒",
                Font = new Font("Segoe UI", 24),
                AutoSize = true,
                Location = new Point(0, 15),
                BackColor = Color.Transparent
            };

            historyTitleLabel = new Label
            {
                Text = "History",
                Font = new Font("roboto", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 58, 138),
                AutoSize = true,
                Location = new Point(55, 20),
                BackColor = Color.Transparent
            };

            titlePanel.Controls.AddRange(new Control[] { historyIcon, historyTitleLabel });

            //history items container
            historyItemsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Location = new Point(0, 150),
                Width = this.Width - 80,
                BackColor = Color.Transparent
            };

            contentPanel.Controls.AddRange(new Control[] { titlePanel, historyItemsPanel });
            this.Controls.Add(contentPanel);
        }
        //load hostory data
        private void LoadHistoryItems()
        {
            //first clare panal
            historyItemsPanel.Controls.Clear();

            //load history data. calling HistoryManager class GetHistoryItems method
            historyItems = HistoryManager.GetHistoryItems();

            //if no history show default text
            if (historyItems.Count == 0)
            {
                //show empty state
                var emptyLabel = new Label
                {
                    Text = "No TV series created yet.\nCreate your first series using the Generate page!",
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.FromArgb(128, 128, 128),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                emptyLabel.Location = new Point((historyItemsPanel.Width - emptyLabel.PreferredWidth) / 2, 50);
                historyItemsPanel.Controls.Add(emptyLabel);
                return;
            }
            //looping and get one by one histry data to historyCard
            foreach (var item in historyItems)
            {
                var historyCard = CreateHistoryCard(item);
                historyItemsPanel.Controls.Add(historyCard);
            }
        }
        //create histry cards
        private Panel CreateHistoryCard(HistoryItem item)
        {
            var card = new Panel
            {
                Size = new Size(historyItemsPanel.Width - 20, 110), // increased height
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 15),
                Cursor = Cursors.Hand,
            };

            //date label
            var dateLabel = new Label
            {
                Text = item.Date,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(128, 128, 128),
                AutoSize = true,
                Location = new Point(20, 15),
                BackColor = Color.Transparent
            };

            //title label
            var titleLabel = new Label
            {
                Text = item.Title,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                AutoSize = true,
                Location = new Point(20, 35),
                BackColor = Color.Transparent
            };

            //source label
            var sourceLabel = new Label
            {
                Text = $"Source: {item.Source}",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = item.Source == "Internet" ? Color.FromArgb(70, 130, 250) : Color.FromArgb(102, 102, 102),
                AutoSize = true,
                Location = new Point(20, 60),
                BackColor = Color.Transparent
            };

            //path label
            var pathLabel = new Label
            {
                Text = item.Path,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(102, 102, 102),
                AutoSize = true,
                Location = new Point(20, 80),
                BackColor = Color.Transparent
            };

            //hover effects and click event
            card.MouseEnter += (s, e) => {
                card.BackColor = Color.FromArgb(210, 211, 212);
                card.Invalidate();
            };
            card.MouseLeave += (s, e) => {
                card.BackColor = Color.White;
                card.Invalidate();
            };
            
            card.Click += (s, e) => {
                try
                {
                    if (Directory.Exists(item.Path))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", item.Path);
                    }
                    else
                    {
                        MessageBox.Show($"Folder not found: {item.Path}\nIt may have been moved or deleted.",
                            "Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening folder: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            card.Controls.AddRange(new Control[] { dateLabel, titleLabel, sourceLabel, pathLabel });
            return card;
        }
        // refreshHistory 
        private void RefreshHistory()
        {
            try
            {
                LoadHistoryItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading history: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //refreshHistoryDisplay
        public void RefreshHistoryDisplay()
        {
            LoadHistoryItems();
        }
        
    }
}
