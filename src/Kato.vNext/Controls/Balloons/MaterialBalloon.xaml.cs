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

namespace Kato.vNext.Controls.Balloons
{
    /// <summary>
    /// Interaction logic for MaterialBalloon.xaml
    /// </summary>
    public partial class MaterialBalloon : UserControl
    {
        public MaterialBalloonModel Model { get; private set; }
        public MaterialBalloon(MaterialBalloonModel model)
        {
            InitializeComponent();
            DataContext = Model = model;
        }
    }

    public class MaterialBalloonModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
    }
}
