using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace DistributedAlgorithms
{
    /// <summary>
    /// Interaction logic for GridView.xaml
    /// </summary>
    public partial class GridView : UserControl
    {
        public delegate void DrageFinished(List<int> permutations);
        public event DrageFinished DragFinishedEvent;
        private List<int> permutations;
        private Label sourceLabel;
        private Canvas canvas;
        public GridView(Canvas canvas,List<string> content)
        {
            InitializeComponent();
            this.canvas = canvas;
            ReplaceContent(content);
            
        }

        public void ReplaceContent(List<string> content)
        {
            Grid_main.Children.Clear();
            for (int idx = 0; idx < content.Count; idx++)
            {
                Grid_main.RowDefinitions.Add(new RowDefinition());
                Label label = new Label();
                label.Content = content[idx];
                label.Height = 25;
                label.Background = Brushes.White;
                label.MouseLeftButtonUp += Label_MouseLeftButtonUp;
                label.MouseMove += Label_MouseMove;
                //label.GiveFeedback += Label_GiveFeedback;
                label.AllowDrop = true;
                label.DragEnter += Label_DragEnter;
                label.DragLeave += Label_DragLeave;
                label.DragOver += Label_DragOver;
                label.Drop += Label_Drop;
                Grid_main.Children.Add(label);
                Grid.SetRow(label, idx);
                Grid.SetColumn(label, 0);
            }
        }
        public Rect GetDimentions()
        {
            double width = 0;
            double height = 0;
            foreach (Label label in Grid_main.Children)
            {
                FormattedText formattedText = new FormattedText(
                                (string)label.Content,
                                System.Globalization.CultureInfo.GetCultureInfo("en-us"),
                                FlowDirection.LeftToRight,
                                new Typeface(label.FontFamily.ToString()),
                                label.FontSize,
                                Brushes.Black);
                width = (formattedText.Width + 12 > width) ? formattedText.Width + 12 : width;
                height += formattedText.Height + 10;
            }
            return new Rect(0,0,width,height);
        }

        private void Label_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            MoveToFront();
        }

        public void MoveToFront()
        {
            List<int> maxZ = canvas.Children.OfType<UIElement>().Select(x => Panel.GetZIndex(x)).ToList();

            foreach (Label label in Grid_main.Children)
            {
                Panel.SetZIndex(label, maxZ.Max() + 1);
            }
        }

        //Enabling the object to be drag source
        private void Label_MouseMove(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            sourceLabel = label;

            if (label != null && e.LeftButton == MouseButtonState.Pressed)
            {
                label.Foreground = Brushes.White;
                label.Background = Brushes.Blue;
                DragDrop.DoDragDrop(label,
                                 label.Content,
                                 DragDropEffects.Move);
                if (permutations is null)
                {
                    permutations = new List<int>();
                    for (int idx = 0; idx < Grid_main.Children.Count; idx++)
                    {
                        permutations.Add(idx);
                    }
                }
            }
        }

        //Enabling the object to be a drag target
        //The DragEnter event specifies how the target object will behave 
        // When the source object will pass on it
        private void Label_DragEnter(object sender, DragEventArgs e)
        {
            Label label = sender as Label;
            MessageRouter.ReportMessage("GridView", "In DragEnter source = " + sourceLabel.Content + " target = " + label.Content, "");
            if (label != null)
            {
                sourceLabel.Foreground = Brushes.Black;
                sourceLabel.Background = Brushes.White;

                label.Foreground = Brushes.White;
                label.Background = Brushes.Blue;
                ReOrder(label);
            }
        }

        // The DragOver event occurs in the target object continuously while the 
        // source object is passes on it
        private void Label_DragOver(object sender, DragEventArgs e)
        {
            
        }

        private void ReOrder(Label targetLabel)
        {
            MessageRouter.ReportMessage("GridView", "In ReOrder source = " + sourceLabel.Content + " target = " + targetLabel.Content, "");
            int sourceLabelRow = Grid.GetRow(sourceLabel);
            string sourceLabelContent = (string)sourceLabel.Content;
            int sourcePermutation = permutations[sourceLabelRow];
            int targetLabelRow = Grid.GetRow(targetLabel);
            if (sourceLabelRow < targetLabelRow)
            {
                for (int row = sourceLabelRow; row < targetLabelRow; row ++)
                {
                    Move(row + 1, row);
                    
                }
            }
            else if (sourceLabelRow > targetLabelRow)
            {
                for(int row = targetLabelRow; row < sourceLabelRow; row ++)
                {
                    Move(row, row + 1);                  
                }
            }
            targetLabel.Content = sourceLabel.Content;
            permutations[targetLabelRow] = sourcePermutation;
            sourceLabel = targetLabel;
        }

        public void Move(int fromIdx, int toIdx)
        {
            Label fromLabel = Grid_main.Children.Cast<Label>()
                                    .First(e => Grid.GetRow(e) == fromIdx + 1 && Grid.GetColumn(e) == 0);
            Label toLabel = Grid_main.Children.Cast<Label>()
               .First(e => Grid.GetRow(e) == toIdx && Grid.GetColumn(e) == 0);
            toLabel.Content = fromLabel.Content;
            permutations[toIdx] = permutations[fromIdx];
        }

        // The DragLeave event occurs in the target object when the source object
        // leaves it
        private void Label_DragLeave(object sender, DragEventArgs e)
        {
            Label label = sender as Label;
            MessageRouter.ReportMessage("GridView", "In DragLeave source = " + sourceLabel.Content + " target = " + label.Content, "");
            if (label != null)
            {
                if (sourceLabel != label)
                {
                    MessageRouter.ReportMessage("GridView", "In DragLeave changing the colors", "");
                    label.Foreground = Brushes.Black;
                    label.Background = Brushes.White;
                }
                //label.Content = _previousContent;
            }
        }

        // The Drop event occurs in the target when the mouse is released
        private void Label_Drop(object sender, DragEventArgs e)
        {
            Label label = (Label)sender;
            MessageRouter.ReportMessage("GridView", "In DragDrop source = " + sourceLabel.Content + " target = " + label.Content, "");
            if (label != null)
            {
                label.Foreground = Brushes.Black;
                label.Background = Brushes.White;
                string s = "The permutation list is : [";
                for (int idx = 0; idx < permutations.Count; idx++)
                {
                    s += permutations[idx].ToString() + ", ";
                }
                s = s.Substring(s.Length - 2);
                s += "]\n What do you want to do?";

                string result = CustomizedMessageBox.Show(new List<string> { s }, "GridView Message", new List<string> { "Continue", "End", "Quit" }, Icons.Question);
                switch (result)
                {
                    case "Continue":
                        break;
                    case "End":
                        DragFinishedEvent(permutations);
                        permutations = null;
                        break;
                    case "Quit":
                        permutations = null;
                        break;
                }
               
            }
        }
        
    }
}
